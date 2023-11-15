[Serializable]
[method:JsonConstructor]
public class ResponseCreateParty(Party party, ushort clientID)
    : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(CreatedParty))] public Party CreatedParty { get; private set; } = party;

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
        e.Writer.Write(CreatedParty);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
#if DEBUG
         CheckValidationProperty(this, CreatedParty);
#endif
        CreatedParty = e.Reader.ReadSerializable<Party>();
    }
}