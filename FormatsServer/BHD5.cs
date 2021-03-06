// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: BHD5.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace SOULS.BHD5 {

  /// <summary>Holder for reflection information generated from BHD5.proto</summary>
  public static partial class BHD5Reflection {

    #region Descriptor
    /// <summary>File descriptor for BHD5.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static BHD5Reflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgpCSEQ1LnByb3RvEgpTT1VMUy5CSEQ1IhcKBEdhbWUSDwoHdmFsdWVfXxgB",
            "IAEoBSJPCgZCdWNrZXQSEAoIQ2FwYWNpdHkYASABKAUSDQoFQ291bnQYAiAB",
            "KAUSJAoESXRlbRgDIAEoCzIWLlNPVUxTLkJIRDUuRmlsZUhlYWRlciKyAQoK",
            "RmlsZUhlYWRlchIUCgxGaWxlTmFtZUhhc2gYASABKA0SFgoOUGFkZGVkRmls",
            "ZVNpemUYAiABKAUSGAoQVW5wYWRkZWRGaWxlU2l6ZRgDIAEoAxISCgpGaWxl",
            "T2Zmc2V0GAQgASgDEiQKB1NIQUhhc2gYBSABKAsyEy5TT1VMUy5CSEQ1LlNI",
            "QUhhc2gSIgoGQUVTS2V5GAYgASgLMhIuU09VTFMuQkhENS5BRVNLZXkiOgoH",
            "U0hBSGFzaBIMCgRIYXNoGAEgAygFEiEKBlJhbmdlcxgCIAMoCzIRLlNPVUxT",
            "LkJIRDUuUmFuZ2UiOAoGQUVTS2V5EgsKA0tleRgBIAMoBRIhCgZSYW5nZXMY",
            "AiADKAsyES5TT1VMUy5CSEQ1LlJhbmdlIi8KBVJhbmdlEhMKC1N0YXJ0T2Zm",
            "c2V0GAEgASgDEhEKCUVuZE9mZnNldBgCIAEoA2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::SOULS.BHD5.Game), global::SOULS.BHD5.Game.Parser, new[]{ "Value" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SOULS.BHD5.Bucket), global::SOULS.BHD5.Bucket.Parser, new[]{ "Capacity", "Count", "Item" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SOULS.BHD5.FileHeader), global::SOULS.BHD5.FileHeader.Parser, new[]{ "FileNameHash", "PaddedFileSize", "UnpaddedFileSize", "FileOffset", "SHAHash", "AESKey" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SOULS.BHD5.SHAHash), global::SOULS.BHD5.SHAHash.Parser, new[]{ "Hash", "Ranges" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SOULS.BHD5.AESKey), global::SOULS.BHD5.AESKey.Parser, new[]{ "Key", "Ranges" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SOULS.BHD5.Range), global::SOULS.BHD5.Range.Parser, new[]{ "StartOffset", "EndOffset" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Game : pb::IMessage<Game> {
    private static readonly pb::MessageParser<Game> _parser = new pb::MessageParser<Game>(() => new Game());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Game> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SOULS.BHD5.BHD5Reflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Game() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Game(Game other) : this() {
      value_ = other.value_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Game Clone() {
      return new Game(this);
    }

    /// <summary>Field number for the "value__" field.</summary>
    public const int ValueFieldNumber = 1;
    private int value_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Value {
      get { return value_; }
      set {
        value_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Game);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Game other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Value != other.Value) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Value != 0) hash ^= Value.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Value != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Value);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Value != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Value);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Game other) {
      if (other == null) {
        return;
      }
      if (other.Value != 0) {
        Value = other.Value;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Value = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Bucket : pb::IMessage<Bucket> {
    private static readonly pb::MessageParser<Bucket> _parser = new pb::MessageParser<Bucket>(() => new Bucket());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Bucket> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SOULS.BHD5.BHD5Reflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Bucket() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Bucket(Bucket other) : this() {
      capacity_ = other.capacity_;
      count_ = other.count_;
      item_ = other.item_ != null ? other.item_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Bucket Clone() {
      return new Bucket(this);
    }

    /// <summary>Field number for the "Capacity" field.</summary>
    public const int CapacityFieldNumber = 1;
    private int capacity_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Capacity {
      get { return capacity_; }
      set {
        capacity_ = value;
      }
    }

    /// <summary>Field number for the "Count" field.</summary>
    public const int CountFieldNumber = 2;
    private int count_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Count {
      get { return count_; }
      set {
        count_ = value;
      }
    }

    /// <summary>Field number for the "Item" field.</summary>
    public const int ItemFieldNumber = 3;
    private global::SOULS.BHD5.FileHeader item_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SOULS.BHD5.FileHeader Item {
      get { return item_; }
      set {
        item_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Bucket);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Bucket other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Capacity != other.Capacity) return false;
      if (Count != other.Count) return false;
      if (!object.Equals(Item, other.Item)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Capacity != 0) hash ^= Capacity.GetHashCode();
      if (Count != 0) hash ^= Count.GetHashCode();
      if (item_ != null) hash ^= Item.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Capacity != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Capacity);
      }
      if (Count != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Count);
      }
      if (item_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(Item);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Capacity != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Capacity);
      }
      if (Count != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Count);
      }
      if (item_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Item);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Bucket other) {
      if (other == null) {
        return;
      }
      if (other.Capacity != 0) {
        Capacity = other.Capacity;
      }
      if (other.Count != 0) {
        Count = other.Count;
      }
      if (other.item_ != null) {
        if (item_ == null) {
          Item = new global::SOULS.BHD5.FileHeader();
        }
        Item.MergeFrom(other.Item);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Capacity = input.ReadInt32();
            break;
          }
          case 16: {
            Count = input.ReadInt32();
            break;
          }
          case 26: {
            if (item_ == null) {
              Item = new global::SOULS.BHD5.FileHeader();
            }
            input.ReadMessage(Item);
            break;
          }
        }
      }
    }

  }

  public sealed partial class FileHeader : pb::IMessage<FileHeader> {
    private static readonly pb::MessageParser<FileHeader> _parser = new pb::MessageParser<FileHeader>(() => new FileHeader());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<FileHeader> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SOULS.BHD5.BHD5Reflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FileHeader() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FileHeader(FileHeader other) : this() {
      fileNameHash_ = other.fileNameHash_;
      paddedFileSize_ = other.paddedFileSize_;
      unpaddedFileSize_ = other.unpaddedFileSize_;
      fileOffset_ = other.fileOffset_;
      sHAHash_ = other.sHAHash_ != null ? other.sHAHash_.Clone() : null;
      aESKey_ = other.aESKey_ != null ? other.aESKey_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public FileHeader Clone() {
      return new FileHeader(this);
    }

    /// <summary>Field number for the "FileNameHash" field.</summary>
    public const int FileNameHashFieldNumber = 1;
    private uint fileNameHash_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public uint FileNameHash {
      get { return fileNameHash_; }
      set {
        fileNameHash_ = value;
      }
    }

    /// <summary>Field number for the "PaddedFileSize" field.</summary>
    public const int PaddedFileSizeFieldNumber = 2;
    private int paddedFileSize_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int PaddedFileSize {
      get { return paddedFileSize_; }
      set {
        paddedFileSize_ = value;
      }
    }

    /// <summary>Field number for the "UnpaddedFileSize" field.</summary>
    public const int UnpaddedFileSizeFieldNumber = 3;
    private long unpaddedFileSize_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long UnpaddedFileSize {
      get { return unpaddedFileSize_; }
      set {
        unpaddedFileSize_ = value;
      }
    }

    /// <summary>Field number for the "FileOffset" field.</summary>
    public const int FileOffsetFieldNumber = 4;
    private long fileOffset_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long FileOffset {
      get { return fileOffset_; }
      set {
        fileOffset_ = value;
      }
    }

    /// <summary>Field number for the "SHAHash" field.</summary>
    public const int SHAHashFieldNumber = 5;
    private global::SOULS.BHD5.SHAHash sHAHash_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SOULS.BHD5.SHAHash SHAHash {
      get { return sHAHash_; }
      set {
        sHAHash_ = value;
      }
    }

    /// <summary>Field number for the "AESKey" field.</summary>
    public const int AESKeyFieldNumber = 6;
    private global::SOULS.BHD5.AESKey aESKey_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::SOULS.BHD5.AESKey AESKey {
      get { return aESKey_; }
      set {
        aESKey_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as FileHeader);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(FileHeader other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (FileNameHash != other.FileNameHash) return false;
      if (PaddedFileSize != other.PaddedFileSize) return false;
      if (UnpaddedFileSize != other.UnpaddedFileSize) return false;
      if (FileOffset != other.FileOffset) return false;
      if (!object.Equals(SHAHash, other.SHAHash)) return false;
      if (!object.Equals(AESKey, other.AESKey)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (FileNameHash != 0) hash ^= FileNameHash.GetHashCode();
      if (PaddedFileSize != 0) hash ^= PaddedFileSize.GetHashCode();
      if (UnpaddedFileSize != 0L) hash ^= UnpaddedFileSize.GetHashCode();
      if (FileOffset != 0L) hash ^= FileOffset.GetHashCode();
      if (sHAHash_ != null) hash ^= SHAHash.GetHashCode();
      if (aESKey_ != null) hash ^= AESKey.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (FileNameHash != 0) {
        output.WriteRawTag(8);
        output.WriteUInt32(FileNameHash);
      }
      if (PaddedFileSize != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(PaddedFileSize);
      }
      if (UnpaddedFileSize != 0L) {
        output.WriteRawTag(24);
        output.WriteInt64(UnpaddedFileSize);
      }
      if (FileOffset != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(FileOffset);
      }
      if (sHAHash_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(SHAHash);
      }
      if (aESKey_ != null) {
        output.WriteRawTag(50);
        output.WriteMessage(AESKey);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (FileNameHash != 0) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(FileNameHash);
      }
      if (PaddedFileSize != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(PaddedFileSize);
      }
      if (UnpaddedFileSize != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(UnpaddedFileSize);
      }
      if (FileOffset != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(FileOffset);
      }
      if (sHAHash_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(SHAHash);
      }
      if (aESKey_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(AESKey);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(FileHeader other) {
      if (other == null) {
        return;
      }
      if (other.FileNameHash != 0) {
        FileNameHash = other.FileNameHash;
      }
      if (other.PaddedFileSize != 0) {
        PaddedFileSize = other.PaddedFileSize;
      }
      if (other.UnpaddedFileSize != 0L) {
        UnpaddedFileSize = other.UnpaddedFileSize;
      }
      if (other.FileOffset != 0L) {
        FileOffset = other.FileOffset;
      }
      if (other.sHAHash_ != null) {
        if (sHAHash_ == null) {
          SHAHash = new global::SOULS.BHD5.SHAHash();
        }
        SHAHash.MergeFrom(other.SHAHash);
      }
      if (other.aESKey_ != null) {
        if (aESKey_ == null) {
          AESKey = new global::SOULS.BHD5.AESKey();
        }
        AESKey.MergeFrom(other.AESKey);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            FileNameHash = input.ReadUInt32();
            break;
          }
          case 16: {
            PaddedFileSize = input.ReadInt32();
            break;
          }
          case 24: {
            UnpaddedFileSize = input.ReadInt64();
            break;
          }
          case 32: {
            FileOffset = input.ReadInt64();
            break;
          }
          case 42: {
            if (sHAHash_ == null) {
              SHAHash = new global::SOULS.BHD5.SHAHash();
            }
            input.ReadMessage(SHAHash);
            break;
          }
          case 50: {
            if (aESKey_ == null) {
              AESKey = new global::SOULS.BHD5.AESKey();
            }
            input.ReadMessage(AESKey);
            break;
          }
        }
      }
    }

  }

  public sealed partial class SHAHash : pb::IMessage<SHAHash> {
    private static readonly pb::MessageParser<SHAHash> _parser = new pb::MessageParser<SHAHash>(() => new SHAHash());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<SHAHash> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SOULS.BHD5.BHD5Reflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SHAHash() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SHAHash(SHAHash other) : this() {
      hash_ = other.hash_.Clone();
      ranges_ = other.ranges_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public SHAHash Clone() {
      return new SHAHash(this);
    }

    /// <summary>Field number for the "Hash" field.</summary>
    public const int HashFieldNumber = 1;
    private static readonly pb::FieldCodec<int> _repeated_hash_codec
        = pb::FieldCodec.ForInt32(10);
    private readonly pbc::RepeatedField<int> hash_ = new pbc::RepeatedField<int>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> Hash {
      get { return hash_; }
    }

    /// <summary>Field number for the "Ranges" field.</summary>
    public const int RangesFieldNumber = 2;
    private static readonly pb::FieldCodec<global::SOULS.BHD5.Range> _repeated_ranges_codec
        = pb::FieldCodec.ForMessage(18, global::SOULS.BHD5.Range.Parser);
    private readonly pbc::RepeatedField<global::SOULS.BHD5.Range> ranges_ = new pbc::RepeatedField<global::SOULS.BHD5.Range>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::SOULS.BHD5.Range> Ranges {
      get { return ranges_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as SHAHash);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(SHAHash other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!hash_.Equals(other.hash_)) return false;
      if(!ranges_.Equals(other.ranges_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= hash_.GetHashCode();
      hash ^= ranges_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      hash_.WriteTo(output, _repeated_hash_codec);
      ranges_.WriteTo(output, _repeated_ranges_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += hash_.CalculateSize(_repeated_hash_codec);
      size += ranges_.CalculateSize(_repeated_ranges_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(SHAHash other) {
      if (other == null) {
        return;
      }
      hash_.Add(other.hash_);
      ranges_.Add(other.ranges_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10:
          case 8: {
            hash_.AddEntriesFrom(input, _repeated_hash_codec);
            break;
          }
          case 18: {
            ranges_.AddEntriesFrom(input, _repeated_ranges_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class AESKey : pb::IMessage<AESKey> {
    private static readonly pb::MessageParser<AESKey> _parser = new pb::MessageParser<AESKey>(() => new AESKey());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AESKey> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SOULS.BHD5.BHD5Reflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AESKey() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AESKey(AESKey other) : this() {
      key_ = other.key_.Clone();
      ranges_ = other.ranges_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AESKey Clone() {
      return new AESKey(this);
    }

    /// <summary>Field number for the "Key" field.</summary>
    public const int KeyFieldNumber = 1;
    private static readonly pb::FieldCodec<int> _repeated_key_codec
        = pb::FieldCodec.ForInt32(10);
    private readonly pbc::RepeatedField<int> key_ = new pbc::RepeatedField<int>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> Key {
      get { return key_; }
    }

    /// <summary>Field number for the "Ranges" field.</summary>
    public const int RangesFieldNumber = 2;
    private static readonly pb::FieldCodec<global::SOULS.BHD5.Range> _repeated_ranges_codec
        = pb::FieldCodec.ForMessage(18, global::SOULS.BHD5.Range.Parser);
    private readonly pbc::RepeatedField<global::SOULS.BHD5.Range> ranges_ = new pbc::RepeatedField<global::SOULS.BHD5.Range>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::SOULS.BHD5.Range> Ranges {
      get { return ranges_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AESKey);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AESKey other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!key_.Equals(other.key_)) return false;
      if(!ranges_.Equals(other.ranges_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= key_.GetHashCode();
      hash ^= ranges_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      key_.WriteTo(output, _repeated_key_codec);
      ranges_.WriteTo(output, _repeated_ranges_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += key_.CalculateSize(_repeated_key_codec);
      size += ranges_.CalculateSize(_repeated_ranges_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AESKey other) {
      if (other == null) {
        return;
      }
      key_.Add(other.key_);
      ranges_.Add(other.ranges_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10:
          case 8: {
            key_.AddEntriesFrom(input, _repeated_key_codec);
            break;
          }
          case 18: {
            ranges_.AddEntriesFrom(input, _repeated_ranges_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class Range : pb::IMessage<Range> {
    private static readonly pb::MessageParser<Range> _parser = new pb::MessageParser<Range>(() => new Range());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Range> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SOULS.BHD5.BHD5Reflection.Descriptor.MessageTypes[5]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Range() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Range(Range other) : this() {
      startOffset_ = other.startOffset_;
      endOffset_ = other.endOffset_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Range Clone() {
      return new Range(this);
    }

    /// <summary>Field number for the "StartOffset" field.</summary>
    public const int StartOffsetFieldNumber = 1;
    private long startOffset_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long StartOffset {
      get { return startOffset_; }
      set {
        startOffset_ = value;
      }
    }

    /// <summary>Field number for the "EndOffset" field.</summary>
    public const int EndOffsetFieldNumber = 2;
    private long endOffset_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long EndOffset {
      get { return endOffset_; }
      set {
        endOffset_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Range);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Range other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (StartOffset != other.StartOffset) return false;
      if (EndOffset != other.EndOffset) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (StartOffset != 0L) hash ^= StartOffset.GetHashCode();
      if (EndOffset != 0L) hash ^= EndOffset.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (StartOffset != 0L) {
        output.WriteRawTag(8);
        output.WriteInt64(StartOffset);
      }
      if (EndOffset != 0L) {
        output.WriteRawTag(16);
        output.WriteInt64(EndOffset);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (StartOffset != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(StartOffset);
      }
      if (EndOffset != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(EndOffset);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Range other) {
      if (other == null) {
        return;
      }
      if (other.StartOffset != 0L) {
        StartOffset = other.StartOffset;
      }
      if (other.EndOffset != 0L) {
        EndOffset = other.EndOffset;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            StartOffset = input.ReadInt64();
            break;
          }
          case 16: {
            EndOffset = input.ReadInt64();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
