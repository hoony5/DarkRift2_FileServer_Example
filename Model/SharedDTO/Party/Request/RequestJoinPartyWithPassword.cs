using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestJoinPartyWithPassword : ServerRequestModelBase
{
    [JsonProperty(nameof(JoinedUserInfo))]public UserHeader JoinedUserInfo { get; private set; }

    [JsonProperty(nameof(PartyName))] public string PartyName { get; private set; }
    [JsonProperty(nameof(PartyPassword))]public string PartyPassword { get; private set; }
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; }

    public RequestJoinPartyWithPassword() : this(new UserHeader(StringNullValue), StringNullValue,
            StringNullValue, NumericNullValue, NumericNullValue, SuccessResponse) { }

    [JsonConstructor]
    public RequestJoinPartyWithPassword(UserHeader joinedUserInfo, string partyName, string partyPassword,
        ushort senderClientID, ushort state, string log) : base(joinedUserInfo.AccountID, state, log)
    {
        JoinedUserInfo = joinedUserInfo;
        PartyName = partyName;
        PartyPassword = partyPassword;
        SenderClientID = senderClientID;
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