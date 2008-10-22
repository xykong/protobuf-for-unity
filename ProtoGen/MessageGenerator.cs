﻿using System.Collections;
using Google.ProtocolBuffers.Descriptors;
using Google.ProtocolBuffers.DescriptorProtos;
using System.Collections.Generic;

using ExtensionRange = Google.ProtocolBuffers.DescriptorProtos.DescriptorProto.Types.ExtensionRange;

namespace Google.ProtocolBuffers.ProtoGen {
  internal class MessageGenerator : SourceGeneratorBase<MessageDescriptor>, ISourceGenerator {
    internal MessageGenerator(MessageDescriptor descriptor) : base(descriptor) {
    }

    private string ClassName {
      get { return Descriptor.Name; }
    }

    private string FullClassName {
      get { return DescriptorUtil.GetClassName(Descriptor); }
    }

    /// <summary>
    /// Get an identifier that uniquely identifies this type within the file.
    /// This is used to declare static variables related to this type at the
    /// outermost file scope.
    /// </summary>
    static string GetUniqueFileScopeIdentifier(IDescriptor descriptor) {
      return "static_" + descriptor.FullName.Replace(".", "_");
    }

    internal void GenerateStaticVariables(TextGenerator writer) {
      // Because descriptor.proto (Google.ProtocolBuffers.DescriptorProtos) is
      // used in the construction of descriptors, we have a tricky bootstrapping
      // problem.  To help control static initialization order, we make sure all
      // descriptors and other static data that depends on them are members of
      // the proto-descriptor class.  This way, they will be initialized in
      // a deterministic order.

      string identifier = GetUniqueFileScopeIdentifier(Descriptor);

      // The descriptor for this type.
      string access = Descriptor.File.Options.GetExtension(CSharpOptions.CSharpNestClasses) ? "private" : "internal";
      writer.WriteLine("{0} static readonly pbd::MessageDescriptor internal__{1}__Descriptor", access, identifier);
      if (Descriptor.ContainingType == null) {
        writer.WriteLine("    = Descriptor.MessageTypes[{0}];", Descriptor.Index);
      } else {
        writer.WriteLine("    = internal__{0}__Descriptor.NestedTypes[{1}];", GetUniqueFileScopeIdentifier(Descriptor.ContainingType), Descriptor.Index);
      }
      writer.WriteLine("{0} static pb::FieldAccess.FieldAccessorTable<{1}, {1}.Builder> internal__{2}__FieldAccessorTable",
          access, FullClassName, identifier);
      writer.WriteLine("    = new pb::FieldAccess.FieldAccessorTable<{0}, {0}.Builder>(internal__{1}__Descriptor,",
          FullClassName, identifier);
      writer.Print("        new string[] { ");
      foreach (FieldDescriptor field in Descriptor.Fields) {
        writer.Write("\"{0}\", ", Helpers.UnderscoresToPascalCase(DescriptorUtil.GetFieldName(field)));
      }
      writer.WriteLine("});");

      // Generate static members for all nested types.
      foreach (MessageDescriptor nestedMessage in Descriptor.NestedTypes) {
        new MessageGenerator(nestedMessage).GenerateStaticVariables(writer);
      }
    }

