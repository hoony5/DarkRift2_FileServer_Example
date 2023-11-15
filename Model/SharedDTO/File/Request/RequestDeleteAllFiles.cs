[Serializable]
[method:JsonConstructor]
public class RequestDeleteAllFiles(string senderPartyKey, string id) : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(SenderPartyKey))] public string SenderPartyKey { get; private set; } = senderPartyKey;

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        SenderPartyKey = e.Reader.ReadString();
    }
    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, SenderPartyKey);
#endif
        e.Writer.Write(SenderPartyKey);
    }
}