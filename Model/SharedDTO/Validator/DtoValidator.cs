using static SharedValue;

using System.Text;
public static class DtoValidator
{
    private const int TYPE_PAD = 10;
    private const int NAME_PAD = 22;
    private const string HEADER = "FATAL";
    private static StringBuilder builder = new StringBuilder(200);
    
    private static void Write(string sender, string message)
    {
        builder.Clear();
        builder.Append("[");
        builder.Append(HEADER);
        builder.Append("]");
        builder.Append(SpaceChar, TYPE_PAD - HEADER.Length - 2);
        builder.Append(sender);
        builder.Append(SpaceChar, Math.Max(NAME_PAD - sender.Length, 1));

        string[] lines = message.Split(LineSignature);
        builder.Append(lines[0]);
        for (int i = 1; i < lines.Length; i++)
        {
            builder.Append(LineSignature);
            builder.Append(SpaceChar, TYPE_PAD + NAME_PAD);
            builder.Append(lines[i]);
        }

        Console.WriteLine(builder);
    }
    public static void CheckValidation<TDto>(bool isValidate, string message)
    {
        if (isValidate) return;
        Write(typeof(TDto).Name, message);
    }
    public static void CheckValidationString<TDto>(TDto dto, string value)
    {
        if (!string.IsNullOrEmpty(value)) return;
        Write(dto.GetType().Name, "string is null or Empty");
    }
    public static void CheckValidationStrings<TDto>(TDto dto, string?[] value)
    {
        string? issue = Array.Find(value, string.IsNullOrEmpty);
        if (value.Length != 0 && string.IsNullOrEmpty(issue)) return;
        Write(dto.GetType().Name, "string is null or Empty");

        builder.Clear();
        
        foreach (string str in value)
        {
            if(string.IsNullOrEmpty(str)) continue;
            builder.Append(str);
            builder.Append(',');
            builder.AppendLine();
        }

        Console.WriteLine(builder);
    }
    public static void CheckValidationProperty<TDto, KProperty>(TDto dto, KProperty value)
    {
        if (value is not null) return;
        Write(dto.GetType().Name, $"{typeof(KProperty).Name}({nameof(value)}) : Null");
    }
    public static void CheckValidationProperty<TDto, KProperty>(TDto dto, KProperty[] value)
    {
        if (value.Length != 0 && Array.FindIndex(value, item => item is null) == -1) return;
        Write(dto.GetType().Name, $"{typeof(KProperty).Name}({nameof(value)}) : Null");
        
        foreach (KProperty property in value)
        {
            if(property is null) continue;
            builder.Append(nameof(property));
            builder.Append(',');
            builder.AppendLine();
        }
        Console.WriteLine(builder);
    }
}
