[Serializable]
[method:JsonConstructor]
public class RequestDownloadFile
    (int fileSegmentIndex, string fileNameWithoutExtension, string fileExtension, string id)
    : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(FileSegmentIndex))] public int FileSegmentIndex { get; private set; } = fileSegmentIndex;
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; } = fileExtension;
    
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileNameWithoutExtension = e.Reader.ReadString();
        FileExtension = e.Reader.ReadString();
        FileSegmentIndex = e.Reader.ReadInt32();
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
        e.Writer.Write(FileSegmentIndex);
    }
}