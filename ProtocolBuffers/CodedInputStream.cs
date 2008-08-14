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
using System.Text;
using Google.ProtocolBuffers.Descriptors;

namespace Google.ProtocolBuffers {

  /// <summary>
  /// Readings and decodes protocol message fields.
  /// </summary>
  /// <remarks>
  /// This class contains two kinds of methods:  methods that read specific
  /// protocol message constructs and field types (e.g. ReadTag and
  /// ReadInt32) and methods that read low-level values (e.g.
  /// ReadRawVarint32 and ReadRawBytes).  If you are reading encoded protocol
  /// messages, you should use the former methods, but if you are reading some
  /// other format of your own design, use the latter. The names of the former
  /// methods are taken from the protocol buffer type names, not .NET types.
  /// (Hence ReadFloat instead of ReadSingle, and ReadBool instead of ReadBoolean.)
  /// 
  /// TODO(jonskeet): Consider whether recursion and size limits shouldn't be readonly,
  /// set at construction time.
  /// </remarks>
  public sealed class CodedInputStream {
    private readonly byte[] buffer;
    private int bufferSize;
    private int bufferSizeAfterLimit = 0;
    private int bufferPos = 0;
    private readonly Stream input;
    private uint lastTag = 0;

    const int DefaultRecursionLimit = 64;
    const int DefaultSizeLimit = 64 << 20; // 64MB
    const int BufferSize = 4096;
    
    /// <summary>
    /// The total number of bytes read before the current buffer. The
    /// total bytes read up to the current position can be computed as
    /// totalBytesRetired + bufferPos.
    /// </summary>
    private int totalBytesRetired = 0;

    /// <summary>
    /// The absolute position of the end of the current message.
    /// </summary> 
    private int currentLimit = int.MaxValue;

    /// <summary>
    /// <see cref="SetRecursionLimit"/>
    /// </summary>
    private int recursionDepth = 0;
    private int recursionLimit = DefaultRecursionLimit;

    /// <summary>
    /// <see cref="SetSizeLimit"/>
    /// </summary>
    private int sizeLimit = DefaultSizeLimit;

    #region Construction
    /// <summary>
    /// Creates a new CodedInputStream reading data from the given
    /// stream.
    /// </summary>
    public static CodedInputStream CreateInstance(Stream input) {
      return new CodedInputStream(input);
    }

    /// <summary>
    /// Creates a new CodedInputStream reading data from the given
    /// byte array.
    /// </summary>
    public static CodedInputStream CreateInstance(byte[] buf) {
      return new CodedInputStream(buf);
    }

    private CodedInputStream(byte[] buffer) {
      this.buffer = buffer;
      this.bufferSize = buffer.Length;
      this.input = null;
    }

    private CodedInputStream(Stream input) {
      this.buffer = new byte[BufferSize];
      this.bufferSize = 0;
      this.input = input;
    }
    #endregion

    #region Validation
    /// <summary>
    /// Verifies that the last call to ReadTag() returned the given tag value.
    /// This is used to verify that a nested group ended with the correct
    /// end tag.
    /// </summary>
    /// <exception cref="InvalidProtocolBufferException">The last
    /// tag read was not the one specified</exception>
    public void CheckLastTagWas(uint value) {
      if (lastTag != value) {
        throw InvalidProtocolBufferException.InvalidEndTag();
      }
    }
    #endregion

    #region Reading of tags etc
    /// <summary>
    /// Attempt to read a field tag, returning 0 if we have reached the end
    /// of the input data. Protocol message parsers use this to read tags,
    /// since a protocol message may legally end wherever a tag occurs, and
    /// zero is not a valid tag number.
    /// </summary>
    public uint ReadTag() {
      if (bufferPos == bufferSize && !RefillBuffer(false)) {
        lastTag = 0;
        return 0;
      }

      lastTag = ReadRawVarint32();
      if (lastTag == 0) {
        // If we actually read zero, that's not a valid tag.
        throw InvalidProtocolBufferException.InvalidTag();
      }
      return lastTag;
    }

    /// <summary>
    /// Read a double field from the stream.
    /// </summary>
    public double ReadDouble() {
      // TODO(jonskeet): Test this on different endiannesses
      return BitConverter.Int64BitsToDouble((long) ReadRawLittleEndian64());
    }

