[System.Serializable]
[method:JsonConstructor]
public class Party(
    string name,
    string password,
    ushort currentPlayer,
    ushort maxPlayers,
    ushort state,
    Queue<string> historyQueue,
    UserHeader leader,
    UserHeader[] members,
    ushort serverID) 
    : IDarkRiftSerializable
{
    public Party() : this(
        PreventExceptionStringValue,
        PreventExceptionStringValue,
        PreventExceptionNumericValue,
        PreventExceptionNumericValue,
        PreventExceptionNumericValue,
        new Queue<string>(16),
        new UserHeader(),
        new UserHeader[]{new()},
        PreventExceptionNumericValue)
    {
        
    }

    public string Name { get; private set; } = name;
    public string Password { get; private set; } = password;
    public ushort CurrentPlayers { get; private set; } = currentPlayer;
    public ushort MaxPlayers { get; private set; } = maxPlayers;
    public ushort State { get; private set; } = state;
    public Queue<string> HistoryQueue { get; private set; } = historyQueue;
    public UserHeader Leader { get; private set; } = leader;
    public UserHeader[] Members { get; private set; } = members;
    public ushort ServerID { get; private set; } = serverID;
    
    public void Deserialize(DeserializeEvent e)
    {
        Name = e.Reader.ReadString();
        Password = e.Reader.ReadString();
        CurrentPlayers = e.Reader.ReadUInt16();
        MaxPlayers = e.Reader.ReadUInt16();
        State = e.Reader.ReadUInt16();
        
        string[] historyArray = e.Reader.ReadStrings();
        
        HistoryQueue = historyArray.Length != 0 ? 
            new Queue<string>(historyArray) :
            new Queue<string>(16);
        
        Leader = e.Reader.ReadSerializable<UserHeader>();
        Members = e.Reader.ReadSerializables<UserHeader>();
        ServerID = e.Reader.ReadUInt16();
    }

    public void Serialize(SerializeEvent e)
    {
#if DEBUG
        CheckValidationString(this, Name);
        CheckValidationString(this, Password);
        CheckValidationProperty(this, Leader);
        CheckValidationProperty(this, Members);
#endif
        e.Writer.Write(Name);
        e.Writer.Write(Password);
        e.Writer.Write(CurrentPlayers);
        e.Writer.Write(MaxPlayers);
        e.Writer.Write(State);

        e.Writer.Write(
            HistoryQueue.Count is 0 ?
            PreventExceptionStringArray :
            HistoryQueue.ToArray()
            );

        e.Writer.Write(Leader);
        e.Writer.Write(Members);
        e.Writer.Write(ServerID);
    }
}
