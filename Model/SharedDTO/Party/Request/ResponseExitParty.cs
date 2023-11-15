[Serializable]
[method:JsonConstructor]
public class ResponseExitParty(UserHeader departedUser, string partyName, ushort clientID)
    : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(DepartedUser))]public UserHeader DepartedUser { get; private set; } = departedUser;
    [JsonProperty(nameof(PartyName))]public string PartyName { get; private set; } = partyName;

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