[Serializable]
[method:JsonConstructor]
public class RequestAdvertiseUploadCompletion(string fileFullName, string id)
    : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(FileFullName))] public string FileFullName { get; private set; } = fileFullName;
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