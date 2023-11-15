
[Serializable]
[method:JsonConstructor]
public abstract class ServerRequestModelBase
    (string id) : IDarkRiftSerializable 
{
    public string ID { get; private set; } = id;
    public ushort State { get; set; }
    public string Log { get; set; } = SuccessResponse;
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