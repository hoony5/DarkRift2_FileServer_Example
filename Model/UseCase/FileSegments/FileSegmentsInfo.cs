public class FileSegmentsInfo
{
    private FileSegment[] _fileToBytesData;
    public int SegmentLength { get; private set; }
    public int ByteTotalLength => _fileToBytesData.Sum(segment => segment.Bytes.Length);
    public int LastTransactedSegmentIndex { get; private set; }
    public int Count { get; private set; }
    
    public FileSegment[] FileBytes => _fileToBytesData;


    public FileSegmentsInfo() : this(0)
    {
        _fileToBytesData = new [ ]{ new FileSegment() };
    }
    public FileSegmentsInfo(int segmentLength)
    {
        _fileToBytesData = new FileSegment[segmentLength];
        SegmentLength = segmentLength;
    }
    public void Resize(int length)
    {
        SegmentLength = length;
        Array.Resize(ref _fileToBytesData, length);
    }

    public void SetFileBytes(FileSegment segment)
    {
        _fileToBytesData[segment.Index] = segment;
        Count++;
    }
    
    public void CountUp()
    {
        Count++;
    }

    public bool IsValidatedIndex(int otherIndex)
    {
        if (_fileToBytesData.Length == 0 || _fileToBytesData.Length <= otherIndex)
            return false;

        return _fileToBytesData[otherIndex].Index == otherIndex;
    }
    public void SetLastBytesLength(FileSegment segment)
    {
        LastTransactedSegmentIndex = segment.Index == SegmentLength - 1
            ? segment.Bytes.Length
            : segment.Index;
    }
}