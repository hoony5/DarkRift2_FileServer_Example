using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestExitParty : ServerRequestModelBase
{
    [JsonProperty(nameof(DepartedUser))]public UserHeader DepartedUser { get; private set; }
    [JsonProperty(nameof(PartyName))]public string PartyName { get; private set; }
    [JsonProperty(nameof(SenderClientID))] public ushort SenderClientID { get; private set; }

    public RequestExitParty() : this(new UserHeader(StringNullValue), StringNullValue,
        NumericNullValue, NumericNullValue, StringNullValue) { }

    [JsonConstructor]
    public RequestExitParty(UserHeader departedUser, string partyName, ushort senderClientID, ushort state, string log)
        : base(departedUser.AccountID, state, log)
    {
        DepartedUser = departedUser;
        PartyName = partyName;
        SenderClientID = senderClientID;
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, DepartedUser);
        CheckValidationString(this, PartyName);
#endif
        e.Writer.Write(DepartedUser);
        e.Writer.Write(PartyName);
        e.Writer.Write(SenderClientID);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        DepartedUser = e.Reader.ReadSerializable<UserHeader>();
        PartyName = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}