    public void Generate(TextGenerator writer) {
      writer.WriteLine("{0} sealed partial class {1} : pb::{2}Message<{1}, {1}.Builder> {{",
          ClassAccessLevel, ClassName, Descriptor.Proto.ExtensionRangeCount > 0 ? "Extendable" : "Generated");
      writer.Indent();
      // Must call BuildPartial() to make sure all lists are made read-only
      writer.WriteLine("private static readonly {0} defaultInstance = new Builder().BuildPartial();", ClassName);
      writer.WriteLine("public static {0} DefaultInstance {{", ClassName);
      writer.WriteLine("  get { return defaultInstance; }");
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("public override {0} DefaultInstanceForType {{", ClassName);
      writer.WriteLine("  get { return defaultInstance; }");
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("protected override {0} ThisMessage {{", ClassName);
      writer.WriteLine("  get { return this; }");
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("public static pbd::MessageDescriptor Descriptor {");
      writer.WriteLine("  get {{ return {0}.internal__{1}__Descriptor; }}", DescriptorUtil.GetFullUmbrellaClassName(Descriptor.File),
          GetUniqueFileScopeIdentifier(Descriptor));
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("protected override pb::FieldAccess.FieldAccessorTable<{0}, {0}.Builder> InternalFieldAccessors {{", ClassName);
      writer.WriteLine("  get {{ return {0}.internal__{1}__FieldAccessorTable; }}", DescriptorUtil.GetFullUmbrellaClassName(Descriptor.File),
          GetUniqueFileScopeIdentifier(Descriptor));
      writer.WriteLine("}");
      writer.WriteLine();

      // Extensions don't need to go in an extra nested type 
      WriteChildren(writer, null, Descriptor.Extensions);

      if (Descriptor.EnumTypes.Count + Descriptor.NestedTypes.Count > 0) {
        writer.WriteLine("#region Nested types");
        writer.WriteLine("public static class Types {");
        writer.Indent();
        WriteChildren(writer, null, Descriptor.EnumTypes);
        WriteChildren(writer, null, Descriptor.NestedTypes);
        writer.Outdent();
        writer.WriteLine("}");
        writer.WriteLine("#endregion");
        writer.WriteLine();
      }

      foreach(FieldDescriptor fieldDescriptor in Descriptor.Fields) {
        // Rats: we lose the debug comment here :(
        SourceGenerators.CreateFieldGenerator(fieldDescriptor).GenerateMembers(writer);
        writer.WriteLine();
      }

      if (Descriptor.File.Options.OptimizeFor == FileOptions.Types.OptimizeMode.SPEED) {
        GenerateIsInitialized(writer);
        GenerateMessageSerializationMethods(writer);
      }

      GenerateParseFromMethods(writer);
      GenerateBuilder(writer);
    }

    private void GenerateMessageSerializationMethods(TextGenerator writer) {
      List<FieldDescriptor> sortedFields = new List<FieldDescriptor>(Descriptor.Fields);
      sortedFields.Sort((f1, f2) => f1.FieldNumber.CompareTo(f2.FieldNumber));

      List<ExtensionRange> sortedExtensions = new List<ExtensionRange>(Descriptor.Proto.ExtensionRangeList);
      sortedExtensions.Sort((r1, r2) => (r1.Start.CompareTo(r2.Start)));

      writer.WriteLine("public override void WriteTo(pb::CodedOutputStream output) {");
      writer.Indent();
      if (Descriptor.Proto.ExtensionRangeList.Count > 0) {
        writer.WriteLine("pb::ExtendableMessage<{0}, {0}.Builder>.ExtensionWriter extensionWriter = CreateExtensionWriter(this);",
          ClassName);
      }

      // Merge the fields and the extension ranges, both sorted by field number.
      for (int i = 0, j = 0; i < Descriptor.Fields.Count || j < sortedExtensions.Count; ) {
        if (i == Descriptor.Fields.Count) {
          GenerateSerializeOneExtensionRange(writer, sortedExtensions[j++]);
        } else if (j == sortedExtensions.Count) {
          GenerateSerializeOneField(writer, sortedFields[i++]);
        } else if (sortedFields[i].FieldNumber < sortedExtensions[j].Start) {
          GenerateSerializeOneField(writer, sortedFields[i++]);
        } else {
          GenerateSerializeOneExtensionRange(writer, sortedExtensions[j++]);
        }
      }

      if (Descriptor.Proto.Options.MessageSetWireFormat) {
        writer.WriteLine("UnknownFields.WriteAsMessageSetTo(output);");
      } else {
        writer.WriteLine("UnknownFields.WriteTo(output);");
      }

      writer.Outdent();
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("private int memoizedSerializedSize = -1;");
      writer.WriteLine("public override int SerializedSize {");
      writer.Indent();
      writer.WriteLine("get {");
      writer.Indent();
      writer.WriteLine("int size = memoizedSerializedSize;");
      writer.WriteLine("if (size != -1) return size;");
      writer.WriteLine();
      writer.WriteLine("size = 0;");
      foreach (FieldDescriptor field in Descriptor.Fields) {
        SourceGenerators.CreateFieldGenerator(field).GenerateSerializedSizeCode(writer);
      }
      if (Descriptor.Proto.ExtensionRangeCount > 0) {
        writer.WriteLine("size += ExtensionsSerializedSize;");
      }

      if (Descriptor.Options.MessageSetWireFormat) {
        writer.WriteLine("size += UnknownFields.SerializedSizeAsMessageSet;");
      } else {
        writer.WriteLine("size += UnknownFields.SerializedSize;");
      }
      writer.WriteLine("memoizedSerializedSize = size;");
      writer.WriteLine("return size;");
      writer.Outdent();
      writer.WriteLine("}");
      writer.Outdent();
      writer.WriteLine("}");
      writer.WriteLine();
    }

