public class DatabaseCenter
{
    public static DatabaseCenter Instance => _instance;
    private static DatabaseCenter? _instance;
    private UserDatabase? _userCacheMap;
    private PartyDatabase? _partyCacheMap;

    public DatabaseCenter(ushort capacity = 5)
    {
        if (_instance is not null) return;
        _instance = this;
        _userCacheMap = new UserDatabase(capacity);
    }
    
    public UserDatabase GetUserDb()
    {
        if (_userCacheMap is null) return _userCacheMap = new UserDatabase();
        
        return _userCacheMap;
    }
    public PartyDatabase GetPartyDb()
    {
        if (_partyCacheMap is null) return _partyCacheMap = new PartyDatabase();

        return _partyCacheMap;
    }
}
