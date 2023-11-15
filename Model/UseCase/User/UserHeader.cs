[System.Serializable]
[method: JsonConstructor]
public class UserHeader(string accountID) : IDarkRiftSerializable
{
    [JsonProperty(nameof(AccountID))]public string AccountID { get; private set; } = accountID;
    [JsonProperty(nameof(NickName))]public string NickName { get; set; } = PreventExceptionStringValue;
    [JsonProperty(nameof(PartyKey))]public string PartyKey { get; set; } = PreventExceptionStringValue;
    [JsonProperty(nameof(ConnectionState))]public ushort ConnectionState { get; set; }

    public UserHeader() : this(PreventExceptionStringValue)
    {
        
    }

    public void Deserialize(DeserializeEvent e)
    {
        AccountID = e.Reader.ReadString();
        NickName = e.Reader.ReadString();
        PartyKey = e.Reader.ReadString();
        ConnectionState = e.Reader.ReadUInt16();
    }

    public void Serialize(SerializeEvent e)
    {
#if DEBUG
        CheckValidationString(this, AccountID);
        CheckValidationString(this, NickName);
        CheckValidationString(this, PartyKey);
#endif
        e.Writer.Write(AccountID);
        e.Writer.Write(NickName);
        e.Writer.Write(PartyKey);
        e.Writer.Write(ConnectionState);
    }
}
