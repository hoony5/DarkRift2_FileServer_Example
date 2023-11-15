public class NetworkDatabase
{
    private readonly NetworkCacheData<IClient> _clientCacheData 
        = new NetworkCacheData<IClient>(100);
    
    private readonly NetworkCacheData<UserHeader> _userCacheData 
        = new NetworkCacheData<UserHeader>(100);
    
    private readonly NetworkCacheData<Party> _partyCacheData 
        = new NetworkCacheData<Party>(100);
    
    
}
