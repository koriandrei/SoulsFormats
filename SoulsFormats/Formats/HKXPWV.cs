using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulsFormats
{
    /// <summary>
    /// Some ragdoll-related container found in Dark Souls 1, Dark Souls III, and Bloodborne
    /// </summary>
    public class HKXPWV : SoulsFile<HKXPWV>
    {
        /// <summary>
        /// The game this HKXPWV is for.
        /// </summary>
        public enum GameType
        {
            /// <summary>
            /// Dark Souls
            /// </summary>
            DS1,
            /// <summary>
            /// Bloodborne
            /// </summary>
            BB,
            /// <summary>
            /// Dark Souls III
            /// </summary>
            DS3
        }
        /// <summary>
        /// Whether the file is big endian.
        /// </summary>
        public bool BigEndian { get; set; }

        /// <summary>
        /// Which game this file is for.
        /// </summary>
        public GameType Game { get; set; }

        /// <summary>
        /// Unknown in header. Can be 0, 1, 2, or 5. Is 0 typically.
        /// </summary>
        public uint UnkC { get; set; }

        /// <summary>
        /// Amount of bone map entries going between the anim skeleton and the ragdoll skeleton.
        /// A list of constant-value structs, with bytes FF FF 00 00 for each, 
        /// is generated automatically.
        /// </summary>
        public ushort BoneMapEntryCount { get; set; }

        /// <summary>
        /// List of items which affect bone map entries in the HKX.
        /// </summary>
        public List<BoneMapEntry> BoneMapEntries { get; set; }

        /// <summary>
        /// List of items which affect bones in the animation skeleton.
        /// </summary>
        public List<AnimBoneEntry> AnimBoneEntries { get; set; }

        /// <summary>
        /// List of items which affect bones in the ragdoll skeleton.
        /// </summary>
        public List<RagdollBoneEntry> RagdollBoneEntries { get; set; }

        /// <summary>
        /// Creates a new empty HKXPWV.
        /// </summary>
        public HKXPWV()
        {
            BoneMapEntries = new List<BoneMapEntry>();
            AnimBoneEntries = new List<AnimBoneEntry>();
            RagdollBoneEntries = new List<RagdollBoneEntry>();
        }

        internal override bool Is(BinaryReaderEx br)
        {
            throw new NotImplementedException();
        }

        internal override void Read(BinaryReaderEx br)
        {
            // Value should be 1 in either endianness
            int versionCheck = br.AssertInt32(1, 0x1000000);
            if (versionCheck == 0x1000000)
            {
                BigEndian = true;
                br.BigEndian = true;
            }

            br.AssertInt16(0);

            BoneMapEntryCount = br.ReadUInt16();
            ushort animBoneCount = br.ReadUInt16();
            ushort ragdollBoneCount = br.ReadUInt16();

            if (br.Length == (0x20 + (BoneMapEntryCount * 8) + (animBoneCount * 8) + (ragdollBoneCount * 16)))
            {
                Game = GameType.BB;
            }
            else if (br.Length == (0x20 + (BoneMapEntryCount * 4) + (animBoneCount * 8) + (ragdollBoneCount * 16)))
            {
                Game = GameType.DS3;
            }
            else if (br.Length == (0x20 + (BoneMapEntryCount * 4) + (animBoneCount * 8) + (ragdollBoneCount * 8)))
            {
                Game = GameType.DS1;
            }
            else
            {
                throw new System.IO.InvalidDataException("Invalid struct sizes found in HKXPWV file.");
            }

            UnkC = br.AssertUInt32(0, 1, 2, 5);

            for (int i = 0; i < 4; i++)
                br.AssertInt32(0);

            for (int i = 0; i < BoneMapEntryCount; i++)
                BoneMapEntries.Add(new BoneMapEntry(br, Game));

            for (int i = 0; i < animBoneCount; i++)
                AnimBoneEntries.Add(new AnimBoneEntry(br));

            for (int i = 0; i < ragdollBoneCount; i++)
                RagdollBoneEntries.Add(new RagdollBoneEntry(br, Game));
        }

        internal override void Write(BinaryWriterEx bw)
        {
            bw.BigEndian = BigEndian;

            //Version
            bw.WriteInt32(1);

            bw.WriteInt16(0);

            bw.WriteUInt16(BoneMapEntryCount);
            bw.WriteUInt16((ushort)AnimBoneEntries.Count);
            bw.WriteUInt16((ushort)RagdollBoneEntries.Count);

            bw.WriteUInt32(UnkC);

            for (int i = 0; i < 4; i++)
                bw.WriteInt32(0);

            foreach (var boneMapEntry in BoneMapEntries)
                boneMapEntry.Write(bw, Game);

            foreach (var animBoneEntry in AnimBoneEntries)
                animBoneEntry.Write(bw);

            foreach (var ragdollBoneEntry in RagdollBoneEntries)
                ragdollBoneEntry.Write(bw, Game);
        }

        /// <summary>
        /// Includes parameters for a bone map entry in the HKX.
        /// </summary>
        public class BoneMapEntry
        {
            /// <summary>
            /// Unknown
            /// </summary>
            public int UnknownValueA { get; set; } = -1;

            /// <summary>
            /// Unknown
            /// </summary>
            public int UnknownValueB { get; set; } = 0;

            /// <summary>
            /// Creates a default BoneMapEntry with default values.
            /// </summary>
            public BoneMapEntry()
            {

            }

            internal BoneMapEntry(BinaryReaderEx br, GameType game)
            {
                if (game == GameType.BB)
                {
                    UnknownValueA = br.ReadInt32();
                    UnknownValueB = br.ReadInt32();
                }
                else
                {
                    UnknownValueA = br.ReadInt16();
                    UnknownValueB = br.ReadInt16();
                }
            }

            internal void Write(BinaryWriterEx bw, GameType game)
            {
                if (game == GameType.BB)
                {
                    bw.WriteInt32(UnknownValueA);
                    bw.WriteInt32(UnknownValueB);
                }
                else
                {
                    bw.WriteInt16((short)UnknownValueA);
                    bw.WriteInt16((short)UnknownValueB);
                }
            }
        }

        /// <summary>
        /// Includes parameters for an animation bone entry in the HKX.
        /// </summary>
        public class AnimBoneEntry
        {
            /// <summary>
            /// ID of the SkeletonParam entry referenced by this animation bone.
            /// </summary>
            public short SkeletonParamID { get; set; }

            /// <summary>
            /// Unknown. 
            /// Possible values on PC: { 0, 1, 2, 3 }.
            /// Possible values on PS3: { 0, 0x10, 0x20, 0x30 }
            /// </summary>
            public byte Unk2 { get; set; }

            /// <summary>
            /// Creates a default AnimBoneEntry with the most commonly found
            /// field values: SkeletonParamRowID = -1, Unk2 = 0.
            /// </summary>
            public AnimBoneEntry()
            {
                SkeletonParamID = -1;
                Unk2 = 0;
            }

            internal AnimBoneEntry(BinaryReaderEx br)
            {
                SkeletonParamID = br.ReadInt16();
                Unk2 = br.ReadByte();
                br.AssertByte(0);
                br.AssertByte(0);
                br.AssertByte(0);
                br.AssertByte(0);
                br.AssertByte(0);
            }

            internal void Write(BinaryWriterEx bw)
            {
                bw.WriteInt16(SkeletonParamID);
                bw.WriteByte(Unk2);
                bw.WriteByte(0);
                bw.WriteByte(0);
                bw.WriteByte(0);
                bw.WriteByte(0);
                bw.WriteByte(0);
            }
        }

        /// <summary>
        /// Includes parameters for a ragdoll bone entry in the HKX.
        /// </summary>
        public class RagdollBoneEntry
        {
            /// <summary>
            /// ID of the RagdollParam entry referenced by this ragdoll bone.
            /// </summary>
            public int RagdollParamID { get; set; } = -1;

            /// <summary>
            /// Some type of unknown ID. Either -1, [2000, 2008], or 2080
            /// </summary>
            public short UnknownID { get; set; } = -1;

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte UnknownValueA { get; set; } = 0;

            /// <summary>
            /// Unknown.
            /// </summary>
            public byte UnknownValueB { get; set; } = 0;

            /// <summary>
            /// Unknown value unique to Bloodborne HKXPWV files.
            /// </summary>
            public sbyte UnknownBB { get; set; } = -1;

            /// <summary>
            /// Creates a default RagdollBoneEntry
            /// </summary>
            public RagdollBoneEntry()
            {

            }

            internal RagdollBoneEntry(BinaryReaderEx br, GameType game)
            {
                if (game == GameType.BB)
                {
                    RagdollParamID = br.ReadInt32();
                    UnknownID = br.ReadInt16();
                    UnknownValueA = br.ReadByte();
                    UnknownValueB = br.ReadByte();
                    UnknownBB = br.ReadSByte();
                    br.AssertByte(0);
                    br.AssertByte(0);
                    br.AssertByte(0);
                    br.AssertInt32(0);
                }
                else
                {
                    RagdollParamID = br.ReadInt16();
                    UnknownID = br.ReadInt16();
                    UnknownValueA = br.ReadByte();
                    UnknownValueB = br.ReadByte();
                    br.AssertByte(0);
                    br.AssertByte(0);

                    if (game == GameType.DS3)
                    {
                        br.AssertInt64(0);
                    }
                }
                
            }

            internal void Write(BinaryWriterEx bw, GameType game)
            {
                if (game == GameType.BB)
                {
                    bw.WriteInt32(RagdollParamID);
                    bw.WriteInt16(UnknownID);
                    bw.WriteByte(UnknownValueA);
                    bw.WriteByte(UnknownValueB);
                    bw.WriteSByte(UnknownBB);
                    bw.WriteByte(0);
                    bw.WriteByte(0);
                    bw.WriteByte(0);
                    bw.WriteInt32(0);
                }
                else
                {
                    bw.WriteInt16((short)RagdollParamID);
                    bw.WriteInt16(UnknownID);
                    bw.WriteByte(UnknownValueA);
                    bw.WriteByte(UnknownValueB);
                    bw.WriteByte(0);
                    bw.WriteByte(0);

                    if (game == GameType.DS3)
                        bw.WriteInt64(0);
                }
            }
        }

    }
}
