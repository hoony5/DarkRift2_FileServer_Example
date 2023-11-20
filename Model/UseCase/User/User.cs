using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[System.Serializable]
public class User : IDarkRiftSerializable
{
    [JsonProperty(nameof(Header))] public UserHeader Header { get; private set; }
    [JsonProperty(nameof(Password))] public string Password { get; private set; }
    [JsonProperty(nameof(ClientID))] public ushort ClientID { get; private set; }

    public User()
        : this(
            new UserHeader(),
            StringNullValue,
            NumericNullValue)
    {
        
    }

    [JsonConstructor]
    public User(UserHeader header, string password, ushort clientID)
    {
        Header = header;
        Password = password;
        ClientID = clientID;
    }

    public void Deserialize(DeserializeEvent e)
    {
        Header = e.Reader.ReadSerializable<UserHeader>();
        Password = e.Reader.ReadString();
        ClientID = e.Reader.ReadUInt16();    
    }

    public void Serialize(SerializeEvent e)
    {
#if DEBUG
        CheckValidationProperty(this, Header);
        CheckValidationString(this, Password);
#endif
        e.Writer.Write(Header);
        e.Writer.Write(Password);
        e.Writer.Write(ClientID);
    }
}
