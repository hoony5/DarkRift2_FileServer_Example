using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestCheckFileExist : ServerRequestModelBase
{
    [JsonProperty(nameof(SenderClientID))] public ushort SenderClientID { get; private set; }
    [JsonProperty(nameof(SenderPartyKey))] public string SenderPartyKey { get; private set; }
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; }
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; }

    [JsonConstructor]
    public RequestCheckFileExist(ushort senderClientID,
        string senderPartyKey,
        string fileNameWithoutExtension,
        string fileExtension,
        string id,
        ushort state,
        string log) : base(id, state, log)
    {
        SenderClientID = senderClientID;
        SenderPartyKey = senderPartyKey;
        FileNameWithoutExtension = fileNameWithoutExtension;
        FileExtension = fileExtension;
    }
    public RequestCheckFileExist()
        : this(NumericNullValue, StringNullValue, StringNullValue,
            StringNullValue, StringNullValue, NumericNullValue, SuccessResponse) { }
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        SenderClientID = e.Reader.ReadUInt16();
        SenderPartyKey = e.Reader.ReadString();
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
        e.Writer.Write(FileNameWithoutExtension);
        e.Writer.Write(FileExtension);
    }
}