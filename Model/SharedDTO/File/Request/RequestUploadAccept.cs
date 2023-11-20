using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestUploadAccept : ServerRequestModelBase
{
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; }
    [JsonProperty(nameof(SenderPartyKey))]public string SenderPartyKey { get; private set; }
    [JsonProperty(nameof(LastSegmentIndex))]public int LastSegmentIndex { get; private set; }
    [JsonProperty(nameof(LastSegmentByteLength))]public int LastSegmentByteLength { get; private set; }
    [JsonProperty(nameof(FileNameWithoutExtension))]public string FileNameWithoutExtension { get; private set; }
    [JsonProperty(nameof(FileExtension))]public string FileExtension { get; private set; }

    [JsonConstructor]
    public RequestUploadAccept(ushort senderClientID,
        string senderPartyKey,
        int lastSegmentIndex,
        int lastSegmentLength,
        string fileNameWithoutExtension,
        string fileExtension,
        string id,
        ushort state,
        string log) : base(id,state,log)
    {
        SenderClientID = senderClientID;
        SenderPartyKey = senderPartyKey;
        LastSegmentIndex = lastSegmentIndex;
        LastSegmentByteLength = lastSegmentLength;
        FileNameWithoutExtension = fileNameWithoutExtension;
        FileExtension = fileExtension;
    }
    public RequestUploadAccept()
        : this(NumericNullValue, StringNullValue, NumericNullValue,
            NumericNullValue, StringNullValue, StringNullValue,
            StringNullValue, NumericNullValue, SuccessResponse) { }
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        
        SenderClientID = e.Reader.ReadUInt16();
        SenderPartyKey = e.Reader.ReadString();
        LastSegmentIndex = e.Reader.ReadInt32();
        LastSegmentByteLength = e.Reader.ReadInt32();
        FileNameWithoutExtension = e.Reader.ReadString();
        FileExtension = e.Reader.ReadString();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, SenderPartyKey);
        CheckValidationString(this, FileNameWithoutExtension);
        CheckValidationString(this, FileExtension);
#endif
        
        e.Writer.Write(SenderClientID);
        e.Writer.Write(SenderPartyKey);
        e.Writer.Write(LastSegmentIndex);
        e.Writer.Write(LastSegmentByteLength);
        e.Writer.Write(FileNameWithoutExtension);
        e.Writer.Write(FileExtension);
    }
}