    /// <summary>
    /// Read a float field from the stream.
    /// </summary>
    public float ReadFloat() {
      // TODO(jonskeet): Test this on different endiannesses
      uint raw = ReadRawLittleEndian32();
      byte[] rawBytes = BitConverter.GetBytes(raw);
      return BitConverter.ToSingle(rawBytes, 0);
    }

    /// <summary>
    /// Read a uint64 field from the stream.
    /// </summary>
    public ulong ReadUInt64() {
      return ReadRawVarint64();
    }

    /// <summary>
    /// Read an int64 field from the stream.
    /// </summary>
    public long ReadInt64() {
      return (long) ReadRawVarint64();
    }

    /// <summary>
    /// Read an int32 field from the stream.
    /// </summary>
    public int ReadInt32() {
      return (int) ReadRawVarint32();
    }

    /// <summary>
    /// Read a fixed64 field from the stream.
    /// </summary>
    public ulong ReadFixed64() {
      return ReadRawLittleEndian64();
    }

    /// <summary>
    /// Read a fixed32 field from the stream.
    /// </summary>
    public uint ReadFixed32() {
      return ReadRawLittleEndian32();
    }

    /// <summary>
    /// Read a bool field from the stream.
    /// </summary>
    public bool ReadBool() {
      return ReadRawVarint32() != 0;
    }

    /// <summary>
    /// Reads a string field from the stream.
    /// </summary>
    public String ReadString() {
      int size = (int) ReadRawVarint32();
      if (size < bufferSize - bufferPos && size > 0) {
        // Fast path:  We already have the bytes in a contiguous buffer, so
        //   just copy directly from it.
        String result = Encoding.UTF8.GetString(buffer, bufferPos, size);
        bufferPos += size;
        return result;
      } else {
        // Slow path:  Build a byte array first then copy it.
        return Encoding.UTF8.GetString(ReadRawBytes(size));
      }
    }

    /// <summary>
    /// Reads a group field value from the stream.
    /// </summary>    
    public void ReadGroup(int fieldNumber, IBuilder builder,
                          ExtensionRegistry extensionRegistry) {
      if (recursionDepth >= recursionLimit) {
        throw InvalidProtocolBufferException.RecursionLimitExceeded();
      }
      ++recursionDepth;
      builder.WeakMergeFrom(this, extensionRegistry);
      CheckLastTagWas(WireFormat.MakeTag(fieldNumber, WireFormat.WireType.EndGroup));
      --recursionDepth;
    }

    /// <summary>
    /// Reads a group field value from the stream and merges it into the given
    /// UnknownFieldSet.
    /// </summary>   
    public void ReadUnknownGroup(int fieldNumber, UnknownFieldSet.Builder builder) {
      if (recursionDepth >= recursionLimit) {
        throw InvalidProtocolBufferException.RecursionLimitExceeded();
      }
      ++recursionDepth;
      builder.MergeFrom(this);
      CheckLastTagWas(WireFormat.MakeTag(fieldNumber, WireFormat.WireType.EndGroup));
      --recursionDepth;
    }

    /// <summary>
    /// Reads an embedded message field value from the stream.
    /// </summary>   
    public void ReadMessage(IBuilder builder, ExtensionRegistry extensionRegistry) {
      int length = (int) ReadRawVarint32();
      if (recursionDepth >= recursionLimit) {
        throw InvalidProtocolBufferException.RecursionLimitExceeded();
      }
      int oldLimit = PushLimit(length);
      ++recursionDepth;
      builder.WeakMergeFrom(this, extensionRegistry);
      CheckLastTagWas(0);
      --recursionDepth;
      PopLimit(oldLimit);
    }

    /// <summary>
    /// Reads a bytes field value from the stream.
    /// </summary>   
    public ByteString ReadBytes() {
      int size = (int) ReadRawVarint32();
      if (size < bufferSize - bufferPos && size > 0) {
        // Fast path:  We already have the bytes in a contiguous buffer, so
        //   just copy directly from it.
        ByteString result = ByteString.CopyFrom(buffer, bufferPos, size);
        bufferPos += size;
        return result;
      } else {
        // Slow path:  Build a byte array first then copy it.
        return ByteString.CopyFrom(ReadRawBytes(size));
      }
    }

    /// <summary>
    /// Reads a uint32 field value from the stream.
    /// </summary>   
    public uint ReadUInt32() {
      return ReadRawVarint32();
    }

