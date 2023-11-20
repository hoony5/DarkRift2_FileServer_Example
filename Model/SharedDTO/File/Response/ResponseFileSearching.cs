using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

[Serializable]
public class ResponseFileSearching : ServerResponseModelBase
{
    [JsonProperty(nameof(FileFullNames))] public string[] FileFullNames { get; private set; }
    [JsonConstructor]
    public ResponseFileSearching(string[] fileFullNames, ushort clientID, ushort state, string log) : base(clientID, state, log)
    {
        FileFullNames = fileFullNames;
    }
    public ResponseFileSearching() : this(new string[0], NumericNullValue, NumericNullValue, SuccessResponse)
    {
    }
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