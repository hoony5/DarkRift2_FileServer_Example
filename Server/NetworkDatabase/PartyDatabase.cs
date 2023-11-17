using System.Collections.Concurrent;

public class PartyDatabase(ushort capacity = 100) : IDisposable
{
    private readonly NetworkCacheData<Party> _partyCacheData = new(capacity);
    private readonly ConcurrentDictionary<string, NetworkCacheData<FileSegmentsInfo>> _uploadedFilesCacheData 
        = new(ConcurrencyLevel, capacity);
    private readonly ConcurrentDictionary<string, NetworkCacheData<FileSegmentsInfo>> _downloadedFilesCacheData 
        = new(ConcurrencyLevel, capacity);

    public NetworkCacheData<Party> PartyMap => _partyCacheData;
    public ConcurrentDictionary<string, NetworkCacheData<FileSegmentsInfo>>  UploadedFilesMap => _uploadedFilesCacheData;
    public ConcurrentDictionary<string, NetworkCacheData<FileSegmentsInfo>>  DownloadedFilesMap => _downloadedFilesCacheData;
    
    public void AddUploadedFile(string key, string fileName, FileSegmentsInfo fileSegmentsInfo)
    {
        if (!_uploadedFilesCacheData.TryGetValue(key, out NetworkCacheData<FileSegmentsInfo>? value))
        {
            value = new NetworkCacheData<FileSegmentsInfo>();
            _ = _uploadedFilesCacheData.TryAdd(key, value);
        }
        
        value.AddOrUpdate(fileName, fileSegmentsInfo);
    }
    
    
    public void Dispose()
    {
        _partyCacheData.Clear();
        _uploadedFilesCacheData.Clear();
        _downloadedFilesCacheData.Clear();
    }
}