    private static void GenerateSerializeOneField(TextGenerator writer, FieldDescriptor fieldDescriptor) {
      SourceGenerators.CreateFieldGenerator(fieldDescriptor).GenerateSerializationCode(writer);
    }

    private static void GenerateSerializeOneExtensionRange(TextGenerator writer, ExtensionRange extensionRange) {
      writer.WriteLine("extensionWriter.WriteUntil({0}, output);", extensionRange.End);
    }

    private void GenerateParseFromMethods(TextGenerator writer) {
      // Note:  These are separate from GenerateMessageSerializationMethods()
      //   because they need to be generated even for messages that are optimized
      //   for code size.

      writer.WriteLine("public static {0} ParseFrom(pb::ByteString data) {{", ClassName);
      writer.WriteLine("  return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();");
      writer.WriteLine("}");
      writer.WriteLine("public static {0} ParseFrom(pb::ByteString data, pb::ExtensionRegistry extensionRegistry) {{", ClassName);
      writer.WriteLine("  return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();");
      writer.WriteLine("}");
      writer.WriteLine("public static {0} ParseFrom(byte[] data) {{", ClassName);
      writer.WriteLine("  return ((Builder) CreateBuilder().MergeFrom(data)).BuildParsed();");
      writer.WriteLine("}");
      writer.WriteLine("public static {0} ParseFrom(byte[] data, pb::ExtensionRegistry extensionRegistry) {{", ClassName);
      writer.WriteLine("  return ((Builder) CreateBuilder().MergeFrom(data, extensionRegistry)).BuildParsed();");
      writer.WriteLine("}");
      writer.WriteLine("public static {0} ParseFrom(global::System.IO.Stream input) {{", ClassName);
      writer.WriteLine("  return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();");
      writer.WriteLine("}");
      writer.WriteLine("public static {0} ParseFrom(global::System.IO.Stream input, pb::ExtensionRegistry extensionRegistry) {{", ClassName);
      writer.WriteLine("  return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();");
      writer.WriteLine("}");
      writer.WriteLine("public static {0} ParseFrom(pb::CodedInputStream input) {{", ClassName);
      writer.WriteLine("  return ((Builder) CreateBuilder().MergeFrom(input)).BuildParsed();");
      writer.WriteLine("}");
      writer.WriteLine("public static {0} ParseFrom(pb::CodedInputStream input, pb::ExtensionRegistry extensionRegistry) {{", ClassName);
      writer.WriteLine("  return ((Builder) CreateBuilder().MergeFrom(input, extensionRegistry)).BuildParsed();");
      writer.WriteLine("}");
    }

    /// <summary>
    /// Returns whether or not the specified message type has any required fields.
    /// If it doesn't, calls to check for initialization can be optimised.
    /// TODO(jonskeet): Move this into MessageDescriptor?
    /// </summary>
    private static bool HasRequiredFields(MessageDescriptor descriptor, Dictionary<MessageDescriptor,object> alreadySeen) {
      if (alreadySeen.ContainsKey(descriptor)) {
        // The type is already in cache.  This means that either:
        // a. The type has no required fields.
        // b. We are in the midst of checking if the type has required fields,
        //    somewhere up the stack.  In this case, we know that if the type
        //    has any required fields, they'll be found when we return to it,
        //    and the whole call to HasRequiredFields() will return true.
        //    Therefore, we don't have to check if this type has required fields
        //    here.
        return false;
      }
      alreadySeen[descriptor] = descriptor; // Value is irrelevant

      // If the type has extensions, an extension with message type could contain
      // required fields, so we have to be conservative and assume such an
      // extension exists.
      if (descriptor.Extensions.Count > 0) {
        return true;
      }

      foreach (FieldDescriptor field in descriptor.Fields) {
        if (field.IsRequired) {
          return true;
        }
        // Message or group
        if (field.MappedType == MappedType.Message) {
          if (HasRequiredFields(field.MessageType, alreadySeen)) {
            return true;
          }
        }
      }
      return false;
    }

