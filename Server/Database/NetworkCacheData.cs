public class NetworkCacheData<T>(ushort capacity)
{
    private readonly Dictionary<string, T> _cacheMap 
        = new Dictionary<string, T>(capacity);

    public bool TryGetValue(string key, out T value)
    {
        return _cacheMap.TryGetValue(key, out value);
    }
    public void AddOrUpdate(string key, T value)
    {
        if (_cacheMap.ContainsKey(key))
            _cacheMap[key] = value;
        else
            _cacheMap.Add(key, value);
    } 
    public void Remove(string key)
    {
        if (!_cacheMap.ContainsKey(key)) return;
        _cacheMap.Remove(key);
    }
}
