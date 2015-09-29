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
  /// <summary>Holder for reflection information generated from addressbook.proto</summary>
  public static partial class Addressbook {

    #region Descriptor
    /// <summary>File descriptor for addressbook.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static Addressbook() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChFhZGRyZXNzYm9vay5wcm90bxIIdHV0b3JpYWwi1QEKBlBlcnNvbhIMCgRu",
            "YW1lGAEgASgJEgoKAmlkGAIgASgFEg0KBWVtYWlsGAMgASgJEiwKBnBob25l",
            "cxgEIAMoCzIcLnR1dG9yaWFsLlBlcnNvbi5QaG9uZU51bWJlchpHCgtQaG9u",
            "ZU51bWJlchIOCgZudW1iZXIYASABKAkSKAoEdHlwZRgCIAEoDjIaLnR1dG9y",
            "aWFsLlBlcnNvbi5QaG9uZVR5cGUiKwoJUGhvbmVUeXBlEgoKBk1PQklMRRAA",
            "EggKBEhPTUUQARIICgRXT1JLEAIiLwoLQWRkcmVzc0Jvb2sSIAoGcGVvcGxl",
            "GAEgAygLMhAudHV0b3JpYWwuUGVyc29uQlAKFGNvbS5leGFtcGxlLnR1dG9y",
            "aWFsQhFBZGRyZXNzQm9va1Byb3Rvc6oCJEdvb2dsZS5Qcm90b2J1Zi5FeGFt",
            "cGxlcy5BZGRyZXNzQm9va2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.InternalBuildGeneratedFileFrom(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedCodeInfo(null, new pbr::GeneratedCodeInfo[] {
            new pbr::GeneratedCodeInfo(typeof(global::Google.Protobuf.Examples.AddressBook.Person), new[]{ "Name", "Id", "Email", "Phones" }, null, new[]{ typeof(global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType) }, new pbr::GeneratedCodeInfo[] { new pbr::GeneratedCodeInfo(typeof(global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber), new[]{ "Number", "Type" }, null, null, null)}),
            new pbr::GeneratedCodeInfo(typeof(global::Google.Protobuf.Examples.AddressBook.AddressBook), new[]{ "People" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  public sealed partial class Person : pb::IMessage<Person> {
    private static readonly pb::MessageParser<Person> _parser = new pb::MessageParser<Person>(() => new Person());
    public static pb::MessageParser<Person> Parser { get { return _parser; } }

    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Examples.AddressBook.Addressbook.Descriptor.MessageTypes[0]; }
    }

    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    public Person() {
      OnConstruction();
    }

    partial void OnConstruction();

    public Person(Person other) : this() {
      name_ = other.name_;
      id_ = other.id_;
      email_ = other.email_;
      phones_ = other.phones_.Clone();
    }

    public Person Clone() {
      return new Person(this);
    }

    public const int NameFieldNumber = 1;
    private string name_ = "";
    public string Name {
      get { return name_; }
      set {
        name_ = pb::Preconditions.CheckNotNull(value, "value");
      }
    }

    public const int IdFieldNumber = 2;
    private int id_;
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    public const int EmailFieldNumber = 3;
    private string email_ = "";
    public string Email {
      get { return email_; }
      set {
        email_ = pb::Preconditions.CheckNotNull(value, "value");
      }
    }

    public const int PhonesFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber> _repeated_phones_codec
        = pb::FieldCodec.ForMessage(34, global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber.Parser);
    private readonly pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber> phones_ = new pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber>();
    public pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneNumber> Phones {
      get { return phones_; }
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
      if(!phones_.Equals(other.phones_)) return false;
      return true;
    }

    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Id != 0) hash ^= Id.GetHashCode();
      if (Email.Length != 0) hash ^= Email.GetHashCode();
      hash ^= phones_.GetHashCode();
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
      phones_.WriteTo(output, _repeated_phones_codec);
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
      size += phones_.CalculateSize(_repeated_phones_codec);
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
      phones_.Add(other.phones_);
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
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
            phones_.AddEntriesFrom(input, _repeated_phones_codec);
            break;
          }
        }
      }
    }

    #region Nested types
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    /// <summary>Container for nested types declared in the Person message type.</summary>
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

        public static pbr::MessageDescriptor Descriptor {
          get { return global::Google.Protobuf.Examples.AddressBook.Person.Descriptor.NestedTypes[0]; }
        }

        pbr::MessageDescriptor pb::IMessage.Descriptor {
          get { return Descriptor; }
        }

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

        public const int NumberFieldNumber = 1;
        private string number_ = "";
        public string Number {
          get { return number_; }
          set {
            number_ = pb::Preconditions.CheckNotNull(value, "value");
          }
        }

        public const int TypeFieldNumber = 2;
        private global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType type_ = global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType.MOBILE;
        public global::Google.Protobuf.Examples.AddressBook.Person.Types.PhoneType Type {
          get { return type_; }
          set {
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
          while ((tag = input.ReadTag()) != 0) {
            switch(tag) {
              default:
                input.SkipLastField();
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

    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Examples.AddressBook.Addressbook.Descriptor.MessageTypes[1]; }
    }

    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    public AddressBook() {
      OnConstruction();
    }

    partial void OnConstruction();

    public AddressBook(AddressBook other) : this() {
      people_ = other.people_.Clone();
    }

    public AddressBook Clone() {
      return new AddressBook(this);
    }

    public const int PeopleFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Google.Protobuf.Examples.AddressBook.Person> _repeated_people_codec
        = pb::FieldCodec.ForMessage(10, global::Google.Protobuf.Examples.AddressBook.Person.Parser);
    private readonly pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person> people_ = new pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person>();
    public pbc::RepeatedField<global::Google.Protobuf.Examples.AddressBook.Person> People {
      get { return people_; }
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
      if(!people_.Equals(other.people_)) return false;
      return true;
    }

    public override int GetHashCode() {
      int hash = 1;
      hash ^= people_.GetHashCode();
      return hash;
    }

    public override string ToString() {
      return pb::JsonFormatter.Default.Format(this);
    }

    public void WriteTo(pb::CodedOutputStream output) {
      people_.WriteTo(output, _repeated_people_codec);
    }

    public int CalculateSize() {
      int size = 0;
      size += people_.CalculateSize(_repeated_people_codec);
      return size;
    }

    public void MergeFrom(AddressBook other) {
      if (other == null) {
        return;
      }
      people_.Add(other.people_);
    }

    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            people_.AddEntriesFrom(input, _repeated_people_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
