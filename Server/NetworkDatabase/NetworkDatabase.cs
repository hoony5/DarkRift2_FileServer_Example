public class NetworkDatabase(ushort capacity = 100) : IDisposable
{
    private readonly NetworkCacheData<IClient> _clientCacheData = new(capacity);
    private readonly NetworkCacheData<UserHeader> _userCacheData = new(capacity);
    private readonly NetworkCacheData<Party> _partyCacheData = new(capacity);
    private readonly NetworkCacheData<FileSegment[]> _partyFilesCacheData = new(capacity);

    public NetworkCacheData<IClient> UserClientMap => _clientCacheData;
    public NetworkCacheData<UserHeader> UserHeaderMap => _userCacheData;
    public NetworkCacheData<Party> PartyMap => _partyCacheData;
    public NetworkCacheData<FileSegment[]> PartyFilesMap => _partyFilesCacheData;
    public void Dispose()
    {
        _clientCacheData.Clear();
        _userCacheData.Clear();
        _partyCacheData.Clear();
        _partyFilesCacheData.Clear();
    }
}
