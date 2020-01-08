using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOULS.Server;
namespace FormatsServer
{
    static class ConvertHelper
    {
        public static TProto Convert<TProto, TSouls>(this TSouls file) where TProto : new()
        {
            TProto proto = new TProto();

            foreach (System.Reflection.FieldInfo soulsField in typeof(TSouls).GetFields())
            {
                System.Reflection.PropertyInfo protoField = typeof(TProto).GetProperty(soulsField.Name);

                if (soulsField.FieldType.IsPrimitive)
                {
                    protoField.SetValue(proto, soulsField.GetValue(file));
                }
                else
                {
                    throw new Exception("Field of type " + soulsField.FieldType + " is not supported");
                }
            }

            return proto;
        }
    }

    class FormatsServer : SOULS.Server.Server.ServerBase
    {
        public override global::System.Threading.Tasks.Task<SOULS.Server.GetResponse> GetRpc(global::SOULS.Server.GetRequest request, Grpc.Core.ServerCallContext context)
        {
            string path = @"G:\SteamLibrary\steamapps\common\DARK SOULS III\Game\" + request.Path.Split(':')[0];

            string innerPath = request.Path.Split(':').Length == 2 ? request.Path.Split(':')[1] : null;

            string extension = System.IO.Path.GetExtension(path);

            byte[] fileContents;

            if (extension == ".dcx")
            {
                string innerExtension = System.IO.Path.GetExtension(System.IO.Path.GetFileNameWithoutExtension(path));

                SoulsFormats.DCX.Type dcxType;

                var dcxContents = SoulsFormats.DCX.Decompress(path, out dcxType);

                

                if (innerExtension.EndsWith("bnd"))
                {
                    if (innerPath == null)
                    {
                        throw new Exception("No inner path specified when accessing BND");
                    }

                    SoulsFormats.IBinder binder;
                    if (SoulsFormats.BND3.Is(dcxContents))
                    {
                        binder = SoulsFormats.BND3.Read(dcxContents);
                    }
                    else if (SoulsFormats.BND4.Is(dcxContents))
                    {
                        binder = SoulsFormats.BND4.Read(dcxContents);
                    }
                    else
                    {
                        throw new Exception("Incorrect BND file " + path);
                    }

                    try
                    {
                        SoulsFormats.BinderFile binderFile = binder.Files.First(file => file.Name.EndsWith(innerPath, StringComparison.OrdinalIgnoreCase));
                        fileContents = binderFile.Bytes;
                        extension = System.IO.Path.GetExtension(innerPath);
                    }
                    catch
                    {
                        Console.WriteLine("Couldn't find file, first one is " + binder.Files.First().Name);
                        throw;
                    }
                }
                else
                {
                    throw new Exception("Unsupported inner extension " + innerExtension);
                }
            }
            else
            {
                var stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
                var binaryReader = new System.IO.BinaryReader(stream);
                fileContents = binaryReader.ReadBytes((int)stream.Length);
            }

            //using ()
            {

                Tuple<Type, Type> ProtoAndSoulsTypePair;
                

                switch (extension)
                {
                    case ".hkx":
                        ProtoAndSoulsTypePair = new Tuple<Type, Type>(typeof(SOULS.HKX.HKASplineCompressedAnimation), typeof(SoulsFormats.HKX));
                        break;

                    default:
                        throw new Exception("Type " + extension + " unrecognized!");
                }

                var methods = typeof(SoulsFormats.SoulsFile<>).MakeGenericType(ProtoAndSoulsTypePair.Item2).GetMethods();

                object retData = methods.First(method => method.IsStatic && method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(byte[]) && method.Name == "Read").Invoke(null, new object[] { fileContents });

                if (extension == ".hkx")
                {
                    ProtoAndSoulsTypePair = new Tuple<Type, Type>(typeof(SOULS.HKX.HKASplineCompressedAnimation), typeof(SoulsFormats.HKX.HKASplineCompressedAnimation));// new Tuple<Type, Type>(.Item2 = typeof(SoulsFormats.HKX.HKASplineCompressedAnimation);
                    retData = ((SoulsFormats.HKX.HKASplineCompressedAnimation) ((SoulsFormats.HKX)(retData)).DataSection.VirtualReferences.First(reference => reference.ClassName.ClassName == "hkaSplineCompressedAnimation").SourceObject);
                }

                //var bhd = SoulsFormats.BHD5.Read(stream, SoulsFormats.BHD5.Game.DarkSouls3);

                SOULS.Server.GetResponse res = new GetResponse();

                object protoConverted = typeof(ConvertHelper).GetMethod("Convert").MakeGenericMethod(ProtoAndSoulsTypePair.Item1, ProtoAndSoulsTypePair.Item2).Invoke(null, new object[] { retData });

                res.Data = Google.Protobuf.WellKnownTypes.Any.Pack((Google.Protobuf.IMessage)protoConverted);

                //res.Data = Google.Protobuf.WellKnownTypes.Any.Pack(ConvertHelper.Convert<SOULS.BHD5.Bucket, SoulsFormats.BHD5.Bucket>(bhd.Buckets[0]));
                Console.WriteLine("Result are in, handling...");
                return Task.FromResult(res);
            };

            throw new Exception("something");
        }
    }

    //class RouteGuide : Grpc.Core.

    class Program
    {
        static void Main(string[] args)
        {
            var grpcServer = new Grpc.Core.Server()
            {
                Services = { SOULS.Server.Server.BindService(new FormatsServer()) },
                Ports = { new Grpc.Core.ServerPort("localhost", 50001, Grpc.Core.ServerCredentials.Insecure) }
            };

            grpcServer.Start();

            Console.WriteLine("Server is working");
            Console.WriteLine("Press any key to stop");

            Console.ReadKey();
            Console.WriteLine("Stopping...");

            grpcServer.ShutdownAsync().Wait();
            Console.WriteLine("Stopped.");
        }
    }
}
