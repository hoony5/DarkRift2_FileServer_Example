[Serializable]
[method: JsonConstructor]
public class RequestRemoveMember
    (UserHeader removedUserInfo, string partyName, ushort senderClientID) 
    : ServerRequestModelBase(removedUserInfo.AccountID)
{
    [JsonProperty(nameof(RemovedUserInfo))]public UserHeader RemovedUserInfo { get; private set; } 
        = removedUserInfo;
    [JsonProperty(nameof(PartyName))] public string PartyName { get; private set; }
        = partyName;
    [JsonProperty(nameof(SenderClientID))] public ushort SenderClientID { get; private set; } 
        = senderClientID;

    public RequestRemoveMember()
        : this( 
            new UserHeader(StringNullValue), 
            StringNullValue, 
            NumericNullValue)
    {
        
    }
    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, RemovedUserInfo);
        CheckValidationString(this, PartyName);
#endif
        e.Writer.Write(RemovedUserInfo);
        e.Writer.Write(PartyName);
        e.Writer.Write(SenderClientID);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        RemovedUserInfo = e.Reader.ReadSerializable<UserHeader>();
        PartyName = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}