    private void GenerateBuilder(TextGenerator writer) {
      writer.WriteLine("public static Builder CreateBuilder() { return new Builder(); }");
      writer.WriteLine("public override Builder CreateBuilderForType() { return new Builder(); }");
      writer.WriteLine("public static Builder CreateBuilder({0} prototype) {{", ClassName);
      writer.WriteLine("  return (Builder) new Builder().MergeFrom(prototype);");
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("{0} sealed partial class Builder : pb::{2}Builder<{1}, Builder> {{",
          ClassAccessLevel, ClassName, Descriptor.Proto.ExtensionRangeCount > 0 ? "Extendable" : "Generated");
      writer.Indent();
      writer.WriteLine("protected override Builder ThisBuilder {");
      writer.WriteLine("  get { return this; }");
      writer.WriteLine("}");
      GenerateCommonBuilderMethods(writer);
      if (Descriptor.File.Options.OptimizeFor == FileOptions.Types.OptimizeMode.SPEED) {
        GenerateBuilderParsingMethods(writer);
      }
      foreach (FieldDescriptor field in Descriptor.Fields) {
        writer.WriteLine();
        // No field comment :(
        SourceGenerators.CreateFieldGenerator(field).GenerateBuilderMembers(writer);
      }
      writer.Outdent();
      writer.WriteLine("}");
      writer.Outdent();
      writer.WriteLine("}");
      writer.WriteLine();
    }

    private void GenerateCommonBuilderMethods(TextGenerator writer) {
      writer.WriteLine("{0} Builder() {{}}", ClassAccessLevel);
      writer.WriteLine();
      writer.WriteLine("{0} result = new {0}();", ClassName);
      writer.WriteLine();
      writer.WriteLine("protected override {0} MessageBeingBuilt {{", ClassName);
      writer.WriteLine("  get { return result; }");
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("public override Builder Clear() {");
      writer.WriteLine("  result = new {0}();", ClassName);
      writer.WriteLine("  return this;");
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("public override Builder Clone() {");
      writer.WriteLine("  return new Builder().MergeFrom(result);");
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("public override pbd::MessageDescriptor DescriptorForType {");
      writer.WriteLine("  get {{ return {0}.Descriptor; }}", ClassName);
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("public override {0} DefaultInstanceForType {{", ClassName);
      writer.WriteLine("  get {{ return {0}.DefaultInstance; }}", ClassName);
      writer.WriteLine("}");
      writer.WriteLine();
    
      writer.WriteLine("public override {0} BuildPartial() {{", ClassName);
      writer.Indent();
      foreach (FieldDescriptor field in Descriptor.Fields) {
        SourceGenerators.CreateFieldGenerator(field).GenerateBuildingCode(writer);
      }
      writer.WriteLine("{0} returnMe = result;", ClassName);
      writer.WriteLine("result = null;");
      writer.WriteLine("return returnMe;");
      writer.Outdent();
      writer.WriteLine("}");
      writer.WriteLine();

      if (Descriptor.File.Options.OptimizeFor == FileOptions.Types.OptimizeMode.SPEED) {
        writer.WriteLine("public override Builder MergeFrom(pb::IMessage other) {");
        writer.WriteLine("  if (other is {0}) {{", ClassName);
        writer.WriteLine("    return MergeFrom(({0}) other);", ClassName);
        writer.WriteLine("  } else {");
        writer.WriteLine("    base.MergeFrom(other);");
        writer.WriteLine("    return this;");
        writer.WriteLine("  }");
        writer.WriteLine("}");
        writer.WriteLine();
        writer.WriteLine("public override Builder MergeFrom({0} other) {{", ClassName);
        // Optimization:  If other is the default instance, we know none of its
        // fields are set so we can skip the merge.
        writer.Indent();
        writer.WriteLine("if (other == {0}.DefaultInstance) return this;", ClassName);
        foreach (FieldDescriptor field in Descriptor.Fields) {
          SourceGenerators.CreateFieldGenerator(field).GenerateMergingCode(writer);
        }
        writer.WriteLine("this.MergeUnknownFields(other.UnknownFields);");
        writer.WriteLine("return this;");
        writer.Outdent();
        writer.WriteLine("}");
        writer.WriteLine();
      }
    }