    /// <summary>
    /// Reads an enum field value from the stream. The caller is responsible
    /// for converting the numeric value to an actual enum.
    /// </summary>   
    public int ReadEnum() {
      return (int) ReadRawVarint32();
    }

    /// <summary>
    /// Reads an sfixed32 field value from the stream.
    /// </summary>   
    public int ReadSFixed32() {
      return (int) ReadRawLittleEndian32();
    }

    /// <summary>
    /// Reads an sfixed64 field value from the stream.
    /// </summary>   
    public long ReadSFixed64() {
      return (long) ReadRawLittleEndian64();
    }

    /// <summary>
    /// Reads an sint32 field value from the stream.
    /// </summary>   
    public int ReadSInt32() {
      return DecodeZigZag32(ReadRawVarint32());
    }

    /// <summary>
    /// Reads an sint64 field value from the stream.
    /// </summary>   
    public long ReadSInt64() {
      return DecodeZigZag64(ReadRawVarint64());
    }

    /// <summary>
    /// Reads a field of any primitive type. Enums, groups and embedded
    /// messages are not handled by this method.
    /// </summary>
    public object ReadPrimitiveField(FieldType fieldType) {
      switch (fieldType) {
        case FieldType.Double:   return ReadDouble();
        case FieldType.Float:    return ReadFloat();
        case FieldType.Int64:    return ReadInt64();
        case FieldType.UInt64:   return ReadUInt64();
        case FieldType.Int32:    return ReadInt32();
        case FieldType.Fixed64:  return ReadFixed64();
        case FieldType.Fixed32:  return ReadFixed32();
        case FieldType.Bool:     return ReadBool();
        case FieldType.String:   return ReadString();
        case FieldType.Bytes:    return ReadBytes();
        case FieldType.UInt32:   return ReadUInt32();
        case FieldType.SFixed32: return ReadSFixed32();
        case FieldType.SFixed64: return ReadSFixed64();
        case FieldType.SInt32:   return ReadSInt32();
        case FieldType.SInt64:   return ReadSInt64();
        case FieldType.Group:
            throw new ArgumentException("ReadPrimitiveField() cannot handle nested groups.");
        case FieldType.Message:
            throw new ArgumentException("ReadPrimitiveField() cannot handle embedded messages.");
        // We don't handle enums because we don't know what to do if the
        // value is not recognized.
        case FieldType.Enum:
            throw new ArgumentException("ReadPrimitiveField() cannot handle enums.");
        default:
          throw new ArgumentOutOfRangeException("Invalid field type " + fieldType);
      }
    }

    #endregion

    #region Underlying reading primitives
    /// <summary>
    /// Read a raw Varint from the stream.  If larger than 32 bits, discard the upper bits.
    /// </summary>
    /// <returns></returns>
    public uint ReadRawVarint32() {
      int tmp = ReadRawByte();
      if (tmp < 128) {
        return (uint) tmp;
      }
      int result = tmp & 0x7f;
      if ((tmp = ReadRawByte()) < 128) {
        result |= tmp << 7;
      } else {
        result |= (tmp & 0x7f) << 7;
        if ((tmp = ReadRawByte()) < 128) {
          result |= tmp << 14;
        } else {
          result |= (tmp & 0x7f) << 14;
          if ((tmp = ReadRawByte()) < 128) {
            result |= tmp << 21;
          } else {
            result |= (tmp & 0x7f) << 21;
            result |= (tmp = ReadRawByte()) << 28;
            if (tmp >= 128) {
              // Discard upper 32 bits.
              for (int i = 0; i < 5; i++) {
                if (ReadRawByte() < 128) return (uint) result;
              }
              throw InvalidProtocolBufferException.MalformedVarint();
            }
          }
        }
      }
      return (uint) result;
    }

    /// <summary>
    /// Read a raw varint from the stream.
    /// </summary>
    public ulong ReadRawVarint64() {
      int shift = 0;
      ulong result = 0;
      while (shift < 64) {
        byte b = ReadRawByte();
        result |= (ulong)(b & 0x7F) << shift;
        if ((b & 0x80) == 0) {
          return result;
        }
        shift += 7;
      }
      throw InvalidProtocolBufferException.MalformedVarint();
    }

    /// <summary>
    /// Read a 32-bit little-endian integer from the stream.
    /// </summary>
    public uint ReadRawLittleEndian32() {
      uint b1 = ReadRawByte();
      uint b2 = ReadRawByte();
      uint b3 = ReadRawByte();
      uint b4 = ReadRawByte();
      return b1 | (b2 << 8) | (b3 << 16) | (b4 << 24);
    }

