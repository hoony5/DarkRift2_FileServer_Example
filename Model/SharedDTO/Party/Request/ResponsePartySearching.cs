using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class ResponsePartySearching : ServerResponseModelBase
{
    [JsonProperty(nameof(SearchingPartyArray))] public Party[] SearchingPartyArray { get; set; }

    [JsonConstructor]
    public ResponsePartySearching(Party[] searchingPartyArray, ushort clientID, ushort state, string log)
        : base(clientID, state, log)
    {
        SearchingPartyArray = searchingPartyArray;
    }
    public ResponsePartySearching() : this(new []{new Party()}, NumericNullValue, NumericNullValue, StringNullValue) { }

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