    private void GenerateBuilderParsingMethods(TextGenerator writer) {
      List<FieldDescriptor> sortedFields = new List<FieldDescriptor>(Descriptor.Fields);
      sortedFields.Sort((f1, f2) => f1.FieldNumber.CompareTo(f2.FieldNumber));

      writer.WriteLine("public override Builder MergeFrom(pb::CodedInputStream input) {");
      writer.WriteLine("  return MergeFrom(input, pb::ExtensionRegistry.Empty);");
      writer.WriteLine("}");
      writer.WriteLine();
      writer.WriteLine("public override Builder MergeFrom(pb::CodedInputStream input, pb::ExtensionRegistry extensionRegistry) {");
      writer.Indent();
      writer.WriteLine("pb::UnknownFieldSet.Builder unknownFields = pb::UnknownFieldSet.CreateBuilder(this.UnknownFields);");
      writer.WriteLine("while (true) {");
      writer.Indent();
      writer.WriteLine("uint tag = input.ReadTag();");
      writer.WriteLine("switch (tag) {");
      writer.Indent();
      writer.WriteLine("case 0: {"); // 0 signals EOF / limit reached
      writer.WriteLine("  this.UnknownFields = unknownFields.Build();");
      writer.WriteLine("  return this;");
      writer.WriteLine("}");
      writer.WriteLine("default: {");
      writer.WriteLine("  if (!ParseUnknownField(input, unknownFields, extensionRegistry, tag)) {");
      writer.WriteLine("    this.UnknownFields = unknownFields.Build();");
      writer.WriteLine("    return this;"); // it's an endgroup tag
      writer.WriteLine("  }");
      writer.WriteLine("  break;");
      writer.WriteLine("}");
      foreach (FieldDescriptor field in sortedFields) {
        uint tag = WireFormat.MakeTag(field.FieldNumber, WireFormat.GetWireType(field.FieldType));
        writer.WriteLine("case {0}: {{", tag);
        writer.Indent();
        SourceGenerators.CreateFieldGenerator(field).GenerateParsingCode(writer);
        writer.WriteLine("break;");
        writer.Outdent();
        writer.WriteLine("}");
      }
      writer.Outdent();
      writer.WriteLine("}");
      writer.Outdent();
      writer.WriteLine("}");
      writer.Outdent();
      writer.WriteLine("}");
      writer.WriteLine();
    }

    private void GenerateIsInitialized(TextGenerator writer) {
      writer.WriteLine("public override bool IsInitialized {");
      writer.Indent();
      writer.WriteLine("get {");
      writer.Indent();

      // Check that all required fields in this message are set.
      // TODO(kenton):  We can optimize this when we switch to putting all the
      // "has" fields into a single bitfield.
      foreach (FieldDescriptor field in Descriptor.Fields) {
        if (field.IsRequired) {
          writer.WriteLine("if (!has{0}) return false;", Helpers.UnderscoresToPascalCase(field.Name));
        }
      }
  
      // Now check that all embedded messages are initialized.
      foreach (FieldDescriptor field in Descriptor.Fields) {
        if (field.FieldType != FieldType.Message ||
            !HasRequiredFields(field.MessageType, new Dictionary<MessageDescriptor, object>())) {
          continue;
        }
        string propertyName = Helpers.UnderscoresToPascalCase(DescriptorUtil.GetFieldName(field));
        if (field.IsRepeated) {
          writer.WriteLine("foreach ({0} element in {1}List) {{", DescriptorUtil.GetClassName(field.MessageType), propertyName);
          writer.WriteLine("  if (!element.IsInitialized) return false;");
          writer.WriteLine("}");
        } else if (field.IsOptional) {
          writer.WriteLine("if (Has{0}) {{", propertyName);
          writer.WriteLine("  if (!{0}.IsInitialized) return false;", propertyName);
          writer.WriteLine("}");
        } else {
          writer.WriteLine("if (!{0}.IsInitialized) return false;", propertyName);
        }
      }

      if (Descriptor.Extensions.Count > 0) {
        writer.WriteLine("if (!ExtensionsAreInitialized) return false;");
      }
      writer.WriteLine("return true;");
      writer.Outdent();
      writer.WriteLine("}");
      writer.Outdent();
      writer.WriteLine("}");
      writer.WriteLine();
    }
  }
}
