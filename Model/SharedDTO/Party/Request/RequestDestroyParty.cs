[Serializable]
[method: JsonConstructor]
public class RequestDestroyParty(string senderAccountID, string partyName, ushort senderClientID)
    : ServerRequestModelBase(senderAccountID)
{
    [JsonProperty(nameof(SenderAccountID))]public string SenderAccountID { get; private set; } = senderAccountID;
    [JsonProperty(nameof(PartyName))]public string PartyName { get; private set; } = partyName;
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; } = senderClientID;
    
    public RequestDestroyParty()
        : this(
            PreventExceptionStringValue,
            PreventExceptionStringValue,
            PreventExceptionNumericValue)
    {
        
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, SenderAccountID);
        CheckValidationString(this, PartyName);
#endif
        e.Writer.Write(SenderAccountID);
        e.Writer.Write(PartyName);
        e.Writer.Write(SenderClientID);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        SenderAccountID = e.Reader.ReadString();
        PartyName = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}