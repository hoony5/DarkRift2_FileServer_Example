[Serializable]
public class RequestRemoveMember
    (UserHeader removedUserInfo, string partyKey, ushort senderClientID, string id) 
    : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(RemovedUserInfo))]public UserHeader RemovedUserInfo { get; private set; } 
        = removedUserInfo;
    [JsonProperty(nameof(PartyKey))] public string PartyKey { get; private set; }
        = partyKey;
    [JsonProperty(nameof(SenderClientID))] public ushort SenderClientID { get; private set; } 
        = senderClientID;

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, RemovedUserInfo);
        CheckValidationString(this, PartyKey);
#endif
        e.Writer.Write(RemovedUserInfo);
        e.Writer.Write(PartyKey);
        e.Writer.Write(SenderClientID);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        RemovedUserInfo = e.Reader.ReadSerializable<UserHeader>();
        PartyKey = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}