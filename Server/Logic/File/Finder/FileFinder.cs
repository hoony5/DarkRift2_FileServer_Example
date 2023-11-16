// rough code
public class FileFinder
{
     private List<FileSegment> FindLocalFiles(string folder)
        {
            var infos = new List<ClientServerModel.FileSegment>();
            var directoryInfo = new DirectoryInfo(folder);
            var files = directoryInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                var current = files[i];
                infos.Add(new ClientServerModel.FileSegment()
                    .SetName(current.Name)
                    .SetExtensionType(Util.GetFileType(current.Extension)));
            }

            return infos;
        }
        public void ProcessSearchFile(byte[] decryptedData, MessageReceivedEventArgs e)
        {
            var req = decryptedData.Deserializer<FileData.SearchFile>();
            if (req.SenderPartyKey is "-1" or "" or null)
            {
                var cancelRet = new FileData.RequestAccessFilesResult();
                cancelRet.isSuccess = false;
                cancelRet.log = "you need to create or join party.";
                cancelRet.senderClientID = e.Client.ID;
                ClientMessageWriter.SendEncrpytedMessage(e.Client, cancelRet, Tag.Tags.SEARCH_FILES_RESULT);
                return;
            }

            // return Search List
            var ret = new FileData.SearchFileResult();
            var partyFilesRootPath = Path.Combine(FileDirectoryInfo.tempPath,req.SenderPartyKey);
            var existDirectory = Directory.Exists(partyFilesRootPath);

            if (!existDirectory)
            {
                ret.isSuccess = false;

                ret.log = $"Failed => There are no files\n";
                FileServerPlugin.Debug($"Failed => There are no files :: Check FileServer {partyFilesRootPath}");
                ret.FileFullNames = new string[1] { "-1" };
            }
            else
            {
                var infos = new DirectoryInfo(partyFilesRootPath).GetFiles();

                // if (fileName.Contains("undecrypted_")) continue;
                infos = infos.Select(x => x).Where(x => !x.Name.Contains("undecrypted_")).ToArray();
                ret.isSuccess = true;
                ret.log = "Success\n";
                ret.senderClientID = req.SenderClientID;
                ret.FileFullNames = new string[infos.Length];
                for (var i = 0; i < infos.Length; i ++)
                {
                    var current = infos[i];
                    ret.FileFullNames[i] = current.Name;
                }
            }

            NetworkData.SaveLog(req.SenderPartyKey, AccessType.Search, ret.isSuccess ? "Success" : "Failed");
            ClientMessageWriter.SendEncrpytedMessage(e.Client, ret, Tag.Tags.SEARCH_FILES_RESULT);
        }
        public void ProcessCheckFileExsist(byte[] decryptedData, MessageReceivedEventArgs e)
        {
            var req = decryptedData.Deserializer<FileData.CheckFileExist>();
            if (req.SenderPartyKey is "-1" or "" or null)
            {
                var cancelRet = new FileData.RequestAccessFilesResult();
                cancelRet.isSuccess = false;
                cancelRet.log = "you need to create or join party.";
                cancelRet.senderClientID = e.Client.ID;
                ClientMessageWriter.SendEncrpytedMessage(e.Client, cancelRet, Tag.Tags.CHECK_FILE_EXSIST_RESULT);
                return;
            }
            req.SenderClientID = e.Client.ID;

            var ret = new FileData.CheckFileExistResult();
            // Find Exist File
            var partyFilesRootPath = Path.Combine(FileDirectoryInfo.tempPath,req.SenderPartyKey);
            var existDirectory = Directory.Exists(partyFilesRootPath);

            ret.log = "Success\n";
            ret.senderClientID = req.SenderClientID;
            ret.isSuccess = existDirectory;
            if(existDirectory)
            {
                var directoryInfo = new DirectoryInfo(partyFilesRootPath);
                var files = directoryInfo.GetFiles();
                var fileInfo = System.Array.Find(files,
                    i => i.Name.Contains(req.FileName) && i.Extension.Contains(Util.GetExtension(req.FileType)));

                ret.isSuccess = fileInfo != null;
                ret.log = ret.isSuccess ? "Success\n" : $"fileInfo is wrong. :: {req.FileName}_{req.FileType}";
            }

            NetworkData.SaveLog(req.SenderPartyKey, AccessType.Search,  ret.log);
            ClientMessageWriter.SendEncrpytedMessage(e.Client, ret, Tag.Tags.CHECK_FILE_EXSIST_RESULT);
        }
}