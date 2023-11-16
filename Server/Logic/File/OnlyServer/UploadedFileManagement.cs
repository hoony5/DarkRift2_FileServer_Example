
// rough code
public class UploadedFileManagement
{
    private readonly Dictionary<string, NetworkCacheData<FileSegmentsInfo>> _uploadedFiles;

    public void AddUploadList(string partyKey,string fileName, int fileBytesLength)
    {
        if (!_uploadedFiles.ContainsKey(partyKey))
            _uploadedFiles.Add(partyKey, new Dictionary<string, FileSegmentsInfo>(0));

        if(!_uploadedFiles[partyKey].ContainsKey(fileName))
            _uploadedFiles[partyKey].Add(fileName, new FileSegmentsInfo(fileBytesLength));

        _uploadedFiles[partyKey][fileName] = new FileSegmentsInfo(fileBytesLength);
    }

    public bool UpdateUploadFileList(string partyKey,string fileName, RequestUploadFile req)
    {
        if (!_uploadedFiles.ContainsKey(partyKey)) return false;
        if (!_uploadedFiles[partyKey].ContainsKey(fileName)) return false;

        if (req.Data.byteArray.index == 0)
            _uploadedFiles[partyKey][fileName] = _uploadedFiles[partyKey][fileName].SetFileBytes(req.Data)
                .SetLastBytesLength(req.Data);
        else
            _uploadedFiles[partyKey][fileName] = _uploadedFiles[partyKey][fileName].SetFileBytes(req.Data);
        return true;
    }
    public FileSegmentsInfo GetUploadFileSegment(string partyKey, string fileName)
    {
        if (!_uploadedFiles.ContainsKey(partyKey) || !_uploadedFiles[partyKey].ContainsKey(fileName)) return new FileSegmentsInfo();

        return _uploadedFiles[partyKey].ContainsKey(fileName) ? _uploadedFiles[partyKey][fileName] : new FileSegmentsInfo();
    }
    public void CopyUploadToDownload(string partyKey, string fileName)
    {
        if (!_uploadedFiles.ContainsKey(partyKey)) return;

        if(!_uploadedFiles[partyKey].ContainsKey(fileName)) return;

        if(!downloadFiles.ContainsKey(partyKey))
            downloadFiles.Add(partyKey, new Dictionary<string, FileSegmentsInfo>(0));

        if(!downloadFiles[partyKey].ContainsKey(fileName))
            downloadFiles[partyKey].Add(fileName, new FileSegmentsInfo(0));

        _uploadedFiles[partyKey][fileName].count = 0;
        downloadFiles[partyKey][fileName] = _uploadedFiles[partyKey][fileName];
    }
    private void RemoveUploadFiles(string partyKey)
    {
        if (!_uploadedFiles.ContainsKey(partyKey)) return;
        _uploadedFiles.Remove(partyKey);
    }

    public void RemoveFiles(LoadType type, string partyKey)
    {
        switch (type)
        {
            case LoadType.Upload:
                RemoveUploadFiles(partyKey);
                break;
            case LoadType.Download:
                RemoveDownloadFiles(partyKey);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    public (string fileFullName,byte[] datas) GetUploadedFileBytes(string partyKey, string fileName)
    {
        if(!_uploadedFiles.ContainsKey(partyKey))
            _uploadedFiles.Add(partyKey , new Dictionary<string, FileSegmentsInfo>(0));

        if(!_uploadedFiles[partyKey].ContainsKey(fileName))
            _uploadedFiles[partyKey].Add(fileName, new FileSegmentsInfo(0));

        var infos = _uploadedFiles[partyKey][fileName];

        var byteList = new List<byte>(0);

        foreach (var item in infos.fileToBytesData)
        {
            FileServerPlugin.Debug($"item {item.byteArray.index} - is added.");
            byteList.AddRange(item.byteArray.bytes);
        }

        return (fileName, byteList.ToArray());
    }

}