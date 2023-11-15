[Serializable]
public abstract class ServerResponseModelBase(ushort clientID) : IDarkRiftSerializable 
{
    public ushort ClientID { get; private set; } = clientID;
    public ushort State { get; set; }
    public string Log { get; set; } = SuccessResponse;

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
