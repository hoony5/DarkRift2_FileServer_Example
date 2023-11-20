using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestUploadFile : ServerRequestModelBase
{
    [JsonProperty(nameof(PartyKey))] public string PartyKey { get; private set; }
    [JsonProperty(nameof(Segment))] public FileSegment Segment { get; private set; }
    [JsonConstructor]
    public RequestUploadFile(string partyKey, FileSegment segment, string id, ushort state, string log)
        : base(id, state, log)
    {
        PartyKey = partyKey;
        Segment = segment;
    }
    public RequestUploadFile() : this(StringNullValue, new FileSegment(),StringNullValue, NumericNullValue,
        SuccessResponse) { }
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        PartyKey = e.Reader.ReadString();
        Segment = e.Reader.ReadSerializable<FileSegment>();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, Segment);
#endif
        e.Writer.Write(PartyKey);
        e.Writer.Write(Segment);
    }
}