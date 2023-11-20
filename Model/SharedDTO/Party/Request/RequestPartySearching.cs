using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestPartySearching : ServerRequestModelBase
{
    [JsonProperty(nameof(Keyword))]public string Keyword { get; private set; }
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; }

    public RequestPartySearching() : this(StringNullValue, NumericNullValue, StringNullValue,
        NumericNullValue, SuccessResponse) { }

    [JsonConstructor]
    public RequestPartySearching(string keyword, ushort senderClientID, string id, ushort state, string log)
        : base(id, state, log)
    {
        Keyword = keyword;
        SenderClientID = senderClientID;
    }

    public static RequestPartySearching Instance { get; } = new();

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, Keyword);
#endif
        e.Writer.Write(Keyword);
        e.Writer.Write(SenderClientID);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        Keyword = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}