// Generated by the protocol buffer compiler.  DO NOT EDIT!

using pb = global::Google.ProtocolBuffers;
using pbc = global::Google.ProtocolBuffers.Collections;
using pbd = global::Google.ProtocolBuffers.Descriptors;
using scg = global::System.Collections.Generic;
namespace Google.ProtocolBuffers.TestProtos {
  
  public static partial class UnitTestImportProtoFile {
  
    #region Descriptor
    public static pbd::FileDescriptor Descriptor {
        get { return descriptor; }
    }
    private static readonly pbd::FileDescriptor descriptor = pbd::FileDescriptor.InternalBuildGeneratedFileFrom (
        new byte[] {
            0x0a, 0x25, 0x67, 0x6f, 0x6f, 0x67, 0x6c, 0x65, 0x2f, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x62, 0x75, 0x66, 0x2f, 0x75, 0x6e, 
            0x69, 0x74, 0x74, 0x65, 0x73, 0x74, 0x5f, 0x69, 0x6d, 0x70, 0x6f, 0x72, 0x74, 0x2e, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x12, 
            0x18, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x62, 0x75, 0x66, 0x5f, 0x75, 0x6e, 0x69, 0x74, 0x74, 0x65, 0x73, 0x74, 0x5f, 0x69, 
            0x6d, 0x70, 0x6f, 0x72, 0x74, 0x22, 0x1a, 0x0a, 0x0d, 0x49, 0x6d, 0x70, 0x6f, 0x72, 0x74, 0x4d, 0x65, 0x73, 0x73, 0x61, 
            0x67, 0x65, 0x12, 0x09, 0x0a, 0x01, 0x64, 0x18, 0x01, 0x20, 0x01, 0x28, 0x05, 0x2a, 0x3c, 0x0a, 0x0a, 0x49, 0x6d, 0x70, 
            0x6f, 0x72, 0x74, 0x45, 0x6e, 0x75, 0x6d, 0x12, 0x0e, 0x0a, 0x0a, 0x49, 0x4d, 0x50, 0x4f, 0x52, 0x54, 0x5f, 0x46, 0x4f, 
            0x4f, 0x10, 0x07, 0x12, 0x0e, 0x0a, 0x0a, 0x49, 0x4d, 0x50, 0x4f, 0x52, 0x54, 0x5f, 0x42, 0x41, 0x52, 0x10, 0x08, 0x12, 
            0x0e, 0x0a, 0x0a, 0x49, 0x4d, 0x50, 0x4f, 0x52, 0x54, 0x5f, 0x42, 0x41, 0x5a, 0x10, 0x09, 0x42, 0x5a, 0x0a, 0x18, 0x63, 
            0x6f, 0x6d, 0x2e, 0x67, 0x6f, 0x6f, 0x67, 0x6c, 0x65, 0x2e, 0x70, 0x72, 0x6f, 0x74, 0x6f, 0x62, 0x75, 0x66, 0x2e, 0x74, 
            0x65, 0x73, 0x74, 0x48, 0x01, 0xc2, 0x3e, 0x21, 0x47, 0x6f, 0x6f, 0x67, 0x6c, 0x65, 0x2e, 0x50, 0x72, 0x6f, 0x74, 0x6f, 
            0x63, 0x6f, 0x6c, 0x42, 0x75, 0x66, 0x66, 0x65, 0x72, 0x73, 0x2e, 0x54, 0x65, 0x73, 0x74, 0x50, 0x72, 0x6f, 0x74, 0x6f, 
            0x73, 0xca, 0x3e, 0x17, 0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74, 0x49, 0x6d, 0x70, 0x6f, 0x72, 0x74, 0x50, 0x72, 
            0x6f, 0x74, 0x6f, 0x46, 0x69, 0x6c, 0x65, 
        }, new pbd::FileDescriptor[] {
        });
    #endregion
    
    #region Extensions
    #endregion
    
