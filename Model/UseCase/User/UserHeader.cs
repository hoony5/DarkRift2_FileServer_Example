using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[System.Serializable]
public class UserHeader : IDarkRiftSerializable
{
    [JsonProperty(nameof(AccountID))]public string AccountID { get; private set; }
    [JsonProperty(nameof(NickName))]public string NickName { get; set; }
    [JsonProperty(nameof(PartyKey))]public string PartyKey { get; set; }
    [JsonProperty(nameof(ConnectionState))]public ushort ConnectionState { get; set; }

    public UserHeader() : this(StringNullValue)
    {
         NickName = StringNullValue;
         PartyKey = StringNullValue;
    }

    [JsonConstructor]
    public UserHeader(string accountID)
    {
        AccountID = accountID;
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
