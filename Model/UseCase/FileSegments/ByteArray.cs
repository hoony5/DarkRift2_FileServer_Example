
[Serializable]
[method: JsonConstructor]
public class ByteArray(int index, byte[] bytes) : IDarkRiftSerializable
{
    [JsonProperty(nameof(Index))]public int Index { get; private set; } = index;
    [JsonProperty(nameof(Bytes))]public byte[] Bytes { get; private set; } = bytes;

    public ByteArray() : this(PreventExceptionNumericValue, PreventExceptionByteArray)
    {
        
    }
    
    public void Deserialize(DeserializeEvent e)
    {
        Index = e.Reader.ReadInt32();
        Bytes = e.Reader.ReadBytes();
    }

    public void Serialize(SerializeEvent e)
    {
#if DEBUG
        if (Bytes.Length == 0)
        {
            CheckValidation<ByteArray>(Bytes.Length == 0, "Bytes.Length == 0");
        }
#endif
        e.Writer.Write(Index);
        e.Writer.Write(Bytes);
    }
}