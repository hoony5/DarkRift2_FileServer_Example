using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestJoinParty : ServerRequestModelBase
{
    [JsonProperty(nameof(JoinedUserInfo))]public UserHeader JoinedUserInfo { get; private set; }
    [JsonProperty(nameof(PartyName))]public string PartyName { get; private set; }
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; }

    public RequestJoinParty() : this(new UserHeader(StringNullValue), StringNullValue,
        NumericNullValue, NumericNullValue, SuccessResponse) { }

    [JsonConstructor]
    public RequestJoinParty(UserHeader joinedUserInfo, string partyName, ushort senderClientID, ushort state, string log)
        : base(joinedUserInfo.AccountID,state, log)
    {
        JoinedUserInfo = joinedUserInfo;
        PartyName = partyName;
        SenderClientID = senderClientID;
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