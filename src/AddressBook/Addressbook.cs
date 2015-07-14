// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: addressbook.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Protobuf.Examples.AddressBook {

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public static partial class Addressbook {

    #region Static variables
    internal static pbr::FieldAccessorTable internal__static_tutorial_Person__FieldAccessorTable;
    internal static pbr::FieldAccessorTable internal__static_tutorial_Person_PhoneNumber__FieldAccessorTable;
    internal static pbr::FieldAccessorTable internal__static_tutorial_AddressBook__FieldAccessorTable;
    #endregion
    #region Descriptor
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static Addressbook() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChFhZGRyZXNzYm9vay5wcm90bxIIdHV0b3JpYWwi1AEKBlBlcnNvbhIMCgRu", 
            "YW1lGAEgASgJEgoKAmlkGAIgASgFEg0KBWVtYWlsGAMgASgJEisKBXBob25l", 
            "GAQgAygLMhwudHV0b3JpYWwuUGVyc29uLlBob25lTnVtYmVyGkcKC1Bob25l", 
            "TnVtYmVyEg4KBm51bWJlchgBIAEoCRIoCgR0eXBlGAIgASgOMhoudHV0b3Jp", 
            "YWwuUGVyc29uLlBob25lVHlwZSIrCglQaG9uZVR5cGUSCgoGTU9CSUxFEAAS", 
            "CAoESE9NRRABEggKBFdPUksQAiIvCgtBZGRyZXNzQm9vaxIgCgZwZXJzb24Y", 
            "ASADKAsyEC50dXRvcmlhbC5QZXJzb25CUAoUY29tLmV4YW1wbGUudHV0b3Jp", 
            "YWxCEUFkZHJlc3NCb29rUHJvdG9zqgIkR29vZ2xlLlByb3RvYnVmLkV4YW1w", 
          "bGVzLkFkZHJlc3NCb29rYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.InternalBuildGeneratedFileFrom(descriptorData,
          new pbr::FileDescriptor[] {
          });
      internal__static_tutorial_Person__FieldAccessorTable = 
          new pbr::FieldAccessorTable(typeof(global::Google.Protobuf.Examples.AddressBook.Person), descriptor.MessageTypes[0],
              new string[] { "Name", "Id", "Email", "Phone", }, new string[] { });
      internal__static_tutorial_Person_PhoneNumber__FieldAccessorTable = 
          new pbr::FieldAccessorTable(typeof(global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber), descriptor.MessageTypes[0].NestedTypes[0],
              new string[] { "Number", "Type", }, new string[] { });
      internal__static_tutorial_AddressBook__FieldAccessorTable = 
          new pbr::FieldAccessorTable(typeof(global::Google.Protobuf.Examples.AddressBook.AddressBook), descriptor.MessageTypes[1],
              new string[] { "Person", }, new string[] { });
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public sealed partial class Person : pb::IMessage<Person> {
    private static readonly pb::MessageParser<Person> _parser = new pb::MessageParser<Person>(() => new Person());
    public static pb::MessageParser<Person> Parser { get { return _parser; } }

    private static readonly string[] _fieldNames = new string[] { "email", "id", "name", "phone" };
    private static readonly uint[] _fieldTags = new uint[] { 26, 16, 10, 34 };
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Examples.AddressBook.Addressbook.Descriptor.MessageTypes[0]; }
    }

    public pbr::FieldAccessorTable Fields {
      get { return global::Google.Protobuf.Examples.AddressBook.Addressbook.internal__static_tutorial_Person__FieldAccessorTable; }
    }

    private bool _frozen = false;
    public bool IsFrozen { get { return _frozen; } }

    public Person() {
      OnConstruction();
    }

    partial void OnConstruction();

    public Person(Person other) : this() {
      name_ = other.name_;
      id_ = other.id_;
      email_ = other.email_;
      phone_ = other.phone_.Clone();
    }

    public Person Clone() {
      return new Person(this);
    }

    public void Freeze() {
      if (IsFrozen) {
        return;
      }
      _frozen = true;
      phone_.Freeze();
    }

    public const int NameFieldNumber = 1;
    private string name_ = "";
    public string Name {
      get { return name_; }
      set {
        pb::Freezable.CheckMutable(this);
        name_ = value ?? "";
      }
    }

    public const int IdFieldNumber = 2;
    private int id_;
    public int Id {
      get { return id_; }
      set {
        pb::Freezable.CheckMutable(this);
        id_ = value;
      }
    }

    public const int EmailFieldNumber = 3;
    private string email_ = "";
    public string Email {
      get { return email_; }
      set {
        pb::Freezable.CheckMutable(this);
        email_ = value ?? "";
      }
    }

    public const int PhoneFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber> _repeated_phone_codec
        = pb::FieldCodec.ForMessage(34, global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber.Parser);
    private readonly pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber> phone_ = new pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber>();
    public pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber> Phone {
      get { return phone_; }
    }

    public override bool Equals(object other) {
      return Equals(other as Person);
    }

    public bool Equals(Person other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Name != other.Name) return false;
      if (Id != other.Id) return false;
      if (Email != other.Email) return false;
      if(!phone_.Equals(other.phone_)) return false;
      return true;
    }

    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Id != 0) hash ^= Id.GetHashCode();
      if (Email.Length != 0) hash ^= Email.GetHashCode();
      hash ^= phone_.GetHashCode();
      return hash;
    }

    public override string ToString() {
      return pb::JsonFormatter.Default.Format(this);
    }

    public void WriteTo(pb::CodedOutputStream output) {
      if (Name.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Name);
      }
      if (Id != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Id);
      }
      if (Email.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Email);
      }
      phone_.WriteTo(output, _repeated_phone_codec);
    }

    public int CalculateSize() {
      int size = 0;
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      if (Email.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Email);
      }
      size += phone_.CalculateSize(_repeated_phone_codec);
      return size;
    }

    public void MergeFrom(Person other) {
      if (other == null) {
        return;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.Email.Length != 0) {
        Email = other.Email;
      }
      phone_.Add(other.phone_);
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while (input.ReadTag(out tag)) {
        switch(tag) {
          case 0:
            throw pb::InvalidProtocolBufferException.InvalidTag();
          default:
            if (pb::WireFormat.IsEndGroupTag(tag)) {
              return;
            }
            break;
          case 10: {
            Name = input.ReadString();
            break;
          }
          case 16: {
            Id = input.ReadInt32();
            break;
          }
          case 26: {
            Email = input.ReadString();
            break;
          }
          case 34: {
            phone_.AddEntriesFrom(input, _repeated_phone_codec);
            break;
          }
        }
      }
    }

    #region Nested types
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public static partial class Types {
      public enum PhoneType {
        MOBILE = 0,
        HOME = 1,
        WORK = 2,
      }

      [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
      public sealed partial class PhoneNumber : pb::IMessage<PhoneNumber> {
        private static readonly pb::MessageParser<PhoneNumber> _parser = new pb::MessageParser<PhoneNumber>(() => new PhoneNumber());
        public static pb::MessageParser<PhoneNumber> Parser { get { return _parser; } }

        private static readonly string[] _fieldNames = new string[] { "number", "type" };
        private static readonly uint[] _fieldTags = new uint[] { 10, 16 };
        public static pbr::MessageDescriptor Descriptor {
          get { return global::Google.Protobuf.Examples.AddressBook.Person.Descriptor.NestedTypes[0]; }
        }

        public pbr::FieldAccessorTable Fields {
          get { return global::Google.Protobuf.Examples.AddressBook.Addressbook.internal__static_tutorial_Person_PhoneNumber__FieldAccessorTable; }
        }

        private bool _frozen = false;
        public bool IsFrozen { get { return _frozen; } }

        public PhoneNumber() {
          OnConstruction();
        }

        partial void OnConstruction();

        public PhoneNumber(PhoneNumber other) : this() {
          number_ = other.number_;
          type_ = other.type_;
        }

        public PhoneNumber Clone() {
          return new PhoneNumber(this);
        }

        public void Freeze() {
          if (IsFrozen) {
            return;
          }
          _frozen = true;
        }

        public const int NumberFieldNumber = 1;
        private string number_ = "";
        public string Number {
          get { return number_; }
          set {
            pb::Freezable.CheckMutable(this);
            number_ = value ?? "";
          }
        }

        public const int TypeFieldNumber = 2;
        private global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType type_ = global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType.MOBILE;
        public global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType Type {
          get { return type_; }
          set {
            pb::Freezable.CheckMutable(this);
            type_ = value;
          }
        }

        public override bool Equals(object other) {
          return Equals(other as PhoneNumber);
        }

        public bool Equals(PhoneNumber other) {
          if (ReferenceEquals(other, null)) {
            return false;
          }
          if (ReferenceEquals(other, this)) {
            return true;
          }
          if (Number != other.Number) return false;
          if (Type != other.Type) return false;
          return true;
        }

        public override int GetHashCode() {
          int hash = 1;
          if (Number.Length != 0) hash ^= Number.GetHashCode();
          if (Type != global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType.MOBILE) hash ^= Type.GetHashCode();
          return hash;
        }

        public override string ToString() {
          return pb::JsonFormatter.Default.Format(this);
        }

        public void WriteTo(pb::CodedOutputStream output) {
          if (Number.Length != 0) {
            output.WriteRawTag(10);
            output.WriteString(Number);
          }
          if (Type != global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType.MOBILE) {
            output.WriteRawTag(16);
            output.WriteEnum((int) Type);
          }
        }

        public int CalculateSize() {
          int size = 0;
          if (Number.Length != 0) {
            size += 1 + pb::CodedOutputStream.ComputeStringSize(Number);
          }
          if (Type != global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType.MOBILE) {
            size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Type);
          }
          return size;
        }

        public void MergeFrom(PhoneNumber other) {
          if (other == null) {
            return;
          }
          if (other.Number.Length != 0) {
            Number = other.Number;
          }
          if (other.Type != global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType.MOBILE) {
            Type = other.Type;
          }
        }

        public void MergeFrom(pb::CodedInputStream input) {
          uint tag;
          while (input.ReadTag(out tag)) {
            switch(tag) {
              case 0:
                throw pb::InvalidProtocolBufferException.InvalidTag();
              default:
                if (pb::WireFormat.IsEndGroupTag(tag)) {
                  return;
                }
                break;
              case 10: {
                Number = input.ReadString();
                break;
              }
              case 16: {
                type_ = (global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType) input.ReadEnum();
                break;
              }
            }
          }
        }

      }

    }
    #endregion

  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public sealed partial class AddressBook : pb::IMessage<AddressBook> {
    private static readonly pb::MessageParser<AddressBook> _parser = new pb::MessageParser<AddressBook>(() => new AddressBook());
    public static pb::MessageParser<AddressBook> Parser { get { return _parser; } }

    private static readonly string[] _fieldNames = new string[] { "person" };
    private static readonly uint[] _fieldTags = new uint[] { 10 };
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Examples.AddressBook.Addressbook.Descriptor.MessageTypes[1]; }
    }

    public pbr::FieldAccessorTable Fields {
      get { return global::Google.Protobuf.Examples.AddressBook.Addressbook.internal__static_tutorial_AddressBook__FieldAccessorTable; }
    }

    private bool _frozen = false;
    public bool IsFrozen { get { return _frozen; } }

    public AddressBook() {
      OnConstruction();
    }

    partial void OnConstruction();

    public AddressBook(AddressBook other) : this() {
      person_ = other.person_.Clone();
    }

    public AddressBook Clone() {
      return new AddressBook(this);
    }

    public void Freeze() {
      if (IsFrozen) {
        return;
      }
      _frozen = true;
      person_.Freeze();
    }

    public const int PersonFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Google.Protobuf.Examples.AddressBook.Person> _repeated_person_codec
        = pb::FieldCodec.ForMessage(10, global::Google.Protobuf.Examples.AddressBook.Person.Parser);
    private readonly pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person> person_ = new pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person>();
    public pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person> Person {
      get { return person_; }
    }

    public override bool Equals(object other) {
      return Equals(other as AddressBook);
    }

    public bool Equals(AddressBook other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!person_.Equals(other.person_)) return false;
      return true;
    }

    public override int GetHashCode() {
      int hash = 1;
      hash ^= person_.GetHashCode();
      return hash;
    }

    public override string ToString() {
      return pb::JsonFormatter.Default.Format(this);
    }

    public void WriteTo(pb::CodedOutputStream output) {
      person_.WriteTo(output, _repeated_person_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += person_.CalculateSize(_repeated_person_codec);
      return size;
    }

    public void MergeFrom(AddressBook other) {
      if (other == null) {
        return;
      }
      person_.Add(other.person_);
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while (input.ReadTag(out tag)) {
        switch(tag) {
          case 0:
            throw pb::InvalidProtocolBufferException.InvalidTag();
          default:
            if (pb::WireFormat.IsEndGroupTag(tag)) {
              return;
            }
            break;
          case 10: {
            person_.AddEntriesFrom(input, _repeated_person_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
