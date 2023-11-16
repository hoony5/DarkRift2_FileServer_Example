public static class CommonValue
{
    public static string SuccessResponse => "* Success *";
    public static string PreventExceptionStringValue => "N/A";
    public static ushort PreventExceptionNumericValue => 0;
    public static string[] PreventExceptionStringArray => new [] { "N/A" };
    public static byte[] PreventExceptionByteArray => new byte[] { 0 };
    public static string SharedFileFolderName => "Files";
    public static string ConfigPath => Path.Combine("Config", "Config.txt"); 
    public static string SecurityPath => Path.Combine("Config", "Security.txt");
    public static char DashSignature => '-';
    public static char EqualSignature => '=';
    public static char ColonSignature => ':';
    public static char LineSignature => '\n';
    public static char SpaceChar => ' ';
    public static int HeaderIndex => 0;
    public static int CommandKeywordIndex => 1;
}