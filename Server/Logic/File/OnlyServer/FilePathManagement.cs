using System.Reflection;

public static class FilePathManagement
{
    private static readonly string? basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
    public static string GetSharedFileRootPath()
    {
        return basePath is null 
            ? "There is no shared file path" 
            : Path.Combine(basePath, SharedFileFolderName);
    }
    public static string GetPartyFilePath(string key)
    {
        return basePath is null 
            ? "There is no shared file path" 
            : Path.Combine(basePath, SharedFileFolderName, key);
    }
}