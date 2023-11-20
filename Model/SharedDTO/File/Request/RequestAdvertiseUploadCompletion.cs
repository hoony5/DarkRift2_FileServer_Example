using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestAdvertiseUploadCompletion : ServerRequestModelBase
{
    [JsonProperty(nameof(FileFullName))] public string FileFullName { get; private set; }
    [JsonConstructor]
    public RequestAdvertiseUploadCompletion(string fileName, string id, string log, ushort state) : base(id, state, log)
    {
        FileFullName = fileName;
    }
    public RequestAdvertiseUploadCompletion() : base(StringNullValue, NumericNullValue, StringNullValue)
    {
        FileFullName = StringNullValue;
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