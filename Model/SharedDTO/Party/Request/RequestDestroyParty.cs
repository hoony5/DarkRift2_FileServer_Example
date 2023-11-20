using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class RequestDestroyParty : ServerRequestModelBase
{
    [JsonProperty(nameof(PartyName))]public string PartyName { get; private set; }
    [JsonProperty(nameof(SenderClientID))]public ushort SenderClientID { get; private set; }
    
    public RequestDestroyParty() : this(StringNullValue, StringNullValue,
        NumericNullValue, NumericNullValue, StringNullValue) { }

    [JsonConstructor]
    public RequestDestroyParty(string senderAccountID, string partyName, ushort senderClientID, ushort state, string log)
        : base(senderAccountID, state, log)
    {
        PartyName = partyName;
        SenderClientID = senderClientID;
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationString(this, PartyName);
#endif
        e.Writer.Write(PartyName);
        e.Writer.Write(SenderClientID);
    }

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        PartyName = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}