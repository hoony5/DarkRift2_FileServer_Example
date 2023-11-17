[Serializable]
[method: JsonConstructor]
public class FileSegment
    (string fileNameWithoutExtension, string fileExtension, int index, byte[] bytes)
    : IDarkRiftSerializable
{
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; } = fileExtension;
    
    [JsonProperty(nameof(Index))]public int Index { get; private set; } = index;
    [JsonProperty(nameof(Bytes))]public byte[] Bytes { get; private set; } = bytes;

    public FileSegment()
        : this(StringNullValue, StringNullValue, NumericNullValue, ByteNullArray)
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
        Index = e.Reader.ReadInt32();
        Bytes = e.Reader.ReadBytes();
    }

    public void Serialize(SerializeEvent e)
    {
#if DEBUG
        CheckValidationString(this, FileNameWithoutExtension);
        CheckValidationString(this, FileExtension);
#endif
        e.Writer.Write(FileNameWithoutExtension);
        e.Writer.Write(FileExtension);
        e.Writer.Write(Index);
    }
}