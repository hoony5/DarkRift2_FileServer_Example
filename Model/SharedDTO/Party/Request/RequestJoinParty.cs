[Serializable]
[method:JsonConstructor]
public class RequestJoinParty
    (UserHeader joinedUserInfo, string partyName, ushort senderClientID)
    : ServerRequestModelBase(joinedUserInfo.AccountID)
{
    [JsonProperty(nameof(JoinedUserInfo))]public UserHeader JoinedUserInfo { get; private set; }
        = joinedUserInfo;
    [JsonProperty(nameof(PartyName))]public string PartyName { get; private set; } 
        = partyName;
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; } 
        = senderClientID;

    public RequestJoinParty()
        : this(
            new UserHeader(StringNullValue),
            StringNullValue,
            NumericNullValue
        )
    {
        
    }
    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, JoinedUserInfo);
        CheckValidationString(this, PartyName);
#endif
        e.Writer.Write(JoinedUserInfo);
        e.Writer.Write(PartyName);
        e.Writer.Write(SenderClientID);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        JoinedUserInfo = e.Reader.ReadSerializable<UserHeader>();
        PartyName = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}