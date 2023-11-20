public class DownloadedFileManagement(IDictionary<string, NetworkCacheData<FileSegmentsInfo>> downloadFiles)
{
    static readonly ushort partitionedSegmentLength = 60000;
    public void RemoveDownloadFiles(string partyKey)
    {
        if (!downloadFiles.ContainsKey(partyKey)) return;
        downloadFiles.Remove(partyKey);
    }
    public bool TryGetDownloadRecord(string partyKey, string fileName, out FileSegmentsInfo? segmentsInfo)
    {
        segmentsInfo = null;
        if (!downloadFiles.ContainsKey(partyKey))
            return false;

        if (!downloadFiles[partyKey].ContainsKey(fileName))
            return false;

        segmentsInfo = downloadFiles[partyKey].TryGetValue(fileName, out FileSegmentsInfo? info)
            ? info
            : segmentsInfo;
        return true;
    }
    public FileSegmentsInfo? SaveDownloadFileRecord(string partyKey, string fileName, byte[]? readAllBytes)
    {
        if (!downloadFiles.TryGetValue(partyKey, out NetworkCacheData<FileSegmentsInfo>? value))
        {
            value = new NetworkCacheData<FileSegmentsInfo>();
            downloadFiles.Add(partyKey, value);
        }

        List<byte[]> segments = PartitionedFileByteArray(readAllBytes);
        if (segments.Count is 0) return new FileSegmentsInfo();

        if(!value.TryGetValue(fileName, out FileSegmentsInfo? cached))
        {
            value.AddOrUpdate(fileName, new FileSegmentsInfo(segments.Count));
        }
        else
        {
            if(segments.Count != cached?.ByteTotalLength)
            {
                cached?.Resize(segments.Count);
            }
        }

        for (int i = 0; i < segments.Count; i ++)
        {
            FileSegment segment = new FileSegment();// fileName
            segment.SetBytes(segments[i]);
            segment.SetBytesIndex(i);
            cached?.SetFileBytes(segment);
        }

        return cached;
    }

    private List<byte[]> PartitionedFileByteArray(byte[]? sourceBytes)
    {
        List<byte[]> result = new List<byte[]>(0);
        if (sourceBytes is null || sourceBytes.Length == 0) return result;
        
        int maxIndex = sourceBytes.Length;
        int lastIndex = maxIndex;
        int segmentCount = maxIndex / partitionedSegmentLength;
        
        for (int i = 0 ; i <= segmentCount; i ++)
        {
            int segmentMinIndex = i * partitionedSegmentLength;
            int segmentMaxIndex = lastIndex > partitionedSegmentLength
                ? i * partitionedSegmentLength + partitionedSegmentLength // not last
                : partitionedSegmentLength * i + maxIndex % partitionedSegmentLength; // last
            int length = segmentMaxIndex - segmentMinIndex;
            byte[] copy = new byte[length];
            Array.ConstrainedCopy(sourceBytes,segmentMinIndex, copy, 0, length);

            lastIndex -= length;

            result.Add(copy);
        }

        return result;
    }
}