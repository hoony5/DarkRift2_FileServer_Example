using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class ResponseExitParty : ServerResponseModelBase
{
    [method:JsonConstructor]
    public ResponseExitParty(ushort clientID, ushort state, string log) : base(clientID, state, log) { }
    public ResponseExitParty() : this(NumericNullValue, NumericNullValue, StringNullValue) { }
    [JsonProperty(nameof(DepartedUser))]public UserHeader DepartedUser { get; set; }
    [JsonProperty(nameof(PartyName))]public string PartyName { get; set; }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, DepartedUser);
        CheckValidationString(this, PartyName);
#endif
        e.Writer.Write(DepartedUser);
        e.Writer.Write(PartyName);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        DepartedUser = e.Reader.ReadSerializable<UserHeader>();
        PartyName = e.Reader.ReadString();
    }
}