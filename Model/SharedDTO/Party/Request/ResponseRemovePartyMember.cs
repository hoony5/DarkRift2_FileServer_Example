[Serializable]
[method: JsonConstructor]
public class ResponseRemovePartyMember(UserHeader removedUserInfo, string partyName, ushort clientID)
    : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(RemovedUserInfo))]public UserHeader RemovedUserInfo { get; set; } 
        = removedUserInfo;
    [JsonProperty(nameof(PartyName))] public string PartyName { get; set; }
        = partyName;

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, RemovedUserInfo);
        CheckValidationString(this, PartyName);
#endif
        e.Writer.Write(RemovedUserInfo);
        e.Writer.Write(PartyName);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        RemovedUserInfo = e.Reader.ReadSerializable<UserHeader>();
        PartyName = e.Reader.ReadString();
    }
}