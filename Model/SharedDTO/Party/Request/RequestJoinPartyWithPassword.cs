[Serializable]
[method: JsonConstructor]
public class RequestJoinPartyWithPassword
    (UserHeader joinedUserInfo, string partyName, string partyPassword, ushort senderClientID) 
    : ServerRequestModelBase(joinedUserInfo.AccountID)
{
    [JsonProperty(nameof(JoinedUserInfo))]public UserHeader JoinedUserInfo { get; private set; } 
        = joinedUserInfo;

    [JsonProperty(nameof(PartyName))] public string PartyName { get; private set; }
        = partyName;
    [JsonProperty(nameof(PartyPassword))]public string PartyPassword { get; private set; } 
        = partyPassword;
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; } 
        = senderClientID;

    public RequestJoinPartyWithPassword()
        : this( 
            new UserHeader(PreventExceptionStringValue), 
            PreventExceptionStringValue, 
            PreventExceptionStringValue, 
            PreventExceptionNumericValue)
    {
        
    }
    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, JoinedUserInfo);
        CheckValidationString(this, PartyName);
        CheckValidationString(this, PartyPassword);
#endif
        e.Writer.Write(JoinedUserInfo);
        e.Writer.Write(PartyName);
        e.Writer.Write(PartyPassword);
        e.Writer.Write(SenderClientID);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        JoinedUserInfo = e.Reader.ReadSerializable<UserHeader>();
        PartyName = e.Reader.ReadString();
        PartyPassword = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}