using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[System.Serializable]
public class Party : IDarkRiftSerializable
{
    private const int LEADER_COUNT = 1;
    [JsonProperty(nameof(Name))] public string Name { get; private set; }
    [JsonProperty(nameof(Password))] public string Password { get; private set; }
    [JsonProperty(nameof(Key))] public string Key { get; private set; }
    [JsonProperty(nameof(CurrentPlayers))] public int CurrentPlayers { get; set; }
    [JsonProperty(nameof(MaxPlayers))] public int MaxPlayers { get; private set; }
    [JsonProperty(nameof(State))] public ushort State { get; private set; }
    [JsonProperty(nameof(HistoryQueue))] public Queue<string> HistoryQueue { get; private set; }
    [JsonProperty(nameof(Leader))] public UserHeader Leader { get; set; }
    [JsonProperty(nameof(Members))] public Dictionary<string, UserHeader> Members { get; private set; } 
    [JsonProperty(nameof(ServerID))] public ushort ServerID { get; set; }

    public Party() : this(
        StringNullValue,
        StringNullValue,
        StringNullValue,
        NumericNullValue,
        NumericNullValue,
        new UserHeader())
    {
        Members = new Dictionary<string, UserHeader>(MaxPlayers);
        HistoryQueue = new Queue<string>(16);
    }

    [JsonConstructor]
    public Party(string name,
        string password,
        string key,
        int maxPlayers,
        ushort state,
        UserHeader leader)
    {
        Name = name;
        Password = password;
        Key = key;
        MaxPlayers = maxPlayers;
        State = state;
        Leader = leader;
    }
    public void AddOrUpdateMember(UserHeader member)
    {
        if (Members.ContainsKey(member.AccountID))
        {
            Members[member.AccountID] = member;
        }
        else
        {
            Members.Add(member.AccountID, member);
        }
    }
    public void RemoveMember(string accountID)
    {
        if (!Members.ContainsKey(accountID)) return;
        Members.Remove(accountID);
    }
    
    public void PassLeaderToMemberOne()
    {
        if (Members.Count is 0) return;
        UserHeader member = Members.FirstOrDefault().Value;
        if (member.AccountID.Equals(StringNullValue)) return;
        Leader = new UserHeader(member.AccountID)
        {
            NickName = member.NickName,
            ConnectionState = member.ConnectionState,
            PartyKey = Key
        };
        Members.Remove(member.AccountID);
        CurrentPlayers = LEADER_COUNT + Members.Count;
    }

    public void UpdateCurrentPlayers()
    {
        CurrentPlayers = LEADER_COUNT + Members.Count;
    }
    public void Deserialize(DeserializeEvent e)
    {
        Name = e.Reader.ReadString();
        Password = e.Reader.ReadString();
        Key = e.Reader.ReadString();
        CurrentPlayers = e.Reader.ReadInt32();
        MaxPlayers = e.Reader.ReadInt32();
        State = e.Reader.ReadUInt16();
        
        string[] historyArray = e.Reader.ReadStrings();
        
        HistoryQueue = historyArray.Length != 0 ? 
            new Queue<string>(historyArray) :
            new Queue<string>(16);
        
        Leader = e.Reader.ReadSerializable<UserHeader>();
        Members = e.Reader.ReadSerializables<UserHeader>().ToDictionary(key => key.AccountID, value => value);
        ServerID = e.Reader.ReadUInt16();
    }

    public void Serialize(SerializeEvent e)
    {
#if DEBUG
        CheckValidationString(this, Name);
        CheckValidationString(this, Password);
        CheckValidationString(this, Key);
        CheckValidationProperty(this, Leader);
        CheckValidationProperty(this, Members);
#endif
        e.Writer.Write(Name);
        e.Writer.Write(Password);
        e.Writer.Write(Key);
        e.Writer.Write(CurrentPlayers);
        e.Writer.Write(MaxPlayers);
        e.Writer.Write(State);

        e.Writer.Write(
            HistoryQueue.Count is 0 ?
            StringNullArray :
            HistoryQueue.ToArray()
            );

        e.Writer.Write(Leader);

        e.Writer.Write(Members.Count is 0 
            ? new UserHeader[] { new() } 
            : Members.Values.ToArray());

        e.Writer.Write(ServerID);
    }
}
