using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class FileSegment : IDarkRiftSerializable
{
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; }
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; }
    
    [JsonProperty(nameof(Index))]public int Index { get; private set; }
    [JsonProperty(nameof(Bytes))]public byte[] Bytes { get; private set; }

    public FileSegment() : this(StringNullValue, StringNullValue, NumericNullValue,
        ByteNullArray) { }

    [JsonConstructor]
    public FileSegment(string fileNameWithoutExtension, string fileExtension, int index, byte[] bytes)
    {
        FileNameWithoutExtension = fileNameWithoutExtension;
        FileExtension = fileExtension;
        Index = index;
        Bytes = bytes;
    }

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
    public void SetBytes(byte[]? bytes)
    {
        if (bytes == null) return;
        Bytes = bytes;
    }

    public void SetBytesIndex(int index)
    {
        Index = index;
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