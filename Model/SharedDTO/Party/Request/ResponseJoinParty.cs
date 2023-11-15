[Serializable]
[method: JsonConstructor]
public class ResponseJoinParty(ushort clientID) : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(JoinedUserInfo))] public UserHeader JoinedUserInfo { get; set; }
    [JsonProperty(nameof(JoinedParty))] public Party JoinedParty { get; set; }

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