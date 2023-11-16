// rough code
public class DownloadedFileManagement
{
    private readonly Dictionary<string, NetworkCacheData<FileSegmentsInfo>> _downloadFiles;
    static readonly ushort maxSegmentLength = 65000;

    public bool ExistDownloadRecord(string partyKey, string fileName)
    {
        if (!_downloadFiles.ContainsKey(partyKey))
            return false;

        return _downloadFiles[partyKey].ContainsKey(fileName);
    }

    private void RemoveDownloadFiles(string partyKey)
    {
        if (!_downloadFiles.ContainsKey(partyKey)) return;
        _downloadFiles.Remove(partyKey);
    }
    public FileSegmentsInfo? GetDownloadRecord(string partyKey, string fileName)
    {
        if (!_downloadFiles.ContainsKey(partyKey))
            return new FileSegmentsInfo();

        Console.WriteLine($"Get Download Record : {fileName}");
        if(!_downloadFiles[partyKey].ContainsKey(fileName))
            return new FileSegmentsInfo();

        if(_downloadFiles[partyKey].TryGetValue(fileName, out FileSegmentsInfo? record))
        {
            record?.CountUp();

            return record;
        }

        return null;
    }
    public FileSegmentsInfo ProcessDownloadFile(string partyKey, string fileName, byte[] readAllBytes)
    {

        if(!_downloadFiles.ContainsKey(partyKey))
            _downloadFiles.Add(partyKey , new Dictionary<string, FileSegmentsInfo>(0));

        var segments = DivideArrayToSkipIndex(readAllBytes);
        if (segments.Count == 0) return new FileSegmentsInfo();

        if (!_downloadFiles[partyKey].ContainsKey(fileName))
            _downloadFiles[partyKey].Add(fileName, new FileSegmentsInfo(segments.Count));

        if(segments.Count != _downloadFiles[partyKey][fileName].dataTotalLength)
            _downloadFiles[partyKey][fileName].Resize(segments.Count);

        for (int i = 0; i < segments.Count; i ++)
        {
            var segment = new FileSegment()// fileName
                .SetBytes(segments[i])
                .SetBytesIndex(i);

            _downloadFiles[partyKey][fileName] = _downloadFiles[partyKey][fileName].SetFileBytes(segment);
            FileServerPlugin.Debug($"segment_{i} / {segments.Count - 1}");
        }

        return _downloadFiles[partyKey][fileName];
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

    private List<byte[]> DivideArrayToSkipIndex(byte[] sourceBytes)
    {
        var ret = new List<byte[]>(0);
        if (sourceBytes == null || sourceBytes.Length == 0)
            return ret;
        var maxIndex = sourceBytes.Length;
        var lastIndex = maxIndex;
        for (int i = 0 ; i <= maxIndex / maxSegmentLength; i ++)
        {
            // divdie
            int segmentMinIndex = i * maxSegmentLength;
            int segmentMaxIndex = lastIndex > maxSegmentLength
                ? i * maxSegmentLength + maxSegmentLength // not last
                : maxSegmentLength * i + maxIndex % maxSegmentLength; // last
            int length = segmentMaxIndex - segmentMinIndex;
            byte[] copy = new byte[length];
            Array.ConstrainedCopy(sourceBytes,segmentMinIndex, copy, 0, length);

            lastIndex -= length;

            ret.Add(copy);
            /*FileServerPlugin.Debug($"Work Divide :: {i} / {maxIndex / maxSegmentLength}");
            if(i == maxIndex / maxSegmentLength)
                FileServerPlugin.Debug($"Debug :: lastIndex is {lastIndex} | segmentMinIndex : {segmentMinIndex} | sementMaxIndex : {segmentMaxIndex} | source Length {sourceBytes.Length} | currentMultiplierIndex {i}");*/
        }

        return ret;
    }
}