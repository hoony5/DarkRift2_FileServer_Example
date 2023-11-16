using System.Reflection;
public class DirectoryCreator
{
    public void CreateDirectoryOnlyForParty(string key)
    {
        string? consolePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        string finalPath = Path.Combine(consolePath, SharedFileFolderName,key);
        if (Directory.Exists(finalPath)) return;
        Directory.CreateDirectory(finalPath);
    }
}