using Newtonsoft.Json;
using DarkRift;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class ResponseUploadAccept : ServerResponseModelBase
{
    [JsonProperty(nameof(FileNameWithoutExtension))]public string FileNameWithoutExtension { get; private set; }
    [JsonProperty(nameof(FileExtension))]public string FileExtension { get; private set; }
    [JsonConstructor]
    public ResponseUploadAccept(string fileNameWithoutExtension,
        string fileExtension,
        ushort clientID, ushort state, string log) : base(clientID, state, log)
    {
        FileNameWithoutExtension = fileNameWithoutExtension;
        FileExtension = fileExtension;
    }
    public ResponseUploadAccept() : this(StringNullValue, StringNullValue, NumericNullValue, NumericNullValue, SuccessResponse) { }
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