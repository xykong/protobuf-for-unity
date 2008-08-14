﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Google.ProtocolBuffers.Descriptors;

namespace Google.ProtocolBuffers {
  /// <summary>
  /// Base type for all generated extensions.
  /// </summary>
  /// <remarks>
  /// The protocol compiler generates a static singleton instance of this
  /// class for each extension. For exmaple, imagine a .proto file with:
  /// <code>
  /// message Foo {
  ///   extensions 1000 to max
  /// }
  /// 
  /// extend Foo {
  ///   optional int32 bar;
  /// }
  /// </code>
  /// Then MyProto.Foo.Bar has type GeneratedExtensionBase&lt;MyProto.Foo,int&gt;.
  /// <para />
  /// In general, users should ignore the details of this type, and
  /// simply use the static singletons as parameters to the extension accessors
  /// in ExtendableMessage and ExtendableBuilder.
  /// The interface implemented by both GeneratedException and GeneratedRepeatException,
  /// to make it easier to cope with repeats separately.
  /// </remarks>
  public abstract class GeneratedExtensionBase<TContainer, TExtension> {

    private readonly FieldDescriptor descriptor;
    private readonly IMessage messageDefaultInstance;

    protected GeneratedExtensionBase(FieldDescriptor descriptor, Type singularExtensionType) {
      if (!descriptor.IsExtension) {
        throw new ArgumentException("GeneratedExtension given a regular (non-extension) field.");
      }

      this.descriptor = descriptor;
      if (descriptor.MappedType == MappedType.Message) {
        PropertyInfo defaultInstanceProperty = singularExtensionType
            .GetProperty("DefaultInstance", BindingFlags.Static | BindingFlags.Public);
        if (defaultInstanceProperty == null) {
          throw new ArgumentException("No public static DefaultInstance property for type " + typeof(TExtension).Name);
        }
        messageDefaultInstance = (IMessage)defaultInstanceProperty.GetValue(null, null);
      }
    }

    public FieldDescriptor Descriptor {
      get { return descriptor; }
    }

    /// <summary>
    /// Returns the default message instance for extensions which are message types.
    /// </summary>
    public IMessage MessageDefaultInstance {
      get { return messageDefaultInstance; }
    }

    public object SingularFromReflectionType(object value) {
      switch (Descriptor.MappedType) {
        case MappedType.Message:
          if (value is TExtension) {
            return value;
          } else {
            // It seems the copy of the embedded message stored inside the
            // extended message is not of the exact type the user was
            // expecting.  This can happen if a user defines a
            // GeneratedExtension manually and gives it a different type.
            // This should not happen in normal use.  But, to be nice, we'll
            // copy the message to whatever type the caller was expecting.
            return MessageDefaultInstance.CreateBuilderForType()
                           .MergeFrom((IMessage)value).Build();
          }
        case MappedType.Enum:
          // Just return a boxed int - that can be unboxed to the enum
          return ((EnumValueDescriptor) value).Number;
        default:
          return value;
      }
    }

    /// <summary>
    /// Converts from the type used by the native accessors to the type
    /// used by reflection accessors. For example, the reflection accessors
    /// for enums use EnumValueDescriptors but the native accessors use
    /// the generated enum type.
    /// </summary>
    public object ToReflectionType(object value) {
      if (descriptor.IsRepeated) {
        if (descriptor.MappedType == MappedType.Enum) {
          // Must convert the whole list.
          IList<object> result = new List<object>();
          foreach (object element in (IEnumerable) value) {
            result.Add(SingularToReflectionType(element));
          }
          return result;
        } else {
          return value;
        }
      } else {
        return SingularToReflectionType(value);
      }
    }

    /// <summary>
    /// Like ToReflectionType(object) but for a single element.
    /// </summary>
    internal Object SingularToReflectionType(object value) {
      return descriptor.MappedType == MappedType.Enum
          ? descriptor.EnumType.FindValueByNumber((int) value)
          : value;
    }

    public abstract object FromReflectionType(object value);
  }
}