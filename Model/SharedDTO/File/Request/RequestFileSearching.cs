using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestFileSearching : ServerRequestModelBase
{
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; }
    [JsonProperty(nameof(SenderPartyKey))]public string SenderPartyKey { get; private set; }

    [JsonConstructor]
    public RequestFileSearching(ushort senderClientID, string senderPartyKey, string id, ushort state, string log)
        : base(id,state,log)
    {
        SenderClientID = senderClientID;
        SenderPartyKey = senderPartyKey;
    }
    public RequestFileSearching() : this(NumericNullValue, StringNullValue, StringNullValue, NumericNullValue, SuccessResponse) { }
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        SenderClientID = e.Reader.ReadUInt16();
        SenderPartyKey = e.Reader.ReadString();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, SenderPartyKey);
#endif
        e.Writer.Write(SenderClientID);
        e.Writer.Write(SenderPartyKey);
    }
}