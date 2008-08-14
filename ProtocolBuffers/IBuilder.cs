﻿// Protocol Buffers - Google's data interchange format
// Copyright 2008 Google Inc.
// http://code.google.com/p/protobuf/
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.IO;
using Google.ProtocolBuffers.Descriptors;

namespace Google.ProtocolBuffers {

  /// <summary>
  /// Non-generic interface for all members whose signatures don't require knowledge of
  /// the type being built. The generic interface extends this one. Some methods return
  /// either an IBuilder or an IMessage; in these cases the generic interface redeclares
  /// the same method with a type-specific signature. Implementations are encouraged to
  /// use explicit interface implemenation for the non-generic form. This mirrors
  /// how IEnumerable and IEnumerable&lt;T&gt; work.
  /// </summary>
  public interface IBuilder {
    /// <summary>
    /// Returns true iff all required fields in the message and all
    /// embedded messages are set.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Behaves like the equivalent property in IMessage&lt;T&gt;.
    /// The returned map may or may not reflect future changes to the builder.
    /// Either way, the returned map is unmodifiable.
    /// </summary>
    IDictionary<FieldDescriptor, object> AllFields { get; }

    /// <summary>
    /// Allows getting and setting of a field.
    /// <see cref="IMessage{TMessage, TBuilder}.Item(FieldDescriptor)"/>
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    object this[FieldDescriptor field] { get; set; }

    /// <summary>
    /// Get the message's type's descriptor.
    /// <see cref="IMessage{TMessage, TBuilder}.DescriptorForType"/>
    /// </summary>
    MessageDescriptor DescriptorForType { get; }

    /// <summary>
    /// Only present in the nongeneric interface - useful for tests, but
    /// not as much in real life.
    /// </summary>
    IBuilder SetField(FieldDescriptor field, object value);

    /// <summary>
    /// Only present in the nongeneric interface - useful for tests, but
    /// not as much in real life.
    /// </summary>
    IBuilder SetRepeatedField(FieldDescriptor field, int index, object value);

    /// <summary>
    /// <see cref="IMessage{TMessage, TBuilder}.GetRepeatedFieldCount"/>
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    int GetRepeatedFieldCount(FieldDescriptor field);

    /// <summary>
    /// Allows getting and setting of a repeated field value.
    /// <see cref="IMessage{TMessage, TBuilder}.Item(FieldDescriptor, int)"/>
    /// </summary>
    object this[FieldDescriptor field, int index] { get; set; }

    /// <summary>
    /// <see cref="IMessage{TMessage, TBuilder}.HasField"/>
    /// </summary>
    bool HasField(FieldDescriptor field);

    /// <summary>
    /// <see cref="IMessage{TMessage, TBuilder}.UnknownFields"/>
    /// </summary>
    UnknownFieldSet UnknownFields { get; set; }

    /// <summary>
    /// Create a builder for messages of the appropriate type for the given field.
    /// Messages built with this can then be passed to the various mutation properties
    /// and methods.
    /// </summary>
    IBuilder CreateBuilderForField(FieldDescriptor field);

    #region Methods which are like those of the generic form, but without any knowledge of the type parameters
    IBuilder WeakAddRepeatedField(FieldDescriptor field, object value);
    IBuilder WeakClear();
    IBuilder WeakClearField(FieldDescriptor field);
    IBuilder WeakMergeFrom(IMessage message);
    IBuilder WeakMergeFrom(ByteString data);
    IBuilder WeakMergeFrom(ByteString data, ExtensionRegistry registry);
    IBuilder WeakMergeFrom(CodedInputStream input);
    IBuilder WeakMergeFrom(CodedInputStream input, ExtensionRegistry registry);
    IMessage WeakBuild();
    IMessage WeakBuildPartial();
    IBuilder WeakClone();
    IMessage WeakDefaultInstanceForType { get; }
    #endregion
  }

