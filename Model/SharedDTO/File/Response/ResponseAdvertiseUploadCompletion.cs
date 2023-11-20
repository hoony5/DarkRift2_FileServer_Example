using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;

[Serializable]
public class ResponseAdvertiseUploadCompletion : ServerResponseModelBase
{
    [JsonProperty(nameof(FileFullName))] public string FileFullName { get; private set; }
    [JsonConstructor]
    public ResponseAdvertiseUploadCompletion(string fileName, ushort clientID, string log, ushort state) : base(clientID, state, log)
    {
        FileFullName = fileName;
    }
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