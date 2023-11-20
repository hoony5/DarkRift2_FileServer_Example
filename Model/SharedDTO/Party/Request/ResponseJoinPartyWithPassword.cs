using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class ResponseJoinPartyWithPassword : ServerResponseModelBase
{
    [JsonProperty(nameof(JoinedUserInfo))] public UserHeader JoinedUserInfo { get; set; }
    [JsonProperty(nameof(JoinedParty))] public Party JoinedParty { get; set; }

    [ JsonConstructor]
    public ResponseJoinPartyWithPassword(UserHeader joinedUserInfo, Party joinedParty, ushort clientID,
        ushort state, string log) : base(clientID, state, log) { }
    public ResponseJoinPartyWithPassword() : this(new UserHeader(), new Party(), NumericNullValue,
        NumericNullValue, StringNullValue) { }
    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, JoinedUserInfo);
        CheckValidationProperty(this, JoinedParty);
#endif
        e.Writer.Write(JoinedUserInfo);
        e.Writer.Write(JoinedParty);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        JoinedUserInfo = e.Reader.ReadSerializable<UserHeader>();
        JoinedParty = e.Reader.ReadSerializable<Party>();
    }
}