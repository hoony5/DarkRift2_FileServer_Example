public class FileSegmentsInfo(int length = 100)
{
    private FileSegment[] _fileToBytesData = new FileSegment[length];
    // filToBytesData's Length
    public int DataTotalLength { get; private set; } = length;

    // filToBytesData[^1]'s byte[]'s Length
    public int LastBytesLength { get; private set; }
    public int Count { get; private set; }

    public void Resize(int length)
    {
        Count = 0;
        DataTotalLength = length;
        Array.Resize(ref _fileToBytesData, length);
    }

    public void SetFileBytes(FileSegment data)
    {
        _fileToBytesData[data.Partition.Index] = data;
        Count++;
    }

    public void CountUp() => Count++;

    public bool IsValidatedIndex(int otherIndex)
    {
        if (_fileToBytesData.Length == 0 || _fileToBytesData.Length <= otherIndex)
            return false;

        return _fileToBytesData[otherIndex].Partition.Index == otherIndex;
    }
    public void SetLastBytesLength(FileSegment data)
    {
        LastBytesLength = data.Partition.Index == DataTotalLength - 1
            ? data.Partition.Bytes.Length
            : data.Partition.Index;
    }
}