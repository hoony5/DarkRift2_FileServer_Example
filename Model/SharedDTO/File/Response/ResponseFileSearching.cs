[Serializable]
[method: JsonConstructor]
public class ResponseFileSearching(string[] fileFullNames, ushort clientID) : ServerResponseModelBase(clientID)
{
    [JsonProperty(nameof(FileFullNames))] public string[] FileFullNames { get; private set; } = fileFullNames;
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        FileFullNames = e.Reader.ReadStrings();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
#if DEBUG
        CheckValidationStrings(this, FileFullNames);
#endif
        e.Writer.Write(FileFullNames);
    }
}