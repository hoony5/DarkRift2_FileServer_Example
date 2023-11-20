using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class ResponseCreateParty : ServerResponseModelBase
{
    [JsonProperty(nameof(CreatedParty))] public Party CreatedParty { get; private set; }
    [JsonConstructor]
    public ResponseCreateParty(Party party, ushort clientID, ushort state, string log) : base(clientID, state, log)
    {
        CreatedParty = party;
    }
    public ResponseCreateParty() : this(new Party(), NumericNullValue, NumericNullValue, StringNullValue) { }
    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
        e.Writer.Write(CreatedParty);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
#if DEBUG
         CheckValidationProperty(this, CreatedParty);
#endif
        CreatedParty = e.Reader.ReadSerializable<Party>();
    }
}