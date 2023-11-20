using Newtonsoft.Json;
using DarkRift;
using static DtoValidator;
using static SharedValue;
[Serializable]
public class ResponseDownloadFile : ServerResponseModelBase
{
    [JsonProperty(nameof(Segment))] public FileSegment Segment { get; set; }
    [JsonConstructor]
    public ResponseDownloadFile(ushort clientID, ushort state, string log) : base(clientID, state, log){ }

    public ResponseDownloadFile() : this(NumericNullValue, NumericNullValue, StringNullValue)
    {
        Segment = new FileSegment();
    }
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        Segment = e.Reader.ReadSerializable<FileSegment>();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationProperty(this, Segment);
#endif
        e.Writer.Write(Segment);
    }
}