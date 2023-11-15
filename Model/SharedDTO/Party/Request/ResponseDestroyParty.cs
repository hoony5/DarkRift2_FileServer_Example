[Serializable]
[method: JsonConstructor]
public class ResponseDestroyParty(string partyName, ushort clientID) : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(PartyName))] public string PartyName { get; private set; } = partyName;

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