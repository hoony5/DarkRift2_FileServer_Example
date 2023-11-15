[System.Serializable]
[method: JsonConstructor]
public class User(UserHeader header, string password, ushort clientID) : IDarkRiftSerializable
{
    [JsonProperty(nameof(Header))] public UserHeader Header { get; private set; } = header;
    [JsonProperty(nameof(Password))] public string Password { get; private set; } = password;
    [JsonProperty(nameof(ClientID))] public ushort ClientID { get; private set; } = clientID;

    public User()
        : this(
            new UserHeader(),
            PreventExceptionStringValue,
            PreventExceptionNumericValue)
    {
        
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