    #region Static variables
    internal static readonly pbd::MessageDescriptor internal__static_protobuf_unittest_import_ImportMessage__Descriptor 
        = Descriptor.MessageTypes[0];
    internal static pb::FieldAccess.FieldAccessorTable<global::Google.ProtocolBuffers.TestProtos.ImportMessage, global::Google.ProtocolBuffers.TestProtos.ImportMessage.Builder> internal__static_protobuf_unittest_import_ImportMessage__FieldAccessorTable
        = new pb::FieldAccess.FieldAccessorTable<global::Google.ProtocolBuffers.TestProtos.ImportMessage, global::Google.ProtocolBuffers.TestProtos.ImportMessage.Builder>(internal__static_protobuf_unittest_import_ImportMessage__Descriptor,
            new string[] { "D", });
    #endregion
    
  }
  
  #region Enums
  public enum ImportEnum {
    IMPORT_FOO = 7,
    IMPORT_BAR = 8,
    IMPORT_BAZ = 9,
  }
  
  #endregion
  
  #region Messages
  public sealed partial class ImportMessage : pb::GeneratedMessage<ImportMessage, ImportMessage.Builder> {
    private static readonly ImportMessage defaultInstance = new ImportMessage();
    public static ImportMessage DefaultInstance {
      get { return defaultInstance; }
    }
    
    public override ImportMessage DefaultInstanceForType {
      get { return defaultInstance; }
    }
    
    protected override ImportMessage ThisMessage {
      get { return this; }
    }
    
    public static pbd::MessageDescriptor Descriptor {
      get { return global::Google.ProtocolBuffers.TestProtos.UnitTestImportProtoFile.internal__static_protobuf_unittest_import_ImportMessage__Descriptor; }
    }
    
    protected override pb::FieldAccess.FieldAccessorTable<ImportMessage, ImportMessage.Builder> InternalFieldAccessors {
      get { return global::Google.ProtocolBuffers.TestProtos.UnitTestImportProtoFile.internal__static_protobuf_unittest_import_ImportMessage__FieldAccessorTable; }
    }
    
    // optional int32 d = 1;
    private bool hasD;
    private int d_ = 0;
    public bool HasD {
      get { return hasD; }
    }
    public int D {
      get { return d_; }
    }
    
    public override bool IsInitialized {
      get {
        return true;
      }
    }
    
    public override void WriteTo(pb::CodedOutputStream output) {
      if (HasD) {
        output.WriteInt32(1, D);
      }
      UnknownFields.WriteTo(output);
    }
    
    private int memoizedSerializedSize = -1;
    public override int SerializedSize {
      get {
        int size = memoizedSerializedSize;
        if (size != -1) return size;
        
        size = 0;
        if (HasD) {
          size += pb::CodedOutputStream.ComputeInt32Size(1, D);
        }
        size += UnknownFields.SerializedSize;
        memoizedSerializedSize = size;
        return size;
      }
    }
    
