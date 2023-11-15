[Serializable]
public class FileSegment
    (string fileNameWithoutExtension, string fileType, ByteArray partition)
    : IDarkRiftSerializable
{
    public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;
    public string FileType { get; private set; } = fileType;
    public ByteArray Partition { get; private set; } = partition;

    public FileSegment()
        : this(PreventExceptionStringValue, PreventExceptionStringValue, new ByteArray(0, System.Array.Empty<byte>()))
    { }
    
    public void Deserialize(DeserializeEvent e)
    {
        FileNameWithoutExtension = e.Reader.ReadString();
        FileType = e.Reader.ReadString();
        Partition = e.Reader.ReadSerializable<ByteArray>();
    }

    public void Serialize(SerializeEvent e)
    {
#if DEBUG
        CheckValidationString(this, FileNameWithoutExtension);
        CheckValidationString(this, FileType);
        CheckValidationProperty(this, Partition);
#endif
        e.Writer.Write(FileNameWithoutExtension);
        e.Writer.Write(FileType);
        e.Writer.Write(Partition);
    }
}