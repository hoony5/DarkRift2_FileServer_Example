[Serializable]
[method:JsonConstructor]
public class RequestUploadValidation(
        ushort senderClientID,
        string senderPartyKey,
        int lastSegmentIndex,
        int lastSegmentLength, 
        string fileNameWithoutExtension,
        string fileType,
        string id) 
    : ServerRequestModelBase(id)
{
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; }
    [JsonProperty(nameof(SenderPartyKey))]public string SenderPartyKey { get; private set; }
    [JsonProperty(nameof(LastSegmentIndex))]public int LastSegmentIndex { get; private set; }
    [JsonProperty(nameof(LastSegmentLength))]public int LastSegmentLength { get; private set; }
    [JsonProperty(nameof(FileNameWithoutExtension))]public string FileNameWithoutExtension { get; private set; }
    [JsonProperty(nameof(FileType))]public string FileType { get; private set; }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        
        SenderClientID = e.Reader.ReadUInt16();
        SenderPartyKey = e.Reader.ReadString();
        LastSegmentIndex = e.Reader.ReadInt32();
        LastSegmentLength = e.Reader.ReadInt32();
        FileNameWithoutExtension = e.Reader.ReadString();
        FileType = e.Reader.ReadString();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, SenderPartyKey);
        CheckValidationString(this, FileNameWithoutExtension);
        CheckValidationString(this, FileType);
#endif
        
        e.Writer.Write(SenderClientID);
        e.Writer.Write(SenderPartyKey);
        e.Writer.Write(LastSegmentIndex);
        e.Writer.Write(LastSegmentLength);
        e.Writer.Write(FileNameWithoutExtension);
        e.Writer.Write(FileType);
    }
}