    public static global::Google.ProtocolBuffers.TestProtos.ImportMessage ParseFrom(pb::ByteString data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static global::Google.ProtocolBuffers.TestProtos.ImportMessage ParseFrom(pb::ByteString data,
        pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry))
               .BuildParsed();
    }
    public static global::Google.ProtocolBuffers.TestProtos.ImportMessage ParseFrom(byte[] data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static global::Google.ProtocolBuffers.TestProtos.ImportMessage ParseFrom(byte[] data,
        pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry))
               .BuildParsed();
    }
    public static global::Google.ProtocolBuffers.TestProtos.ImportMessage ParseFrom(global::System.IO.Stream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static global::Google.ProtocolBuffers.TestProtos.ImportMessage ParseFrom(
        global::System.IO.Stream input,
        pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry))
               .BuildParsed();
    }
    public static global::Google.ProtocolBuffers.TestProtos.ImportMessage ParseFrom(pb::CodedInputStream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static global::Google.ProtocolBuffers.TestProtos.ImportMessage ParseFrom(pb::CodedInputStream input,
        pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry))
               .BuildParsed();
    }
    
    public static Builder CreateBuilder() { return new Builder(); }
    public override Builder CreateBuilderForType() { return new Builder(); }
    public static Builder CreateBuilder(global::Google.ProtocolBuffers.TestProtos.ImportMessage prototype) {
      return (Builder) new Builder().MergeFrom(prototype);
    }
    
    public sealed partial class Builder : pb::GeneratedBuilder<global::Google.ProtocolBuffers.TestProtos.ImportMessage, Builder> {
      protected override Builder ThisBuilder {
        get { return this; }
      }
      
      // Construct using global::Google.ProtocolBuffers.TestProtos.ImportMessage.CreateBuilder()
      public Builder() {}
      
      global::Google.ProtocolBuffers.TestProtos.ImportMessage result = new global::Google.ProtocolBuffers.TestProtos.ImportMessage();
      
      protected override global::Google.ProtocolBuffers.TestProtos.ImportMessage MessageBeingBuilt {
        get { return result; }
      }
      
      public override Builder Clear() {
        result = new global::Google.ProtocolBuffers.TestProtos.ImportMessage();
        return this;
      }
      
      public override Builder Clone() {
        return new Builder().MergeFrom(result);
      }
      
      public override pbd::MessageDescriptor DescriptorForType {
        get { return global::Google.ProtocolBuffers.TestProtos.ImportMessage.Descriptor; }
      }
      
      public override global::Google.ProtocolBuffers.TestProtos.ImportMessage DefaultInstanceForType {
        get { return global::Google.ProtocolBuffers.TestProtos.ImportMessage.DefaultInstance; }
      }
      
      public override global::Google.ProtocolBuffers.TestProtos.ImportMessage BuildPartial() {
        global::Google.ProtocolBuffers.TestProtos.ImportMessage returnMe = result;
        result = null;
        return returnMe;
      }
      
      public override Builder MergeFrom(pb::IMessage other) {
        if (other is global::Google.ProtocolBuffers.TestProtos.ImportMessage) {
          return MergeFrom((global::Google.ProtocolBuffers.TestProtos.ImportMessage) other);
        } else {
          base.MergeFrom(other);
          return this;
        }
      }
      
      public override Builder MergeFrom(global::Google.ProtocolBuffers.TestProtos.ImportMessage other) {
        if (other == global::Google.ProtocolBuffers.TestProtos.ImportMessage.DefaultInstance) return this;
        if (other.HasD) {
          D = other.D;
        }
        this.MergeUnknownFields(other.UnknownFields);
        return this;
      }
      
      public override Builder MergeFrom(pb::CodedInputStream input) {
        return MergeFrom(input, pb::ExtensionRegistry.Empty);
      }
      
      public override Builder MergeFrom(pb::CodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
        pb::UnknownFieldSet.Builder unknownFields =
          pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
        while (true) {
          uint tag = input.ReadTag();
          switch (tag) {
            case 0:
              this.UnknownFields = unknownFields.Build();
              return this;
            default: {
              if (!ParseUnknownField(input, unknownFields,
                                     extensionRegistry, tag)) {
                this.UnknownFields = unknownFields.Build();
                return this;
              }
              break;
            }
            case 8: {
              D = input.ReadInt32();
              break;
            }
          }
        }
      }
      
      
      // optional int32 d = 1;
      public bool HasD {
        get { return result.HasD; }
      }
      public int D {
        get { return result.D; }
        set { SetD(value); }
      }
      public Builder SetD(int value) {
        result.hasD = true;
        result.d_ = value;
        return this;
      }
      public Builder ClearD() {
        result.hasD = false;
        result.d_ = 0;
        return this;
      }
    }
  }
  
  #endregion
  
  #region Services
  #endregion
}
