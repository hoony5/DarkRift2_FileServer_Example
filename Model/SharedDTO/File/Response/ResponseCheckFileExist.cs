using Newtonsoft.Json;
using static SharedValue;

[Serializable]
public class ResponseCheckFileExist : ServerResponseModelBase
{
    [JsonConstructor]
    public ResponseCheckFileExist(ushort clientID, ushort state, string log) : base(clientID, state, log){ }
    public ResponseCheckFileExist() : this(NumericNullValue, NumericNullValue, SuccessResponse){ }
}