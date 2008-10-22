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
    private static readonly pbd::FileDescriptor descriptor = pbd::FileDescriptor.InternalBuildGeneratedFileFrom(
        global::System.Convert.FromBase64String(
        "ChV1bml0dGVzdF9pbXBvcnQucHJvdG8SGHByb3RvYnVmX3VuaXR0ZXN0X2lt" + 
        "cG9ydBokZ29vZ2xlL3Byb3RvYnVmL2NzaGFycF9vcHRpb25zLnByb3RvGiBn" + 
        "b29nbGUvcHJvdG9idWYvZGVzY3JpcHRvci5wcm90byIaCg1JbXBvcnRNZXNz" + 
        "YWdlEgkKAWQYASABKAUqPAoKSW1wb3J0RW51bRIOCgpJTVBPUlRfRk9PEAcS" + 
        "DgoKSU1QT1JUX0JBUhAIEg4KCklNUE9SVF9CQVoQCUJcChhjb20uZ29vZ2xl" + 
        "LnByb3RvYnVmLnRlc3RIAYLiCSFHb29nbGUuUHJvdG9jb2xCdWZmZXJzLlRl" + 
        "c3RQcm90b3OK4gkXVW5pdFRlc3RJbXBvcnRQcm90b0ZpbGU="),
        new pbd::FileDescriptor[] {
          global::Google.ProtocolBuffers.DescriptorProtos.CSharpOptions.Descriptor, 
          global::Google.ProtocolBuffers.DescriptorProtos.DescriptorProtoFile.Descriptor, 
        });
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
    private static readonly ImportMessage defaultInstance = new Builder().BuildPartial();
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
    
    public static ImportMessage ParseFrom(pb::ByteString data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static ImportMessage ParseFrom(pb::ByteString data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static ImportMessage ParseFrom(byte[] data) {
      return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();
    }
    public static ImportMessage ParseFrom(byte[] data, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();
    }
    public static ImportMessage ParseFrom(global::System.IO.Stream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static ImportMessage ParseFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    public static ImportMessage ParseFrom(pb::CodedInputStream input) {
      return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();
    }
    public static ImportMessage ParseFrom(pb::CodedInputStream input, pb::ExtensionRegistry extensionRegistry) {
      return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();
    }
    public static Builder CreateBuilder() { return new Builder(); }
    public override Builder CreateBuilderForType() { return new Builder(); }
    public static Builder CreateBuilder(ImportMessage prototype) {
      return (Builder) new Builder().MergeFrom(prototype);
    }
    
    public sealed partial class Builder : pb::GeneratedBuilder<ImportMessage, Builder> {
      protected override Builder ThisBuilder {
        get { return this; }
      }
      public Builder() {}
      
      ImportMessage result = new ImportMessage();
      
      protected override ImportMessage MessageBeingBuilt {
        get { return result; }
      }
      
      public override Builder Clear() {
        result = new ImportMessage();
        return this;
      }
      
      public override Builder Clone() {
        return new Builder().MergeFrom(result);
      }
      
      public override pbd::MessageDescriptor DescriptorForType {
        get { return ImportMessage.Descriptor; }
      }
      
      public override ImportMessage DefaultInstanceForType {
        get { return ImportMessage.DefaultInstance; }
      }
      
      public override ImportMessage BuildPartial() {
        ImportMessage returnMe = result;
        result = null;
        return returnMe;
      }
      
      public override Builder MergeFrom(pb::IMessage other) {
        if (other is ImportMessage) {
          return MergeFrom((ImportMessage) other);
        } else {
          base.MergeFrom(other);
          return this;
        }
      }
      
      public override Builder MergeFrom(ImportMessage other) {
        if (other == ImportMessage.DefaultInstance) return this;
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
        pb::UnknownFieldSet.Builder unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);
        while (true) {
          uint tag = input.ReadTag();
          switch (tag) {
            case 0: {
              this.UnknownFields = unknownFields.Build();
              return this;
            }
            default: {
              if (!ParseUnknownField(input, unknownFields, extensionRegistry, tag)) {
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
  
}
