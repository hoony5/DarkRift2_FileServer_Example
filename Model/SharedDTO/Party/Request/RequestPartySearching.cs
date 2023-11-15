[Serializable]
[method:JsonConstructor]
public class RequestPartySearching(ushort filterType, string searchResult, ushort senderClientID, string id)
    : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(FilterType))]public ushort FilterType { get; private set; } = filterType;
    [JsonProperty(nameof(SearchResult))]public string SearchResult { get; private set; } = searchResult;
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; } = senderClientID;

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, SearchResult);
#endif
        e.Writer.Write(FilterType);
        e.Writer.Write(SearchResult);
        e.Writer.Write(SenderClientID);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        FilterType = e.Reader.ReadUInt16();
        SearchResult = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}