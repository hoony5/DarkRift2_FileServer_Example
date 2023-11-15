[Serializable]
[method: JsonConstructor]
public class RequestJoinPartyWithPassword
    (UserHeader joinedUserInfo, string partyPassword, ushort senderClientID, string id) 
    : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(JoinedUserInfo))]public UserHeader JoinedUserInfo { get; private set; } 
        = joinedUserInfo;
    [JsonProperty(nameof(PartyPassword))]public string PartyPassword { get; private set; } 
        = partyPassword;
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; } 
        = senderClientID;

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, JoinedUserInfo);
        CheckValidationString(this, PartyPassword);
#endif
        e.Writer.Write(JoinedUserInfo);
        e.Writer.Write(PartyPassword);
        e.Writer.Write(SenderClientID);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        JoinedUserInfo = e.Reader.ReadSerializable<UserHeader>();
        PartyPassword = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}