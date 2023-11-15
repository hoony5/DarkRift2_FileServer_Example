[Serializable]
[method: JsonConstructor]
public class ResponseUploadFile(
        string fileNameWithoutExtension,
        string fileType,
        int uploadSegmentIndex,
        ushort clientID)
    : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;
    [JsonProperty(nameof(FileType))] public string FileType { get; private set; } = fileType;
    [JsonProperty(nameof(UploadSegmentIndex))] public int UploadSegmentIndex { get; private set; } = uploadSegmentIndex;

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileNameWithoutExtension = e.Reader.ReadString();
        FileType = e.Reader.ReadString();
        UploadSegmentIndex = e.Reader.ReadInt32();
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
        e.Writer.Write(UploadSegmentIndex);
    }
}