using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestDeleteAllFiles : ServerRequestModelBase
{
    [JsonProperty(nameof(SenderPartyKey))] public string SenderPartyKey { get; private set; }

    [JsonConstructor]
    public RequestDeleteAllFiles(string senderPartyKey, string id, ushort state, string log) : base(id, state, log)
    {
        SenderPartyKey = senderPartyKey;
    }
    public RequestDeleteAllFiles() : this(StringNullValue, StringNullValue, NumericNullValue, SuccessResponse) { }
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        SenderPartyKey = e.Reader.ReadString();
    }
    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, SenderPartyKey);
#endif
        e.Writer.Write(SenderPartyKey);
    }
}