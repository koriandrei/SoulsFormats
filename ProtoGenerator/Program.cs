using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace ProtoGenerator
{
    public class FieldGenerator
    {
        public FieldIds Ids { get; }

        public struct FieldIds
        {
            public struct FieldId
            {
                public Type type;
                public string name;
                public int id;
            }

            public List<FieldId> fields;
        }

        public FieldGenerator(FieldIds ids)
        {
            Ids = ids;
        }

        public FieldGenerator() : this(new FieldIds() { fields = new List<FieldIds.FieldId>() })
        {
        }

        internal FieldIds.FieldId GenerateField(Type type, string name)
        {
            try
            {
                return Ids.fields.First(existingFieldId => existingFieldId.type == type && existingFieldId.name == name);
            }
            catch (InvalidOperationException)
            {
            }
            int maxId = 0;
            try
            {
                maxId = Ids.fields.Max(existingFieldId => existingFieldId.id);
            }
            catch (InvalidOperationException) { }
            FieldIds.FieldId fieldId = new FieldIds.FieldId{type = type, name = name, id = maxId + 1};

            Ids.fields.Add(fieldId);

            return fieldId;
        }
        
    }

    //class GenericTypeProto
    //{
    //    Type specializedType;

    //    Type genericType => specializedType.GetGenericTypeDefinition();

    //    Type[] args => specializedType.GenericTypeArguments;

    //    string GenerateTypeName()
    //    {
    //        return genericType.Name + "_" + string.Join("_", args.Select(type => type.Name));
    //    }

    //    string GenerateMessage()
    //    {
    //        string messageFormat = @"
    //            message {0} {
    //                {1}
    //            }
    //        ";

    //        string messageBody = "";

    //        genericType.GetFields().Select(field=>field.FieldType);

    //        string.Format(messageFormat, GenerateTypeName(), messageBody);
    //    }
    //}

    class Program
    {
        static IDictionary<Type, string> predefinedTypes;

        static Program()
        {
            predefinedTypes = new Dictionary<Type, string> {
                { typeof(int), "int32" },
                { typeof(short), "int32" },
                { typeof(ushort), "uint32" },
                { typeof(string), "string" },
                { typeof(float), "float" },
                { typeof(double), "double" },
                { typeof(bool), "bool" },
                { typeof(byte), "int32" },
                { typeof(uint), "uint32" },
                { typeof(long), "int64" },
                //{ typeof(System.Numerics.Vector2), "Souls.Vector2" },
                //{ typeof(System.Numerics.Vector3), "Souls.Vector3" },
                //{ typeof(System.Numerics.Quaternion), "Souls.Quaternion" },
                //{ typeof(System.Drawing.Color), "Souls.Color" },
            }; 
        }



        static string GenerateProtoTypeName(Type message, Type type)
        {
            if (predefinedTypes.ContainsKey(type))
            {
                return predefinedTypes[type];
            }

            if (type.IsArray)
            {
                string elementProtoType = GenerateProtoTypeName(message, type.GetElementType());
                return "repeated " + elementProtoType;
            }

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
                {
                    string elementProtoType = GenerateProtoTypeName(message, type.GetGenericArguments()[0]);
                    return "repeated " + elementProtoType;
                }

                if (type.GetGenericTypeDefinition() == typeof(System.Collections.Generic.Dictionary<,>))
                {
                    string keyProtoType = GenerateProtoTypeName(message, type.GetGenericArguments()[0]);
                    string valueProtoType = GenerateProtoTypeName(message, type.GetGenericArguments()[1]);
                    return "map<" + keyProtoType + ", " + valueProtoType + ">";
                }

                return type.Name.Substring(0, type.Name.Length - 2) + "__" + string.Join("_", type.GetGenericArguments().Select(genericType => genericType.Name));
            }

            string relativeName = type.Name;

            if (type == message)
            {
                return relativeName;
            }

            Type commonType = type.DeclaringType;

            if (commonType == null && type != message)
            {
                return relativeName + "." + relativeName;
            }

            while (commonType != null && commonType != message)
            {
                relativeName = commonType.Name + "." + relativeName;

                commonType = commonType.DeclaringType;
            }

            return relativeName;
        }

        static string GenerateProtoField(Type message, FieldGenerator gen, Type type, string name)
        {
            var field = gen.GenerateField(type, name);
            
            return string.Format("{0} {1} = {2};", GenerateProtoTypeName(message, field.type), field.name, field.id);
        }

        static string GenerateProtoField(Type type, FieldGenerator gen, FieldInfo field)
        {
            return GenerateProtoField(type, gen, field.FieldType, field.Name);
        }

        private static string GenerateProtoProp(Type type, FieldGenerator gen, PropertyInfo prop)
        {
            return GenerateProtoField(type, gen, prop.PropertyType, prop.Name);
        }

        static string GenerateProtoMessage(Type type)
        {
            string nestedMessages = string.Join(Environment.NewLine, type.GetNestedTypes().Select(nestedType => GenerateProtoMessage(nestedType)));

            FieldGenerator gen = new FieldGenerator();
            
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SoulsFormats.HKX.HKArray<>))
            {

            }

            IEnumerable<FieldInfo> fields = type.GetFields(/*BindingFlags.NonPublic |*/ BindingFlags.Public | BindingFlags.Instance).Where(field=>!field.Name.Contains("__BackingField"));

            IEnumerable<PropertyInfo> properties = type.GetProperties(/*BindingFlags.NonPublic |*/ BindingFlags.Public | BindingFlags.Instance);


            string fieldString = string.Join(Environment.NewLine, fields.Where(field=>!field.IsStatic).Select(field => GenerateProtoField(type, gen, field)).Concat(properties/*.Where(prop=>prop.GetMethod.IsPublic)*/.Select(prop => GenerateProtoProp(type, gen, prop))));

            const string protoMessageFormat = @"
                message {0} {{
                {1}

                {2}
            }}";

            return string.Format(protoMessageFormat, GenerateProtoTypeName(type, type), nestedMessages, fieldString);
        }

        static IEnumerable<Type> GetAllNestedTypes(Type type)
        {
            List<Type> allNestedTypes = new List<Type> { type };

            IEnumerable<Type> currentNestedTypes = new[] { type };

            while (true)
            {
                IEnumerable<Type> nestedTypes = currentNestedTypes.SelectMany(nestedType => nestedType.GetNestedTypes());

                if (nestedTypes.Count() == 0)
                {
                    break;
                }

                currentNestedTypes = nestedTypes;

                allNestedTypes.AddRange(nestedTypes);
            }

            return allNestedTypes.Distinct();
        }

        static IEnumerable<Type> GetUsedTypes(Type type)
        {
            Func<Type, IEnumerable<Type>> selector = (t => t.GetFields().SelectMany(field => (field.FieldType.IsGenericType ? field.FieldType.GetGenericArguments() : new Type[] { }).Concat(new[] { field.FieldType })).Concat(t.GetProperties().SelectMany(prop => new[] { prop.PropertyType }.Concat(prop.PropertyType.IsGenericType ? prop.PropertyType.GetGenericArguments() : new Type[] { }))));

            return selector(type);
        }

        static IEnumerable<Type> GetAllUsedTypes(Type type)
        {
            IEnumerable<Type> nestedTypes = GetAllNestedTypes(type);

            IEnumerable<Type> types = nestedTypes.SelectMany(GetUsedTypes).Concat(nestedTypes).Distinct();

            while (true)
            {
                IEnumerable<Type> super = types.SelectMany(GetUsedTypes).Concat(types).Distinct();

                if (types.Count() == super.Count())
                {
                    return types;
                }

                types = super;
            }
        }

        static IEnumerable<Type> GetPackageTypes(IEnumerable<Type> typesToConsider)
        {
            List<Type> actualPackageTypes = new List<Type>();

            while (true)
            {
                actualPackageTypes.AddRange(typesToConsider.Where(type => type.DeclaringType == null));
                typesToConsider = typesToConsider.Where(type => type.DeclaringType != null).Select(type => type.DeclaringType);

                if (typesToConsider.Count() == 0)
                {
                    break;
                }
            }

            return actualPackageTypes;
        }

        static IEnumerable<Type> GetUsedPackageTypes(Type type)
        {
            return GetPackageTypes(GetAllUsedTypes(type));
        }

        static bool IsDeclaredInSamePackage(Type type, Type package)
        {
            while (type != null && type != package)
            {
                type = type.DeclaringType;
            }

            return type != null;
        }

        static Type GetPackageType(Type type)
        {
            while (type.DeclaringType != null)
            {
                type = type.DeclaringType;
            }

            return type;
        }

        static void Main(string[] args)
        {
            System.Reflection.Assembly formats = System.Reflection.Assembly.Load("SoulsFormats");

            string[] typesToTreatAsModule = new[] { /*"FLVER",*/ "FLVER2", "DCX", "HKX", "BND4", "BHD5", };

            IEnumerable<Type> packageTypes = formats.ExportedTypes.Where((type) => typesToTreatAsModule.Contains(type.Name));


            IEnumerable<Type> allUsedTypes = packageTypes.SelectMany(type => GetAllUsedTypes(type)).Distinct();
            IEnumerable<Type> actualPackageTypes = GetPackageTypes(allUsedTypes).Distinct();

            foreach (var item in actualPackageTypes.Distinct().Where(type => type.Assembly != typeof(object).Assembly))
            {
                Console.WriteLine("Exported {0}", item);

                

                IEnumerable<Type> inPackageTypes = GetAllUsedTypes(item).Distinct().Where(type=> IsDeclaredInSamePackage(type, item)).Except(new[] { item });

                if (inPackageTypes.Count() == 0)
                {
                    inPackageTypes = new[] { item };
                }

                List<string> allImports = new List<string>();

                string allMessages = "";

                foreach (Type inPackageType in inPackageTypes.Where(type => !type.IsGenericParameter && !type.IsGenericTypeDefinition && (!type.IsGenericType || type.IsConstructedGenericType ) && !type.ContainsGenericParameters))
                {
                    Console.WriteLine("\tProcessing message type: {0}", inPackageType);

                    string protoMessage = GenerateProtoMessage(inPackageType);

                    IEnumerable<Type> usedPackageTypes = GetAllNestedTypes(inPackageType).SelectMany(x => GetUsedTypes(x).Where(type => type != inPackageType && type.Assembly != typeof(object).Assembly).Except(GetAllNestedTypes(inPackageType))).Select(type => GetPackageType(type)).Distinct().Except(new[] { item });

                    //string imports = string.Join(Environment.NewLine, );

                    allMessages += protoMessage;

                    allImports.AddRange(usedPackageTypes.Select(type => "import \"" + GenerateProtoTypeName(type, type) + ".proto\";"));

                    //protoMessage = String.Format(@"
                    //    syntax = ""proto3"";

                    //    package {0};

                    //    {1}
                    //    ", GenerateProtoTypeName(item, item), imports) + protoMessage;


                    //string protoFileName = GenerateProtoTypeName(inPackageType, inPackageType) + ".proto";

                    //using (var writer = new System.IO.StreamWriter(protoFileName))
                    //{
                    //    writer.WriteLine(protoMessage);
                    //}
                }


                string finalProtoMessage = String.Format(@"
                        syntax = ""proto3"";

                        package SOULS.{0};

                        {1}
                        ", GenerateProtoTypeName(item, item), string.Join(Environment.NewLine, allImports.Distinct())) + allMessages;

                string protoFileName = GenerateProtoTypeName(item, item) + ".proto";

                using (var writer = new System.IO.StreamWriter(protoFileName))
                {
                    writer.WriteLine(finalProtoMessage);
                }

            }

            List<Type> packagesToGenerateConverts = new List<Type>();

            foreach (var item in actualPackageTypes.Distinct().Where(type => type.Assembly != typeof(object).Assembly))
            {
                Console.WriteLine("Exported {0}", item);

                IEnumerable<Type> inPackageTypes = item.GetNestedTypes();

                if (inPackageTypes.Count() == 0)
                {
                    inPackageTypes = new[] { item };
                }
                //foreach (Type inPackageType in inPackageTypes)
                {
                    Type inPackageType = item;

                    //const string cmdPath = "cmd.exe";
                    const string protocPath = "G:/work/dks/tools/SoulsFormats/packages/Grpc.Tools.2.26.0/tools/windows_x64/protoc.exe";
                    string protoFileName = GenerateProtoTypeName(inPackageType, inPackageType) + ".proto";

                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = protocPath,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        Arguments = "--csharp_out=. " + protoFileName
                    };

                    System.Diagnostics.Process protoc = new System.Diagnostics.Process();

                    protoc.StartInfo = startInfo;
                    protoc.Start();

                    bool processFinished = protoc.WaitForExit(1000);

                    //protoc.Kill();
                    bool processFinishedAfterKilling = protoc.WaitForExit(100);

                    if ((processFinished || processFinishedAfterKilling))
                    {
                        if (protoc.ExitCode == 0)
                        {
                            using (var reader = new System.IO.StreamReader(protoFileName))
                            {
                            }
                            Console.WriteLine("Generated proto for {0} successfully", inPackageType);
                            packagesToGenerateConverts.Add(inPackageType);
                        }
                        else
                        {
                            Console.WriteLine("An error occurred while generating proto for {0}", inPackageType);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot generate proto for {0}", inPackageType);
                    }

                    protoc.Close();
                }
            }
       
            //foreach (var package in packagesToGenerateConverts)
            //{
            //    string protoFileName = GenerateProtoTypeName(package, package) + ".proto";

            //    string generated;

            //    using (var reader = new System.IO.StreamReader(protoFileName))
            //    {
            //        generated = reader.ReadToEnd();
            //    }


            //}
        }
    }
}
