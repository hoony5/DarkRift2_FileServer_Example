[Serializable]
[method:JsonConstructor]
public class RequestCreateParty
  (User leader,
    string partyName,
    ushort maxPartyPlayers,
    bool isPublic,
    ushort senderClientID)
  : ServerRequestModelBase(leader.Header.AccountID)
{
  [JsonProperty(nameof(Leader))] public User Leader { get; private set; } = leader;
  [JsonProperty(nameof(PartyName))] public string PartyName { get; private set; } = partyName;
  [JsonProperty(nameof(MaxPartyPlayers))] public ushort MaxPartyPlayers { get; private set; } = maxPartyPlayers;
  [JsonProperty(nameof(IsPublic))] public bool IsPublic { get; private set; } = isPublic;
  [JsonProperty(nameof(SenderClientID))] public ushort SenderClientID { get; set; } = senderClientID;

  public override void Serialize(SerializeEvent e)
  {
    base.Serialize(e);
#if DEBUG
     CheckValidationProperty(this, Leader);
     CheckValidationString(this, PartyName);
#endif
    e.Writer.Write(Leader);
    e.Writer.Write(PartyName);
    e.Writer.Write(MaxPartyPlayers);
    e.Writer.Write(IsPublic);
    e.Writer.Write(SenderClientID);
  }

  public override void Deserialize(DeserializeEvent e)
  {
    base.Deserialize(e);
    Leader = e.Reader.ReadSerializable<User>();
    PartyName = e.Reader.ReadString();
    MaxPartyPlayers = e.Reader.ReadUInt16();
    IsPublic = e.Reader.ReadBoolean();
    SenderClientID = e.Reader.ReadUInt16();
  }
}