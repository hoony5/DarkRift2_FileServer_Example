﻿using System.Reflection;
public class DirectoryCreator
{
    public string MakeOrGetloadFileDirectory(string key)
    {
        string finalPath 
            = Path.Combine(
                FilePathManagement.GetSharedFilePath(),
                SharedFileFolderName,key);
        
        if (Directory.Exists(finalPath)) return finalPath;
        _ = Directory.CreateDirectory(finalPath);
        return finalPath;
    }
}