    /// <summary>
    /// Read a 64-bit little-endian integer from the stream.
    /// </summary>
    public ulong ReadRawLittleEndian64() {
      ulong b1 = ReadRawByte();
      ulong b2 = ReadRawByte();
      ulong b3 = ReadRawByte();
      ulong b4 = ReadRawByte();
      ulong b5 = ReadRawByte();
      ulong b6 = ReadRawByte();
      ulong b7 = ReadRawByte();
      ulong b8 = ReadRawByte();
      return b1 | (b2 << 8) | (b3 << 16) | (b4 << 24)
          | (b5 << 32) | (b6 << 40) | (b7 << 48) | (b8 << 56);
    }
    #endregion

    /// <summary>
    /// Decode a 32-bit value with ZigZag encoding.
    /// </summary>
    /// <remarks>
    /// ZigZag encodes signed integers into values that can be efficiently
    /// encoded with varint.  (Otherwise, negative values must be 
    /// sign-extended to 64 bits to be varint encoded, thus always taking
    /// 10 bytes on the wire.)
    /// </remarks>
    public static int DecodeZigZag32(uint n) {
      return (int)(n >> 1) ^ -(int)(n & 1);
    }

    /// <summary>
    /// Decode a 32-bit value with ZigZag encoding.
    /// </summary>
    /// <remarks>
    /// ZigZag encodes signed integers into values that can be efficiently
    /// encoded with varint.  (Otherwise, negative values must be 
    /// sign-extended to 64 bits to be varint encoded, thus always taking
    /// 10 bytes on the wire.)
    /// </remarks>
    public static long DecodeZigZag64(ulong n) {
      return (long)(n >> 1) ^ -(long)(n & 1);
    }

    /// <summary>
    /// Set the maximum message recursion depth.
    /// </summary>
    /// <remarks>
    /// In order to prevent malicious
    /// messages from causing stack overflows, CodedInputStream limits
    /// how deeply messages may be nested.  The default limit is 64.
    /// </remarks>
    public int SetRecursionLimit(int limit) {
      if (limit < 0) {
        throw new ArgumentOutOfRangeException("Recursion limit cannot be negative: " + limit);
      }
      int oldLimit = recursionLimit;
      recursionLimit = limit;
      return oldLimit;
    }

    /// <summary>
    /// Set the maximum message size.
    /// </summary>
    /// <remarks>
    /// In order to prevent malicious messages from exhausting memory or
    /// causing integer overflows, CodedInputStream limits how large a message may be.
    /// The default limit is 64MB.  You should set this limit as small
    /// as you can without harming your app's functionality.  Note that
    /// size limits only apply when reading from an InputStream, not
    /// when constructed around a raw byte array (nor with ByteString.NewCodedInput).
    /// </remarks>
    public int SetSizeLimit(int limit) {
      if (limit < 0) {
        throw new ArgumentOutOfRangeException("Size limit cannot be negative: " + limit);
      }
      int oldLimit = sizeLimit;
      sizeLimit = limit;
      return oldLimit;
    }

    #region Internal reading and buffer management
    /// <summary>
    /// Sets currentLimit to (current position) + byteLimit. This is called
    /// when descending into a length-delimited embedded message. The previous
    /// limit is returned.
    /// </summary>
    /// <returns>The old limit.</returns>
    public int PushLimit(int byteLimit) {
      if (byteLimit < 0) {
        throw InvalidProtocolBufferException.NegativeSize();
      }
      byteLimit += totalBytesRetired + bufferPos;
      int oldLimit = currentLimit;
      if (byteLimit > oldLimit) {
        throw InvalidProtocolBufferException.TruncatedMessage();
      }
      currentLimit = byteLimit;

      RecomputeBufferSizeAfterLimit();

      return oldLimit;
    }

    private void RecomputeBufferSizeAfterLimit() {
      bufferSize += bufferSizeAfterLimit;
      int bufferEnd = totalBytesRetired + bufferSize;
      if (bufferEnd > currentLimit) {
        // Limit is in current buffer.
        bufferSizeAfterLimit = bufferEnd - currentLimit;
        bufferSize -= bufferSizeAfterLimit;
      } else {
        bufferSizeAfterLimit = 0;
      }
    }

