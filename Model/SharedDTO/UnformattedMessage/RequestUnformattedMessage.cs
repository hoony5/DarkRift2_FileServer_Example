using DarkRift;
using Newtonsoft.Json;
using static DtoValidator;
using static SharedValue;

public class RequestUnformattedMessage : ServerRequestModelBase
{
    private Dictionary<string , string>? _data;
    public IReadOnlyDictionary<string, string> Map => _data;
    [JsonProperty(nameof(Keys))] public string[] Keys { get; private set; }
    [JsonProperty(nameof(Values))] public string[] Values { get; private set; }

    [JsonConstructor]
    public RequestUnformattedMessage(string[] keys, string[] values, string id, ushort state, string log) : base(id, state, log)
    {
        Keys = keys;
        Values = values;
        _data = new Dictionary<string, string>(Keys.Length);
    }

    public RequestUnformattedMessage() : this(StringNullArray, StringNullArray, StringNullValue, NumericNullValue, StringNullValue)
    {
        
    }
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        Keys = e.Reader.ReadStrings();
        Values = e.Reader.ReadStrings();

        if (Keys.Length != Values.Length) return;

        _data = new Dictionary<string, string>(Keys.Length);
        for (int i = 0; i < Keys.Length; i++)
        {
            _data.Add(Keys[i], Values[i]);
        }
    }

    public override void Serialize(SerializeEvent e)
    {
        if(_data is null)
        {
            Console.WriteLine($"{nameof(RequestUnformattedMessage)}'s data is null.");
            return;
        }
        else
        {
            Keys = _data.Keys.ToArray();
            Values = _data.Values.ToArray();
        }
#if DEBUG
        CheckValidationStrings(this, Keys);
        CheckValidationStrings(this, Values);
#endif
        base.Serialize(e);
        e.Writer.Write(Keys);
        e.Writer.Write(Values);
    }
}