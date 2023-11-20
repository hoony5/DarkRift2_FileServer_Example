using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class ResponseRemoveMember : ServerResponseModelBase
{
    [JsonProperty(nameof(RemovedUserInfo))]public UserHeader RemovedUserInfo { get; set; }
    [JsonProperty(nameof(PartyName))] public string PartyName { get; set; }

    [JsonConstructor]
    public ResponseRemoveMember(UserHeader removedUserInfo, string partyName, ushort clientID, ushort state, string log)
        : base(clientID, state, log)
    {
        RemovedUserInfo = removedUserInfo;
        PartyName = partyName;
    }

    public ResponseRemoveMember() : this(new UserHeader(), StringNullValue, NumericNullValue,
        NumericNullValue, StringNullValue) { }

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