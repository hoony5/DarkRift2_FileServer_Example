public class DirectoryCreator
{
    public void CreateDirectory(string newDirectory)
    {
        if(!Directory.Exists(newDirectory))
            Directory.CreateDirectory(newDirectory);
    }
}