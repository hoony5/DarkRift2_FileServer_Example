[Serializable]
[method:JsonConstructor]
public class RequestFileSearching(ushort senderClientID, string senderPartyKey, string id) 
    : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; } = senderClientID;
    [JsonProperty(nameof(SenderPartyKey))]public string SenderPartyKey { get; private set; } = senderPartyKey;

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        SenderClientID = e.Reader.ReadUInt16();
        SenderPartyKey = e.Reader.ReadString();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, SenderPartyKey);
#endif
        e.Writer.Write(SenderClientID);
        e.Writer.Write(SenderPartyKey);
    }
}