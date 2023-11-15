[Serializable]
[method:JsonConstructor]
public class RequestPartySearching(string keyword, ushort senderClientID, string id)
    : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(Keyword))]public string Keyword { get; private set; } = keyword;
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; } = senderClientID;

    public RequestPartySearching()
        : this(
            PreventExceptionStringValue,
            PreventExceptionNumericValue,
            PreventExceptionStringValue)
    {
        
    }
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