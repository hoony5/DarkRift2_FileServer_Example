public class PartyDatabase(ushort capacity = 100) : IDisposable
{
    private readonly NetworkCacheData<Party> _partyCacheData = new(capacity);
    private readonly NetworkCacheData<FileSegmentsInfo> _uploadedFilesCacheData = new(capacity);
    private readonly NetworkCacheData<FileSegmentsInfo> _downloadedFilesCacheData = new(capacity);

    public NetworkCacheData<Party> PartyMap => _partyCacheData;
    public NetworkCacheData<FileSegmentsInfo> UploadedFilesMap => _uploadedFilesCacheData;
    public NetworkCacheData<FileSegmentsInfo> DownloadedFilesMap => _downloadedFilesCacheData;
    public void Dispose()
    {
        _partyCacheData.Clear();
        _uploadedFilesCacheData.Clear();
        _downloadedFilesCacheData.Clear();
    }
}