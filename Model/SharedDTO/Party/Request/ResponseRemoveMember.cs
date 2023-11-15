[Serializable]
[method: JsonConstructor]
public class ResponseRemoveMember(ushort clientID) : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(RemovedUserInfo))]public UserHeader RemovedUserInfo { get; set; }
    [JsonProperty(nameof(PartyName))] public string PartyName { get; set; }

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