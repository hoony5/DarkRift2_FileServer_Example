[Serializable]
[method: JsonConstructor]
public class ResponseJoinPartyWithPassword
    (UserHeader joinedUserInfo, Party joinedParty, ushort clientID) 
    : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(JoinedUserInfo))] public UserHeader JoinedUserInfo { get; private set; } = joinedUserInfo;
    [JsonProperty(nameof(JoinedParty))] public Party JoinedParty { get; private set; } = joinedParty;

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, JoinedUserInfo);
        CheckValidationProperty(this, JoinedParty);
#endif
        e.Writer.Write(JoinedUserInfo);
        e.Writer.Write(JoinedParty);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        JoinedUserInfo = e.Reader.ReadSerializable<UserHeader>();
        JoinedParty = e.Reader.ReadSerializable<Party>();
    }
}