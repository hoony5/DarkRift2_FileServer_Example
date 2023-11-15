using System.Text;

public static class Debugger
{
    private static StringBuilder builder = new StringBuilder(200);
    private const int TYPE_PAD = 10;
    private const int NAME_PAD = 22;
    private const string HEADER = "FATAL";
    
    private static void Write(string sender, string message)
    {
        builder.Clear();
        builder.Append("[");
        builder.Append(HEADER);
        builder.Append("]");
        builder.Append(' ', TYPE_PAD - HEADER.Length - 2);
        builder.Append(sender);
        builder.Append(' ', Math.Max(NAME_PAD - sender.Length, 1));

        string[] lines = message.Split('\n');
        builder.Append(lines[0]);
        for (int i = 1; i < lines.Length; i++)
        {
            builder.Append('\n');
            builder.Append(' ', TYPE_PAD + NAME_PAD);
            builder.Append(lines[i]);
        }

        Console.WriteLine(builder);
    }
}
