
// rough code
public class UploadedFileManagement(IDictionary<string, NetworkCacheData<FileSegmentsInfo>> uploadedFiles)
{
    private readonly int FirstSegmentIndex = 0;
    public void AddPartyUploadFileInfo(string partyKey,string fileName, int segmentsLength)
    {
        // if there is no repository for partyKey, create it.
        if (!uploadedFiles.TryGetValue(partyKey, out NetworkCacheData<FileSegmentsInfo>? value))
        {
            value = new NetworkCacheData<FileSegmentsInfo>();
            uploadedFiles.Add(partyKey, value);
        }

        value.AddOrUpdate(fileName, new FileSegmentsInfo(segmentsLength));
    }

    public bool UpdateUploadFileData(string partyKey,string fileName, RequestUploadFile req)
    {
        if (!uploadedFiles[partyKey].TryGetValue(fileName, out FileSegmentsInfo? segmentsInfo)) return false;
        
        segmentsInfo?.SetFileBytes(req.Segment);
        
        if (req.Segment.Index != FirstSegmentIndex) return true;
        segmentsInfo?.SetLastBytesLength(req.Segment);
        return true;
    }
    
    public bool TryGetSegmentsInfo(string partyKey, string fileName, out FileSegmentsInfo? segmentsInfo)
    {
        segmentsInfo = null;
        return uploadedFiles.TryGetValue(partyKey, out NetworkCacheData<FileSegmentsInfo>? value) &&
               value.TryGetValue(fileName, out segmentsInfo);
    }
    public void CopyToDownloadFileMap(IDictionary<string, NetworkCacheData<FileSegmentsInfo>> downloadFiles,
            string partyKey,
            string fileName)
    {
        if (!uploadedFiles.ContainsKey(partyKey)) return;
        if(!uploadedFiles[partyKey].ContainsKey(fileName)) return;

        // if there is no repository for partyKey, create it.
        if(!downloadFiles.TryGetValue(partyKey, out NetworkCacheData<FileSegmentsInfo>? value))
        {
            value = new NetworkCacheData<FileSegmentsInfo>();
            downloadFiles.Add(partyKey, value);
        }

        // if there is no repository for fileName, create it.
        if(!value.ContainsKey(fileName))
        {
            value.AddOrUpdate(fileName, new FileSegmentsInfo());
        }

        if (!uploadedFiles[partyKey].TryGetValue(fileName, out FileSegmentsInfo? segmentsInfo)) return;
        // copy
        value.AddOrUpdate(fileName, segmentsInfo);
    }
    
    private void RemoveUploadFiles(string partyKey)
    {
        if (!uploadedFiles.ContainsKey(partyKey)) return;
        uploadedFiles.Remove(partyKey);
    }
    
    public byte[] GetUploadedFile(string partyKey, string fileName)
    {
        if (!uploadedFiles.TryGetValue(partyKey, out NetworkCacheData<FileSegmentsInfo>? cached))
        {
              return ByteNullArray;   
        }

        if (!cached.TryGetValue(fileName, out FileSegmentsInfo? infos)) return ByteNullArray;
        
        List<byte> result = new List<byte>(infos.ByteTotalLength);
        foreach (FileSegment segment in infos.FileBytes)
        {
            result.AddRange(segment.Bytes);
        }

        return result.ToArray();
    }

}