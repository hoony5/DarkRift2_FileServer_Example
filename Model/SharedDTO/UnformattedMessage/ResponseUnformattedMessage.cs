using Newtonsoft.Json;
using DarkRift;
using static DtoValidator;
using static SharedValue;

public class ResponseUnformattedMessage : ServerResponseModelBase
{
    [JsonProperty(nameof(AccountID))] public string AccountID { get; set; }

    [JsonConstructor]
    public ResponseUnformattedMessage(string accountID, ushort clientID, ushort state,string log) : base(clientID, state, log)
    {
        AccountID = accountID;
    }

    public ResponseUnformattedMessage() : this(StringNullValue, NumericNullValue, NumericNullValue, StringNullValue)
    {
    }
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        AccountID = e.Reader.ReadString();
    }

    public override void Serialize(SerializeEvent e)
    {
#if DEBUG
        CheckValidationString(this, AccountID);
#endif
        base.Serialize(e);
        e.Writer.Write(AccountID);
    }
}