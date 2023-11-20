using DarkRift;
using Newtonsoft.Json;
using static SharedValue;

[Serializable]
public abstract class ServerResponseModelBase : IDarkRiftSerializable
{
    [JsonProperty(nameof(ClientID))]public ushort ClientID { get; set; }
    [JsonProperty(nameof(State))]public ushort State { get; set; }
    [JsonProperty(nameof(Log))]public string Log { get; set; }

    [JsonConstructor]
    protected ServerResponseModelBase(ushort clientID, ushort state, string log)
    {
        ClientID = clientID;
        State = state;
        Log = string.IsNullOrEmpty(log) ? SuccessResponse : log;
    }
    public virtual void Deserialize(DeserializeEvent e)
    {
        ClientID = e.Reader.ReadUInt16();
        State = e.Reader.ReadUInt16();
        Log = e.Reader.ReadString();
    }

    public virtual void Serialize(SerializeEvent e)
    {
        e.Writer.Write(ClientID);
        e.Writer.Write(State);
        e.Writer.Write(Log);
    }
}
