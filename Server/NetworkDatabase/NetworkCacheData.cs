using System.Collections.Concurrent;

public class NetworkCacheData<TValue>(ushort capacity = 100)
{
    private readonly ConcurrentDictionary<string, TValue> _cacheMap 
        = new ConcurrentDictionary<string, TValue>(ConcurrencyLevel, capacity);

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
        if (_cacheMap.TryGetValue(key, out TValue? comparisonValue))
        {
            _cacheMap.TryUpdate(key, value, comparisonValue);
            FileServer.DebugLog($"key: {key} | value : {nameof(TValue)} => Updated.");
        }
        else
        {
            _cacheMap.TryAdd(key, value);
            FileServer.DebugLog($"key: {key} | value : {nameof(TValue)} => Added.");
        }
    }
    public bool ContainsKey(string key)
    {
        return _cacheMap.ContainsKey(key);
    }
    public void Remove(string key)
    {
        if (!_cacheMap.ContainsKey(key))
        {
            FileServer.DebugLog($"key: {key} => Already Removed.");
            return;
        }
        _ = _cacheMap.TryRemove(key, out _);
        FileServer.DebugLog($"key: {key} => Removed.");
    }
    
    public void Clear()
    {
        _cacheMap.Clear();
        FileServer.DebugLog($"{nameof(TValue)} Map is Cleared.");
    }

    public int Count => _cacheMap.Count;
}
