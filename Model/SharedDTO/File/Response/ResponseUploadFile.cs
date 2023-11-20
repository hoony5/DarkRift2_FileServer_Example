using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class ResponseUploadFile : ServerResponseModelBase
{
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; }
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; }
    [JsonProperty(nameof(UploadSegmentIndex))] public int UploadSegmentIndex { get; private set; }

    [JsonConstructor]
    public ResponseUploadFile(string fileNameWithoutExtension,
        string fileExtension,
        int uploadSegmentIndex,
        ushort clientID,
        ushort state,
        string log) : base(clientID, state, log)
    {
        FileNameWithoutExtension = fileNameWithoutExtension;
        FileExtension = fileExtension;
        UploadSegmentIndex = uploadSegmentIndex;
    }
    public ResponseUploadFile() : this(StringNullValue, StringNullValue, NumericNullValue,
        NumericNullValue, NumericNullValue, SuccessResponse) { }
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileNameWithoutExtension = e.Reader.ReadString();
        FileExtension = e.Reader.ReadString();
        UploadSegmentIndex = e.Reader.ReadInt32();
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
        e.Writer.Write(UploadSegmentIndex);
    }
}