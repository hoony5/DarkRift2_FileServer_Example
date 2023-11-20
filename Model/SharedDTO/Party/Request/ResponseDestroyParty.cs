using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class ResponseDestroyParty : ServerResponseModelBase
{
    [JsonProperty(nameof(PartyName))] public string PartyName { get; set; }
    [JsonConstructor]
    public ResponseDestroyParty(string partyName, ushort clientID, ushort state, string log) : base(clientID, state, log)
    {
        PartyName = partyName;
    }
    public ResponseDestroyParty() : this(StringNullValue, NumericNullValue, NumericNullValue, SuccessResponse) { }
    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, PartyName);
#endif
        e.Writer.Write(PartyName);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        PartyName = e.Reader.ReadString();
    }
}