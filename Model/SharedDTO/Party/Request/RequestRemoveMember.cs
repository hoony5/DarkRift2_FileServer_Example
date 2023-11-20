using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestRemoveMember : ServerRequestModelBase
{
    [JsonProperty(nameof(RemovedUserInfo))]public UserHeader RemovedUserInfo { get; private set; }
    [JsonProperty(nameof(PartyName))] public string PartyName { get; private set; }
    [JsonProperty(nameof(SenderClientID))] public ushort SenderClientID { get; private set; }

    public RequestRemoveMember() : this(new UserHeader(StringNullValue), StringNullValue, NumericNullValue,
        NumericNullValue, StringNullValue) { }

    [JsonConstructor]
    public RequestRemoveMember(UserHeader removedUserInfo, string partyName, ushort senderClientID,
        ushort state, string log) : base(removedUserInfo.AccountID, state, log)
    {
        RemovedUserInfo = removedUserInfo;
        PartyName = partyName;
        SenderClientID = senderClientID;
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