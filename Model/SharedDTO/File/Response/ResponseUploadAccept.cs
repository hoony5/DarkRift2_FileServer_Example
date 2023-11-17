[Serializable]
[method: JsonConstructor]
public class ResponseUploadAccept(
        string fileNameWithoutExtension,
        string fileExtension,
        ushort clientID) 
    : ServerResponseModelBase(clientID)
{
    public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;

    public string FileExtension { get; private set; } = fileExtension;

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileNameWithoutExtension = e.Reader.ReadString();
        FileExtension = e.Reader.ReadString();
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
    }
}