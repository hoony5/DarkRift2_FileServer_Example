using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public abstract class ServerRequestModelBase : IDarkRiftSerializable
{
    [JsonProperty(nameof(ID))]public string ID { get; private set; }
    [JsonProperty(nameof(State))]public ushort State { get; set; }
    [JsonProperty(nameof(Log))]public string Log { get; set; }

    [JsonConstructor]
    protected ServerRequestModelBase(string id, ushort state, string log)
    {
        ID = id;
        State = state;
        Log = string.IsNullOrEmpty(log) ? SuccessResponse : log;
    }
    public virtual void Deserialize(DeserializeEvent e)
    {
        ID = e.Reader.ReadString();
        State = e.Reader.ReadUInt16();
        Log = e.Reader.ReadString();
    }

    public virtual void Serialize(SerializeEvent e)
    {
#if DEBUG
        CheckValidationString(this, ID);
#endif
        e.Writer.Write(ID);
        e.Writer.Write(State);
        e.Writer.Write(Log);
    }
}