  /// <summary>
  /// Interface implemented by Protocol Message builders.
  /// TODO(jonskeet): Consider "SetXXX" methods returning the builder, as well as the properties.
  /// </summary>
  /// <typeparam name="TMessage">Type of message</typeparam>
  /// <typeparam name="TBuilder">Type of builder</typeparam>
  public interface IBuilder<TMessage, TBuilder> : IBuilder
      where TMessage : IMessage<TMessage, TBuilder> 
      where TBuilder : IBuilder<TMessage, TBuilder> {

    TBuilder SetUnknownFields(UnknownFieldSet unknownFields);

    /// <summary>
    /// Resets all fields to their default values.
    /// </summary>
    TBuilder Clear();

    /// <summary>
    /// Merge the specified other message into the message being
    /// built. Merging occurs as follows. For each field:
    /// For singular primitive fields, if the field is set in <paramref name="other"/>,
    /// then <paramref name="other"/>'s value overwrites the value in this message.
    /// For singular message fields, if the field is set in <paramref name="other"/>,
    /// it is merged into the corresponding sub-message of this message using the same
    /// merging rules.
    /// For repeated fields, the elements in <paramref name="other"/> are concatenated
    /// with the elements in this message.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    TBuilder MergeFrom(TMessage other);

    /// <summary>
    /// Merge the specified other message which may be a different implementation of
    /// the same message descriptor.
    /// </summary>
    TBuilder MergeFrom(IMessage other);

    /// <summary>
    /// Constructs the final message. Once this is called, this Builder instance
    /// is no longer valid, and calling any other method may throw a
    /// NullReferenceException. If you need to continue working with the builder
    /// after calling Build, call Clone first.
    /// </summary>
    /// <exception cref="UninitializedMessageException">the message
    /// is missing one or more required fields; use BuildPartial to bypass
    /// this check</exception>
    TMessage Build();

    /// <summary>
    /// Like Build(), but does not throw an exception if the message is missing
    /// required fields. Instead, a partial message is returned.
    /// </summary>
    TMessage BuildPartial();

    /// <summary>
    /// Clones this builder.
    /// TODO(jonskeet): Explain depth of clone.
    /// </summary>
    TBuilder Clone();

    /// <summary>
    /// Parses a message of this type from the input and merges it with this
    /// message, as if using MergeFrom(IMessage&lt;T&gt;).
    /// </summary>
    /// <remarks>
    /// Warning: This does not verify that all required fields are present
    /// in the input message. If you call Build() without setting all
    /// required fields, it will throw an UninitializedMessageException.
    /// There are a few good ways to deal with this:
    /// <list>
    /// <item>Call IsInitialized to verify to verify that all required fields are
    /// set before building.</item>
    /// <item>Parse  the message separately using one of the static ParseFrom
    /// methods, then use MergeFrom(IMessage&lt;T&gt;) to merge it with
    /// this one. ParseFrom will throw an InvalidProtocolBufferException
    /// (an IOException) if some required fields are missing.
    /// Use BuildPartial to build, which ignores missing required fields.
    /// </list>
    /// </remarks>
    TBuilder MergeFrom(CodedInputStream input);

    /// <summary>
    /// Like MergeFrom(CodedInputStream), but also parses extensions.
    /// The extensions that you want to be able to parse must be registered
    /// in <paramref name="extensionRegistry"/>. Extensions not in the registry
    /// will be treated as unknown fields.
    /// </summary>
    TBuilder MergeFrom(CodedInputStream input, ExtensionRegistry extensionRegistry);

    /// <summary>
    /// Get's the message's type's default instance.
    /// <see cref="IMessage{TMessage}.DefaultInstanceForType" />
    /// </summary>
    TMessage DefaultInstanceForType { get; }

    /// <summary>
    /// Clears the field. This is exactly equivalent to calling the generated
    /// Clear method corresponding to the field.
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    TBuilder ClearField(FieldDescriptor field);

    /// <summary>
    /// Appends the given value as a new element for the specified repeated field.
    /// </summary>
    /// <exception cref="ArgumentException">the field is not a repeated field,
    /// the field does not belong to this builder's type, or the value is
    /// of the incorrect type
    /// </exception>
    TBuilder AddRepeatedField(FieldDescriptor field, object value);

    /// <summary>
    /// Merge some unknown fields into the set for this message.
    /// </summary>
    TBuilder MergeUnknownFields(UnknownFieldSet unknownFields);

    #region Convenience methods
    // TODO(jonskeet): Implement these as extension methods?
    /// <summary>
    /// Parse <paramref name="data"/> as a message of this type and merge
    /// it with the message being built. This is just a small wrapper around
    /// MergeFrom(CodedInputStream).
    /// </summary>
    TBuilder MergeFrom(ByteString data);

    /// <summary>
    /// Parse <paramref name="data"/> as a message of this type and merge
    /// it with the message being built. This is just a small wrapper around
    /// MergeFrom(CodedInputStream, ExtensionRegistry).
    /// </summary>
    TBuilder MergeFrom(ByteString data, ExtensionRegistry extensionRegistry);

    /// <summary>
    /// Parse <paramref name="data"/> as a message of this type and merge
    /// it with the message being built. This is just a small wrapper around
    /// MergeFrom(CodedInputStream).
    /// </summary>
    TBuilder MergeFrom(byte[] data);

    /// <summary>
    /// Parse <paramref name="data"/> as a message of this type and merge
    /// it with the message being built. This is just a small wrapper around
    /// MergeFrom(CodedInputStream, ExtensionRegistry).
    /// </summary>
    TBuilder MergeFrom(byte[] data, ExtensionRegistry extensionRegistry);

    /// <summary>
    /// Parse <paramref name="input"/> as a message of this type and merge
    /// it with the message being built. This is just a small wrapper around
    /// MergeFrom(CodedInputStream). Note that this method always reads
    /// the entire input (unless it throws an exception). If you want it to
    /// stop earlier, you will need to wrap the input in a wrapper
    /// stream which limits reading. Despite usually reading the entire
    /// stream, this method never closes the stream.
    /// </summary>
    TBuilder MergeFrom(Stream input);

    /// <summary>
    /// Parse <paramref name="input"/> as a message of this type and merge
    /// it with the message being built. This is just a small wrapper around
    /// MergeFrom(CodedInputStream, ExtensionRegistry).
    /// </summary>
    TBuilder MergeFrom(Stream input, ExtensionRegistry extensionRegistry);
    #endregion
  }
}
