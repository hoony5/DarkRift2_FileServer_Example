using Newtonsoft.Json;
using static SharedValue;

[Serializable]
public class ResponseDeleteFiles : ServerResponseModelBase
{
    [JsonConstructor]
    public ResponseDeleteFiles(ushort clientID, ushort state, string log) : base(clientID, state, log){ }
    public ResponseDeleteFiles() : this(NumericNullValue, NumericNullValue, SuccessResponse){ }
}