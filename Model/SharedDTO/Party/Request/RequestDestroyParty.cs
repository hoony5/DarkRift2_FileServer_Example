[Serializable]
public class RequestDestroyParty : IDarkRiftSerializable
{
    public string SenderAccountID { get; set; }

    public string PartyName { get; set; }

    public ushort SenderClientID { get; set; }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(SenderAccountID);
        e.Writer.Write(PartyName);
        e.Writer.Write(SenderClientID);
    }

    public void Deserialize(DeserializeEvent e)
    {
        SenderAccountID = e.Reader.ReadString();
        PartyName = e.Reader.ReadString();
        SenderClientID = e.Reader.ReadUInt16();
    }
}