    /// <summary>
    /// Discards the current limit, returning the previous limit.
    /// </summary>
    public void PopLimit(int oldLimit) {
      currentLimit = oldLimit;
      RecomputeBufferSizeAfterLimit();
    }

    /// <summary>
    /// Called when buffer is empty to read more bytes from the
    /// input.  If <paramref name="mustSucceed"/> is true, RefillBuffer() gurantees that
    /// either there will be at least one byte in the buffer when it returns
    /// or it will throw an exception.  If <paramref name="mustSucceed"/> is false,
    /// RefillBuffer() returns false if no more bytes were available.
    /// </summary>
    /// <param name="mustSucceed"></param>
    /// <returns></returns>
    private bool RefillBuffer(bool mustSucceed)  {
      if (bufferPos < bufferSize) {
        throw new InvalidOperationException("RefillBuffer() called when buffer wasn't empty.");
      }

      if (totalBytesRetired + bufferSize == currentLimit) {
        // Oops, we hit a limit.
        if (mustSucceed) {
          throw InvalidProtocolBufferException.TruncatedMessage();
        } else {
          return false;
        }
      }

      totalBytesRetired += bufferSize;

      bufferPos = 0;
      bufferSize = (input == null) ? 0 : input.Read(buffer, 0, buffer.Length);
      if (bufferSize == 0) {
        bufferSize = 0;
        if (mustSucceed) {
          throw InvalidProtocolBufferException.TruncatedMessage();
        } else {
          return false;
        }
      } else {
        RecomputeBufferSizeAfterLimit();
        int totalBytesRead =
          totalBytesRetired + bufferSize + bufferSizeAfterLimit;
        if (totalBytesRead > sizeLimit || totalBytesRead < 0) {
          throw InvalidProtocolBufferException.SizeLimitExceeded();
        }
        return true;
      }
    }

    /// <summary>
    /// Read one byte from the input.
    /// </summary>
    /// <exception cref="InvalidProtocolBufferException">
    /// he end of the stream or the current limit was reached
    /// </exception>
    public byte ReadRawByte() {
      if (bufferPos == bufferSize) {
        RefillBuffer(true);
      }
      return buffer[bufferPos++];
    }
    
    /// <summary>
    /// Read a fixed size of bytes from the input.
    /// </summary>
    /// <exception cref="InvalidProtocolBufferException">
    /// the end of the stream or the current limit was reached
    /// </exception>
    public byte[] ReadRawBytes(int size) {
      if (size < 0) {
        throw InvalidProtocolBufferException.NegativeSize();
      }

      if (totalBytesRetired + bufferPos + size > currentLimit) {
        // Read to the end of the stream anyway.
        SkipRawBytes(currentLimit - totalBytesRetired - bufferPos);
        // Then fail.
        throw InvalidProtocolBufferException.TruncatedMessage();
      }

      if (size <= bufferSize - bufferPos) {
        // We have all the bytes we need already.
        byte[] bytes = new byte[size];
        Array.Copy(buffer, bufferPos, bytes, 0, size);
        bufferPos += size;
        return bytes;
      } else if (size < BufferSize) {
        // Reading more bytes than are in the buffer, but not an excessive number
        // of bytes.  We can safely allocate the resulting array ahead of time.

        // First copy what we have.
        byte[] bytes = new byte[size];
        int pos = bufferSize - bufferPos;
        Array.Copy(buffer, bufferPos, bytes, 0, pos);
        bufferPos = bufferSize;

        // We want to use RefillBuffer() and then copy from the buffer into our
        // byte array rather than reading directly into our byte array because
        // the input may be unbuffered.
        RefillBuffer(true);

        while (size - pos > bufferSize) {
          Array.Copy(buffer, 0, bytes, pos, bufferSize);
          pos += bufferSize;
          bufferPos = bufferSize;
          RefillBuffer(true);
        }

        Array.Copy(buffer, 0, bytes, pos, size - pos);
        bufferPos = size - pos;

        return bytes;
      } else {
        // The size is very large.  For security reasons, we can't allocate the
        // entire byte array yet.  The size comes directly from the input, so a
        // maliciously-crafted message could provide a bogus very large size in
        // order to trick the app into allocating a lot of memory.  We avoid this
        // by allocating and reading only a small chunk at a time, so that the
        // malicious message must actually *be* extremely large to cause
        // problems.  Meanwhile, we limit the allowed size of a message elsewhere.

        // Remember the buffer markers since we'll have to copy the bytes out of
        // it later.
        int originalBufferPos = bufferPos;
        int originalBufferSize = bufferSize;

        // Mark the current buffer consumed.
        totalBytesRetired += bufferSize;
        bufferPos = 0;
        bufferSize = 0;

        // Read all the rest of the bytes we need.
        int sizeLeft = size - (originalBufferSize - originalBufferPos);
        List<byte[]> chunks = new List<byte[]>();

        while (sizeLeft > 0) {
          byte[] chunk = new byte[Math.Min(sizeLeft, BufferSize)];
          int pos = 0;
          while (pos < chunk.Length) {
            int n = (input == null) ? -1 : input.Read(chunk, pos, chunk.Length - pos);
            if (n <= 0) {
              throw InvalidProtocolBufferException.TruncatedMessage();
            }
            totalBytesRetired += n;
            pos += n;
          }
          sizeLeft -= chunk.Length;
          chunks.Add(chunk);
        }

        // OK, got everything.  Now concatenate it all into one buffer.
        byte[] bytes = new byte[size];

        // Start by copying the leftover bytes from this.buffer.
        int newPos = originalBufferSize - originalBufferPos;
        Array.Copy(buffer, originalBufferPos, bytes, 0, newPos);

        // And now all the chunks.
        foreach (byte[] chunk in chunks) {
          Array.Copy(chunk, 0, bytes, newPos, chunk.Length);
          newPos += chunk.Length;
        }

        // Done.
        return bytes;
      }
    }

