[Serializable]
[method: JsonConstructor]
public class FileSegment
    (string fileNameWithoutExtension, string fileExtension, ByteArray partition)
    : IDarkRiftSerializable
{
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; } = fileExtension;
    [JsonProperty(nameof(Partition))] public ByteArray Partition { get; private set; } = partition;

    public FileSegment()
        : this(StringNullValue, StringNullValue, new ByteArray(0, System.Array.Empty<byte>()))
    { }
    
    public void SetFileNameWithoutExtension(string fileNameWithoutExtension)
    {
        if (string.IsNullOrEmpty(fileNameWithoutExtension)) return;
        FileNameWithoutExtension = fileNameWithoutExtension;
    }
    
    public void SetFileExtension(string fileExtension)
    {
        if (string.IsNullOrEmpty(fileExtension)) return;
        FileExtension = fileExtension;
    }
    
    public void Deserialize(DeserializeEvent e)
    {
        FileNameWithoutExtension = e.Reader.ReadString();
        FileExtension = e.Reader.ReadString();
        Partition = e.Reader.ReadSerializable<ByteArray>();
    }

    public void Serialize(SerializeEvent e)
    {
#if DEBUG
        CheckValidationString(this, FileNameWithoutExtension);
        CheckValidationString(this, FileExtension);
        CheckValidationProperty(this, Partition);
#endif
        e.Writer.Write(FileNameWithoutExtension);
        e.Writer.Write(FileExtension);
        e.Writer.Write(Partition);
    }
}