[Serializable]
[method: JsonConstructor]
public class ResponseUploadValidation(
        string fileNameWithoutExtension,
        string fileType,
        ushort clientID) 
    : ServerResponseModelBase(clientID)
{
    public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;

    public string FileType { get; private set; } = fileType;

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileNameWithoutExtension = e.Reader.ReadString();
        FileType = e.Reader.ReadString();
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
    }
}