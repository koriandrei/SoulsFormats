﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SoulsFormats
{
    public partial class TAE
    {
        /// <summary>
        /// Template for the parameters in an event.
        /// </summary>
        public class Template : Dictionary<long, Template.BankTemplate>
        {
            /// <summary>
            /// The game(s) this template is for.
            /// </summary>
            public TAEFormat Game;

            /// <summary>
            /// Creates new empty template.
            /// </summary>
            public Template()
                : base()
            {

            }

            private Template(XmlDocument xml)
                : base()
            {
                XmlNode templateNode = xml.SelectSingleNode("event_template");
                Game = (TAEFormat)Enum.Parse(typeof(TAEFormat), templateNode.Attributes["game"].InnerText);

                Dictionary<BankTemplate, long> basedOnMap = new Dictionary<BankTemplate, long>();

                foreach (XmlNode bankNode in templateNode.SelectNodes("bank"))
                {
                    var newBank = new BankTemplate(bankNode, out long basedOn);
                    basedOnMap.Add(newBank, basedOn);
                    if (ContainsKey(newBank.ID))
                    {
                        throw new Exception($"TAE Template has more than one bank with ID {newBank.ID}.");
                    }
                    Add(newBank.ID, newBank);
                }

                foreach (var kvp in basedOnMap)
                {
                    if (kvp.Value != -1)
                    {
                        foreach (var importFromKvp in this[kvp.Value])
                        {
                            if (!kvp.Key.ContainsKey(importFromKvp.Key))
                            {
                                kvp.Key.Add(importFromKvp.Key, importFromKvp.Value);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Read a TAE template from an XML file.
            /// </summary>
            public static Template ReadXMLFile(string path)
            {
                var xml = new XmlDocument();
                xml.Load(path);
                return new Template(xml);
            }

            /// <summary>
            /// Read a TAE template from an XML string.
            /// </summary>
            public static Template ReadXMLText(string text)
            {
                var xml = new XmlDocument();
                xml.LoadXml(text);
                return new Template(xml);
            }

            /// <summary>
            /// Read a TAE template from an XML document.
            /// </summary>
            public static Template ReadXMLDoc(XmlDocument xml)
            {
                return new Template(xml);
            }

            /// <summary>
            /// A template for a bank of events.
            /// </summary>
            public class BankTemplate : Dictionary<long, EventTemplate>
            {
                /// <summary>
                /// ID of this bank template.
                /// </summary>
                public long ID;

                /// <summary>
                /// Name of this bank template.
                /// </summary>
                public string Name;

                internal BankTemplate(XmlNode bankNode, out long basedOn)
                    : base()
                {
                    ID = long.Parse(bankNode.Attributes["id"].InnerText);
                    Name = bankNode.Attributes["name"].InnerText;

                    basedOn = long.Parse(bankNode.Attributes["basedon"]?.InnerText ?? "-1");

                    foreach (XmlNode eventNode in bankNode.SelectNodes("event"))
                    {
                        var newEvent = new EventTemplate(ID, eventNode);
                        if (ContainsKey(newEvent.ID))
                        {
                            throw new Exception($"TAE Bank Template has more than one event with ID {newEvent.ID}.");
                        }
                        Add(newEvent.ID, newEvent);
                    }
                }
            }

            /// <summary>
            /// Info about a parameter supplied to a TAE event.
            /// </summary>
            public class ParameterTemplate
            {
                /// <summary>
                /// Gets the byte count of a specific value type.
                /// </summary>
                public int GetByteCount()
                {
                    switch (Type)
                    {
                        case ParamType.s8:
                        case ParamType.u8:
                        case ParamType.x8:
                            return 1;
                        case ParamType.s16:
                        case ParamType.u16:
                        case ParamType.x16:
                            return 2;
                        case ParamType.s32:
                        case ParamType.u32:
                        case ParamType.x32:
                        case ParamType.f32:
                            return 4;
                        case ParamType.s64:
                        case ParamType.u64:
                        case ParamType.x64:
                        case ParamType.f64:
                            return 8;
                        case ParamType.aob:
                            return AobLength;
                        default: throw new ArgumentException("Not a real ParamType");
                    }
                }

                /// <summary>
                /// Converts a string to a value based on this ParameterTemplate's type.
                /// </summary>
                public object StringToValue(string str)
                {
                    IEnumerable<string> GetArrayFromSingleLineString(string s)
                    {
                        return s.Split(' ')
                            .Where(st => !string.IsNullOrWhiteSpace(st))
                            .Select(st => st.Trim());
                    }

                    List<string> GetArrayFromString(string s)
                    {
                        List<string> result = new List<string>();
                        var lines = s.Split('\n');
                        foreach (var l in lines)
                            result.AddRange(GetArrayFromSingleLineString(l.Replace("\r", "").Replace("\n", "").Replace("\t", "")));
                        return result;
                    }

                    switch (Type)
                    {
                        case ParamType.aob: return GetArrayFromString(str).Select(b => byte.Parse(b, System.Globalization.NumberStyles.HexNumber)).ToArray();
                        case ParamType.u8: return byte.Parse(str);
                        case ParamType.x8: return byte.Parse(str, System.Globalization.NumberStyles.HexNumber);
                        case ParamType.s8: return sbyte.Parse(str);
                        case ParamType.u16: return ushort.Parse(str);
                        case ParamType.x16: return ushort.Parse(str, System.Globalization.NumberStyles.HexNumber);
                        case ParamType.s16: return short.Parse(str);
                        case ParamType.u32: return uint.Parse(str);
                        case ParamType.x32: return uint.Parse(str, System.Globalization.NumberStyles.HexNumber);
                        case ParamType.s32: return int.Parse(str);
                        case ParamType.u64: return ulong.Parse(str);
                        case ParamType.x64: return ulong.Parse(str, System.Globalization.NumberStyles.HexNumber);
                        case ParamType.s64: return long.Parse(str);
                        case ParamType.f32: return float.Parse(str);
                        case ParamType.f64: return double.Parse(str);
                        default: throw new Exception($"Invalid ParamTemplate ParamType: {Type.ToString()}");
                    }
                }

                /// <summary>
                /// Converts a value to a string based on this ParameterTemplate's type.
                /// </summary>
                public string ValueToString(object val)
                {
                    switch (Type)
                    {
                        case ParamType.aob: return string.Join(" ", ((byte[])val).Select(b => b.ToString("X2")));
                        case ParamType.x8: return ((byte)val).ToString("X2");
                        case ParamType.x16: return ((ushort)val).ToString("X4");
                        case ParamType.x32: return ((uint)val).ToString("X8");
                        case ParamType.x64: return ((ulong)val).ToString("X16");
                        default: return val.ToString();
                    }
                }

                internal void WriteValue(BinaryWriterEx bw, object value)
                {
                    switch (Type)
                    {
                        case ParamType.aob: bw.WriteBytes((byte[])value); break;
                        case ParamType.u8: case ParamType.x8: bw.WriteByte((byte)value); break;
                        case ParamType.s8: bw.WriteSByte((sbyte)value); break;
                        case ParamType.u16: case ParamType.x16: bw.WriteUInt16((ushort)value); break;
                        case ParamType.s16: bw.WriteInt16((short)value); break;
                        case ParamType.u32: case ParamType.x32: bw.WriteUInt32((uint)value); break;
                        case ParamType.s32: bw.WriteInt32((int)value); break;
                        case ParamType.u64: case ParamType.x64: bw.WriteUInt64((ulong)value); break;
                        case ParamType.s64: bw.WriteInt64((long)value); break;
                        case ParamType.f32: bw.WriteSingle((float)value); break;
                        case ParamType.f64: bw.WriteDouble((double)value); break;
                        default: throw new Exception($"Invalid ParamTemplate ParamType: {Type.ToString()}");
                    }
                }

                internal object ReadValue(BinaryReaderEx br)
                {
                    object value = null;
                    switch (Type)
                    {
                        case ParamType.aob: return br.ReadBytes(AobLength);
                        case ParamType.u8: case ParamType.x8: return br.ReadByte();
                        case ParamType.s8: return br.ReadSByte();
                        case ParamType.u16: case ParamType.x16: return br.ReadUInt16();
                        case ParamType.s16: return br.ReadInt16();
                        case ParamType.u32: case ParamType.x32: return br.ReadUInt32();
                        case ParamType.s32: return br.ReadInt32();
                        case ParamType.u64: case ParamType.x64: return br.ReadUInt64();
                        case ParamType.s64: return br.ReadInt64();
                        case ParamType.f32: return br.ReadSingle();
                        case ParamType.f64: return br.ReadDouble();
                        default: throw new Exception($"Invalid ParamTemplate ParamType: {Type.ToString()}");
                    }
                }

                internal void AssertValue(BinaryReaderEx br)
                {
                    switch (Type)
                    {
                        case ParamType.aob:
                            var assertAob = (byte[])ValueToAssert;
                            for (int i = 0; i < AobLength; i++)
                            {
                                br.AssertByte(assertAob[i]);
                            }
                            break;
                        case ParamType.u8: case ParamType.x8: br.AssertByte((byte)ValueToAssert); break;
                        case ParamType.s8: br.AssertSByte((sbyte)ValueToAssert); break;
                        case ParamType.u16: case ParamType.x16: br.AssertUInt16((ushort)ValueToAssert); break;
                        case ParamType.s16: br.AssertInt16((short)ValueToAssert); break;
                        case ParamType.u32: case ParamType.x32: br.AssertUInt32((uint)ValueToAssert); break;
                        case ParamType.s32: br.AssertInt32((int)ValueToAssert); break;
                        case ParamType.u64: case ParamType.x64: br.AssertUInt64((ulong)ValueToAssert); break;
                        case ParamType.s64: br.AssertInt64((long)ValueToAssert); break;
                        case ParamType.f32: br.AssertSingle((float)ValueToAssert); break;
                        case ParamType.f64: br.AssertDouble((double)ValueToAssert); break;
                        default: throw new Exception($"Invalid ParamTemplate ParamType: {Type.ToString()}");
                    }
                }

                /// <summary>
                /// The value type of this parameter.
                /// </summary>
                public ParamType Type;

                /// <summary>
                /// The name of this parameter.
                /// </summary>
                public string Name;

                /// <summary>
                /// (Optional) The value which should be asserted on this parameter.
                /// </summary>
                public object ValueToAssert = null;

                /// <summary>
                /// (Only applies if Type == ParamType.aob)
                /// The length of the array of bytes.
                /// </summary>
                public int AobLength = -1;

                /// <summary>
                /// Possible values if this is an enum, otherwise it's null.
                /// </summary>
                public Dictionary<string, object> EnumEntries { get; private set; } = null;

                internal ParameterTemplate(long bankId, long eventId, long paramIndex, XmlNode paramNode)
                {
                    Type = (ParamType)Enum.Parse(typeof(ParamType), paramNode.Name);

                    Name = paramNode.Attributes["name"]?.InnerText;

                    if (paramNode.HasChildNodes)
                    {
                        var valueNode = paramNode.SelectSingleNode("value");
                        if (valueNode != null)
                        {
                            ValueToAssert = StringToValue(valueNode.InnerText);
                        }
                    }

                    if (ValueToAssert == null)
                        ValueToAssert = paramNode.Attributes["value"]?.InnerText;

                    var lengthAttribute = paramNode.Attributes["length"];
                    if (lengthAttribute != null)
                        AobLength = int.Parse(lengthAttribute.InnerText);

                    if (Type == ParamType.aob && ValueToAssert != null)
                    {
                        var aob = (byte[])ValueToAssert;
                        if (aob.Length != AobLength)
                        {
                            throw new Exception($"Bank {bankId} -> Event {eventId} -> Parameter '{Name}': " +
                                $"AoB assert value length was {aob.Length} but 'length' " +
                                $"attribute was set to {AobLength}.");
                        }
                    }
                    var enumNodes = paramNode.SelectNodes("entry");
                    if (enumNodes.Count > 0)
                    {
                        EnumEntries = new Dictionary<string, object>();
                        foreach (XmlNode entryNode in paramNode.SelectNodes("entry"))
                        {
                            var entryName = entryNode.Attributes["name"].InnerText;
                            var entryValue = StringToValue(entryNode.Attributes["value"].InnerText);
                            EnumEntries.Add(entryName, entryValue);
                        }
                    }
                }
            }

            /// <summary>
            /// Info about an event in a TAE file.
            /// </summary>
            public class EventTemplate : Dictionary<string, ParameterTemplate>
            {
                /// <summary>
                /// ID of this TAE event.
                /// </summary>
                public readonly long ID;

                /// <summary>
                /// Name of this TAE event.
                /// </summary>
                public string Name;

                /// <summary>
                /// Gets the byte count of the entire list of parameters.
                /// </summary>
                public int GetAllParametersByteCount()
                {
                    int result = 0;
                    foreach (var paramKvp in this)
                    {
                        result += paramKvp.Value.GetByteCount();
                    }
                    return result;
                }

                internal EventTemplate(long bankId, XmlNode eventNode)
                    : base()
                {
                    ID = long.Parse(eventNode.Attributes["id"].InnerText);
                    Name = eventNode.Attributes["name"].InnerText;
                    int i = 0;
                    foreach (XmlNode paramNode in eventNode.ChildNodes)
                    {
                        var newParam = new ParameterTemplate(bankId, ID, i++, paramNode);
                        Add(newParam.Name, newParam);
                    }
                }
            }

            /// <summary>
            /// Possible types for values in an event parameter.
            /// </summary>
            public enum ParamType
            {
                /// <summary>
                /// Unsigned byte.
                /// </summary>
                u8,

                /// <summary>
                /// Unsigned byte, display as hex.
                /// </summary>
                x8,

                /// <summary>
                /// Signed byte.
                /// </summary>
                s8,

                /// <summary>
                /// Unsigned short.
                /// </summary>
                u16,

                /// <summary>
                /// Unsigned short, display as hex.
                /// </summary>
                x16,

                /// <summary>
                /// Signed short.
                /// </summary>
                s16,

                /// <summary>
                /// Unsigned int.
                /// </summary>
                u32,

                /// <summary>
                /// Unsigned int, display as hex.
                /// </summary>
                x32,

                /// <summary>
                /// Signed int.
                /// </summary>
                s32,

                /// <summary>
                /// Unsigned long.
                /// </summary>
                u64,

                /// <summary>
                /// Unsigned long, display as hex.
                /// </summary>
                x64,

                /// <summary>
                /// Signed long.
                /// </summary>
                s64,

                /// <summary>
                /// Single-precision float.
                /// </summary>
                f32,

                /// <summary>
                /// Double-precision float.
                /// </summary>
                f64,

                /// <summary>
                /// Array of bytes.
                /// </summary>
                aob,
            }

        }
    }
}