    /// <summary>
    /// Reads and discards a single field, given its tag value.
    /// </summary>
    /// <returns>false if the tag is an end-group tag, in which case
    /// nothing is skipped. Otherwise, returns true.</returns>
    public bool SkipField(uint tag) {
      switch (WireFormat.GetTagWireType(tag)) {
        case WireFormat.WireType.Varint:
          ReadInt32();
          return true;
        case WireFormat.WireType.Fixed64:
          ReadRawLittleEndian64();
          return true;
        case WireFormat.WireType.LengthDelimited:
          SkipRawBytes((int) ReadRawVarint32());
          return true;
        case WireFormat.WireType.StartGroup:
          SkipMessage();
          CheckLastTagWas(
            WireFormat.MakeTag(WireFormat.GetTagFieldNumber(tag),
                               WireFormat.WireType.EndGroup));
          return true;
        case WireFormat.WireType.EndGroup:
          return false;
        case WireFormat.WireType.Fixed32:
          ReadRawLittleEndian32();
          return true;
        default:
          throw InvalidProtocolBufferException.InvalidWireType();
      }
    }

    /// <summary>
    /// Reads and discards an entire message.  This will read either until EOF
    /// or until an endgroup tag, whichever comes first.
    /// </summary>
    public void SkipMessage() {
      while (true) {
        uint tag = ReadTag();
        if (tag == 0 || !SkipField(tag)) {
          return;
        }
      }
    }

    /// <summary>
    /// Reads and discards <paramref name="size"/> bytes.
    /// </summary>
    /// <exception cref="InvalidProtocolBufferException">the end of the stream
    /// or the current limit was reached</exception>
    public void SkipRawBytes(int size) {
      if (size < 0) {
        throw InvalidProtocolBufferException.NegativeSize();
      }

      if (totalBytesRetired + bufferPos + size > currentLimit) {
        // Read to the end of the stream anyway.
        SkipRawBytes(currentLimit - totalBytesRetired - bufferPos);
        // Then fail.
        throw InvalidProtocolBufferException.TruncatedMessage();
      }

      if (size < bufferSize - bufferPos) {
        // We have all the bytes we need already.
        bufferPos += size;
      } else {
        // Skipping more bytes than are in the buffer.  First skip what we have.
        int pos = bufferSize - bufferPos;
        totalBytesRetired += pos;
        bufferPos = 0;
        bufferSize = 0;

        // Then skip directly from the InputStream for the rest.
        if (pos < size) {
          // TODO(jonskeet): Java implementation uses skip(). Not sure whether this is really equivalent...
          if (input == null) {
            throw InvalidProtocolBufferException.TruncatedMessage();
          }
          input.Seek(size - pos, SeekOrigin.Current);
          if (input.Position > input.Length) {
            throw InvalidProtocolBufferException.TruncatedMessage();
          }
          totalBytesRetired += size - pos;
        }
      }
    }
    #endregion
  }
}
