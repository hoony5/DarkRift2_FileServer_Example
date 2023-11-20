using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class ResponseDownloadAccept : ServerResponseModelBase
{
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; }
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; }
    [JsonProperty(nameof(LastSegmentIndex))] public int LastSegmentIndex { get; private set; }
    [JsonProperty(nameof(LastSegmentLength))] public int LastSegmentLength { get; private set; }

    [JsonConstructor]
    public ResponseDownloadAccept(string fileNameWithoutExtension,
        string fileExtension,
        int lastSegment,
        int lastSegmentLength,
        ushort clientID,
        ushort state,
        string log) : base(clientID, state, log)
    {
        FileNameWithoutExtension = fileNameWithoutExtension;
        FileExtension = fileExtension;
        LastSegmentIndex = lastSegment;
        LastSegmentLength = lastSegmentLength;
    }

    public ResponseDownloadAccept() : this(StringNullValue, StringNullValue,
        NumericNullValue, NumericNullValue, NumericNullValue, NumericNullValue,
        SuccessResponse) { }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileNameWithoutExtension = e.Reader.ReadString();
        FileExtension = e.Reader.ReadString();
        LastSegmentIndex = e.Reader.ReadInt32();
        LastSegmentLength = e.Reader.ReadInt32();
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
        e.Writer.Write(LastSegmentIndex);
        e.Writer.Write(LastSegmentLength);
    }
}