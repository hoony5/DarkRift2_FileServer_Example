public class DatabaseCenter
{
    public static DatabaseCenter Instance => _instance;
    private static DatabaseCenter? _instance;
    private NetworkDatabase? _cacheMap;
    
    public DatabaseCenter(ushort capacity = 5)
    {
        if (_instance is not null) return;
        _instance = this;
        _cacheMap = new NetworkDatabase(capacity);
    }
    
    public NetworkDatabase GetDb()
    {
        if (_cacheMap is null) return _cacheMap = new NetworkDatabase();
        
        return _cacheMap;
    }
}
