using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestDownloadFile : ServerRequestModelBase
{
    [JsonProperty(nameof(FileSegmentIndex))] public int FileSegmentIndex { get; private set; }
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; }
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; }
    [JsonProperty(nameof(PartyKey))] public string PartyKey { get; private set; }

    [JsonConstructor]
    public RequestDownloadFile(string partyKey, int fileSegmentIndex, string fileNameWithoutExtension,
        string fileExtension, string id, ushort state, string log) : base(id, state, log)
    {
        FileSegmentIndex = fileSegmentIndex;
        FileNameWithoutExtension = fileNameWithoutExtension;
        FileExtension = fileExtension;
        PartyKey = partyKey;
    }
    public RequestDownloadFile() : this(StringNullValue, NumericNullValue, StringNullValue,
        StringNullValue, StringNullValue, NumericNullValue, SuccessResponse) { }
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileNameWithoutExtension = e.Reader.ReadString();
        FileExtension = e.Reader.ReadString();
        FileSegmentIndex = e.Reader.ReadInt32();
        PartyKey = e.Reader.ReadString();
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
        e.Writer.Write(FileSegmentIndex);
        e.Writer.Write(PartyKey);
    }
}