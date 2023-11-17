[Serializable]
[method: JsonConstructor]
public class ResponseDownloadAccept(
        string fileNameWithoutExtension,
        string fileExtension,
        int lastSegment,
        int lastSegmentLength,
        ushort clientID)
    : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; } = fileExtension;
    [JsonProperty(nameof(LastSegmentIndex))] public int LastSegmentIndex { get; private set; } = lastSegment;
    [JsonProperty(nameof(LastSegmentLength))] public int LastSegmentLength { get; private set; } = lastSegmentLength;

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileNameWithoutExtension = e.Reader.ReadString();
        FileExtension = e.Reader.ReadString();
        LastSegmentIndex = e.Reader.ReadInt32();
        LastSegmentLength = e.Reader.ReadInt32();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, FileNameWithoutExtension);
        CheckValidationString(this, FileExtension);
#endif
        
        e.Writer.Write(FileNameWithoutExtension);
        e.Writer.Write(FileExtension);
        e.Writer.Write(LastSegmentIndex);
        e.Writer.Write(LastSegmentLength);
    }
}