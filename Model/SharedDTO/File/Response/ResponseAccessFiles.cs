using Newtonsoft.Json;
using static SharedValue;

    [Serializable]
    public class ResponseAccessFiles : ServerResponseModelBase
    {
        [JsonConstructor]
        public ResponseAccessFiles(ushort clientID, ushort state, string log) : base(clientID, state, log)
        {
        }

        public ResponseAccessFiles() : this(NumericNullValue, NumericNullValue, SuccessResponse)
        {
        }
    }