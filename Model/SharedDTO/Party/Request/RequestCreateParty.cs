using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestCreateParty : ServerRequestModelBase
{
  [JsonProperty(nameof(Leader))] public User Leader { get; private set; }
  [JsonProperty(nameof(PartyName))] public string PartyName { get; private set; }
  [JsonProperty(nameof(Password))] public string Password { get; private set; }
  [JsonProperty(nameof(MaxPartyPlayers))] public ushort MaxPartyPlayers { get; private set; }
  [JsonProperty(nameof(IsPublic))] public bool IsPublic { get; private set; }
  [JsonProperty(nameof(SenderClientID))] public ushort SenderClientID { get; private set; }

  public RequestCreateParty() : this(new User(new UserHeader(StringNullValue), StringNullValue, NumericNullValue),
    StringNullValue, StringNullValue, NumericNullValue, false, NumericNullValue,
    NumericNullValue, StringNullValue) { }

  [JsonConstructor]
  public RequestCreateParty(User leader,
    string partyName,
    string password,
    ushort maxPartyPlayers,
    bool isPublic,
    ushort senderClientID,
    ushort state,
    string log) : base(leader.Header.AccountID, state, log)
  {
    Leader = leader;
    PartyName = partyName;
    Password = password;
    MaxPartyPlayers = maxPartyPlayers;
    IsPublic = isPublic;
    SenderClientID = senderClientID;
  }

  public override void Serialize(SerializeEvent e)
  {
    base.Serialize(e);
#if DEBUG
     CheckValidationProperty(this, Leader);
     CheckValidationString(this, PartyName);
     CheckValidationString(this, Password);
#endif
    e.Writer.Write(Leader);
    e.Writer.Write(PartyName);
    e.Writer.Write(Password);
    e.Writer.Write(MaxPartyPlayers);
    e.Writer.Write(IsPublic);
    e.Writer.Write(SenderClientID);
  }

  public override void Deserialize(DeserializeEvent e)
  {
    base.Deserialize(e);
    Leader = e.Reader.ReadSerializable<User>();
    PartyName = e.Reader.ReadString();
    Password = e.Reader.ReadString();
    MaxPartyPlayers = e.Reader.ReadUInt16();
    IsPublic = e.Reader.ReadBoolean();
    SenderClientID = e.Reader.ReadUInt16();
  }
}