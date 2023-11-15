[Serializable]
[method:JsonConstructor]
public class ResponseDownloadFile(ushort clientID, FileSegment segment) : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(Segment))] public FileSegment Segment { get; private set; } = segment;
    
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        Segment = e.Reader.ReadSerializable<FileSegment>();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, Segment);
#endif
        e.Writer.Write(Segment);
    }
}