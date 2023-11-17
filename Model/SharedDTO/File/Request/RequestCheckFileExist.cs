[Serializable]
[method:JsonConstructor]
public class RequestCheckFileExist(
        ushort senderClientID,
        string senderPartyKey,
        string fileNameWithoutExtension,
        string fileExtension,
        string id) 
    : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(SenderClientID))] public ushort SenderClientID { get; private set; } = senderClientID;
    [JsonProperty(nameof(SenderPartyKey))] public string SenderPartyKey { get; private set; } = senderPartyKey;
    [JsonProperty(nameof(FileNameWithoutExtension))] public string FileNameWithoutExtension { get; private set; } = fileNameWithoutExtension;
    [JsonProperty(nameof(FileExtension))] public string FileExtension { get; private set; } = fileExtension;

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