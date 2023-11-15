[Serializable]
[method:JsonConstructor]
public class RequestExitParty(UserHeader departedUser, string partyName)
    : ServerRequestModelBase(departedUser.AccountID)
{
    [JsonProperty(nameof(DepartedUser))]public UserHeader DepartedUser { get; private set; } = departedUser;
    [JsonProperty(nameof(PartyName))]public string PartyName { get; private set; } = partyName;

    public ushort SenderClientID { get; set; }

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