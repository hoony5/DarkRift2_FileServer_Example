[Serializable]
[method: JsonConstructor]
public class ResponseDestroyParty(ushort clientID) : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(PartyName))] public string PartyName { get; set; }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, PartyName);
#endif
        e.Writer.Write(PartyName);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        PartyName = e.Reader.ReadString();
    }
}