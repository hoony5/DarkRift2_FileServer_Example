[Serializable]
[method: JsonConstructor]
public class ResponseDownloadValidation(
        string fileNameWithoutExtension,
        string fileType,
        int lastSegment,
        int lastSegmentLength,
        ushort clientID)
    : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;
    [JsonProperty(nameof(FileType))] public string FileType { get; private set; } = fileType;
    [JsonProperty(nameof(LastSegmentIndex))] public int LastSegmentIndex { get; private set; } = lastSegment;
    [JsonProperty(nameof(LastSegmentLength))] public int LastSegmentLength { get; private set; } = lastSegmentLength;

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileNameWithoutExtension = e.Reader.ReadString();
        FileType = e.Reader.ReadString();
        LastSegmentIndex = e.Reader.ReadInt32();
        LastSegmentLength = e.Reader.ReadInt32();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, FileNameWithoutExtension);
        CheckValidationString(this, FileType);
#endif
        
        e.Writer.Write(FileNameWithoutExtension);
        e.Writer.Write(FileType);
        e.Writer.Write(LastSegmentIndex);
        e.Writer.Write(LastSegmentLength);
    }
}