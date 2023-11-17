﻿[Serializable]
[method: JsonConstructor]
public class ResponseUploadFile(
        string fileNameWithoutExtension,
        string fileExtension,
        int uploadSegmentIndex,
        ushort clientID)
    : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; } = fileExtension;
    [JsonProperty(nameof(UploadSegmentIndex))] public int UploadSegmentIndex { get; private set; } = uploadSegmentIndex;

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileNameWithoutExtension = e.Reader.ReadString();
        FileExtension = e.Reader.ReadString();
        UploadSegmentIndex = e.Reader.ReadInt32();
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
        e.Writer.Write(UploadSegmentIndex);
    }
}