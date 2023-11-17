[Serializable]
[method:JsonConstructor]
public class RequestUploadFile(string id, string partyKey, FileSegment segment) : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(AccountID))] public string AccountID { get; private set; } = id;
    [JsonProperty(nameof(PartyKey))] public string PartyKey { get; private set; } = partyKey;
    [JsonProperty(nameof(Segment))] public FileSegment Segment { get; private set; } = segment;

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        AccountID = e.Reader.ReadString();
        PartyKey = e.Reader.ReadString();
        Segment = e.Reader.ReadSerializable<FileSegment>();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, Segment);
#endif
        e.Writer.Write(AccountID);
        e.Writer.Write(PartyKey);
        e.Writer.Write(Segment);
    }
}