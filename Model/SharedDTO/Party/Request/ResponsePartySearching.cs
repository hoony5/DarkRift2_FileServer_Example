[Serializable]
[method:JsonConstructor]
public class ResponsePartySearching(ushort clientID) : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(SearchingPartyArray))] public Party[] SearchingPartyArray { get; set; } 

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, SearchingPartyArray);
#endif
        e.Writer.Write(SearchingPartyArray);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        SearchingPartyArray = e.Reader.ReadSerializables<Party>();
    }
}