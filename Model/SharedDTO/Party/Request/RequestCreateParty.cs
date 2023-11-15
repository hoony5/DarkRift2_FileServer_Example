[Serializable]
[method:JsonConstructor]
public class RequestCreateParty
  (User leader,
    string partyName,
    string password,
    ushort maxPartyPlayers,
    bool isPublic,
    ushort senderClientID)
  : ServerRequestModelBase(leader.Header.AccountID)
{
  [JsonProperty(nameof(Leader))] public User Leader { get; private set; } = leader;
  [JsonProperty(nameof(PartyName))] public string PartyName { get; private set; } = partyName;
  [JsonProperty(nameof(Password))] public string Password { get; private set; } = password;
  [JsonProperty(nameof(MaxPartyPlayers))] public ushort MaxPartyPlayers { get; private set; } = maxPartyPlayers;
  [JsonProperty(nameof(IsPublic))] public bool IsPublic { get; private set; } = isPublic;
  [JsonProperty(nameof(SenderClientID))] public ushort SenderClientID { get; private set; } = senderClientID;

  public RequestCreateParty()
    : this(
      new User(new UserHeader(PreventExceptionStringValue), PreventExceptionStringValue, PreventExceptionNumericValue),
      PreventExceptionStringValue,
      PreventExceptionStringValue,
      PreventExceptionNumericValue,
      false,
      PreventExceptionNumericValue)
  {
    
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