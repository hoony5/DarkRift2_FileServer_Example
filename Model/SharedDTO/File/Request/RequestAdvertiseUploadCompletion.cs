[Serializable]
[method:JsonConstructor]
public class RequestAdvertiseUploadCompletion(string fileName, string id)
    : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(FileFullName))] public string FileFullName { get; private set; } = fileName;
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileFullName = e.Reader.ReadString();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, FileFullName);
#endif
        
        e.Writer.Write(FileFullName);
    }
}