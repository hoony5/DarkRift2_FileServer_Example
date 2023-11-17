public static class CommonValue
{
    public static string SuccessResponse => "* Success *";
    public static string StringNullValue => "N/A";
    public static ushort NumericNullValue => 0;
    public static string[] StringNullArray => new [] { "N/A" };
    public static byte[] ByteNullArray => new byte[] { 0 };
    public static string SharedFileFolderName => "Files";
    public static string ConfigPath => Path.Combine("Config", "Config.txt"); 
    public static string SecurityPath => Path.Combine("Config", "Security.txt");
    public static char DashSignature => '-';
    public static char EqualSignature => '=';
    public static char ColonSignature => ':';
    public static char DotSignature => '.';
    public static string PdfExtension => ".pdf";
    public static string ExcelExtension => ".xlsx";
    public static string WordExtension => ".docx";
    public static string WordExtension2 => ".doc";
    public static string PowerPointExtension => ".pptx";
    public static string PowerPointExtension2 => ".ppt";
    public static char LineSignature => '\n';
    public static char SpaceChar => ' ';
    public static int HeaderIndex => 0;
    public static int CommandKeywordIndex => 1;

    public static int ConcurrencyLevel = Environment.ProcessorCount * 2;
}