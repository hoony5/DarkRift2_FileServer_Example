public class UserDatabase(ushort capacity = 100) : IDisposable
{
    private readonly NetworkCacheData<IClient> _clientCacheData = new(capacity);
    private readonly NetworkCacheData<UserHeader> _userCacheData = new(capacity);

    public NetworkCacheData<IClient> UserClientMap => _clientCacheData;
    public NetworkCacheData<UserHeader> UserHeaderMap => _userCacheData;
    public void Dispose()
    {
        _clientCacheData.Clear();
        _userCacheData.Clear();
    }
}