public class NetworkCacheData<TValue>(ushort capacity)
{
    private readonly Dictionary<string, TValue> _cacheMap 
        = new Dictionary<string, TValue>(capacity);

    public bool TryGetValue(string key, out TValue? value)
    {
#if Debug
        bool exist = _cacheMap.TryGetValue(key, out value);
        FileServer.DebugLog($"{(exist ? $"key:{key} is Loaded." : $"key: {key} => There is no pair.")}");
        return exist;
#else
        return _cacheMap.TryGetValue(key, out value);
#endif
    }

    public TValue[] Values => _cacheMap.Values.ToArray();
    public void AddOrUpdate(string key, TValue value)
    {
        if (_cacheMap.ContainsKey(key))
        {
            _cacheMap[key] = value;
            FileServer.DebugLog($"key: {key} | value : {nameof(TValue)} => Updated.");
        }
        else
        {
            _cacheMap.Add(key, value);
            FileServer.DebugLog($"key: {key} | value : {nameof(TValue)} => Added.");
        }
    } 
    public void Remove(string key)
    {
        if (!_cacheMap.ContainsKey(key))
        {
            FileServer.DebugLog($"key: {key} => Already Removed.");
            return;
        }
        _cacheMap.Remove(key);
        FileServer.DebugLog($"key: {key} => Removed.");
    }
    
    public void Clear()
    {
        _cacheMap.Clear();
        FileServer.DebugLog($"{nameof(TValue)} Map is Cleared.");
    }

    public int Count => _cacheMap.Count;
}
