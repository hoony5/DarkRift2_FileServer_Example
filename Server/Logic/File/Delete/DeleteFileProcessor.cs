// rough code
public class DeleteFileProcessor
{

        public void ProcessDeleteFiles(byte[] decryptedData, MessageReceivedEventArgs e)
        {
            var req = decryptedData.Deserializer<FileData.DeleteFiles>();
            if (req.SenderPartyKey is "-1" or "" or null)
            {
                var cancelRet = new FileData.RequestAccessFilesResult();
                cancelRet.isSuccess = false;
                cancelRet.log = "you need to create or join party.";
                cancelRet.senderClientID = e.Client.ID;
                ClientMessageWriter.SendEncrpytedMessage(e.Client, cancelRet, Tag.Tags.DELETE_FILE_RESULT);
                return;
            }

            var ret = new FileData.DeleteFilesResult();

            ret.log = "Success\n";

            RemoveDatas(req.SenderPartyKey);

            ret.isSuccess = true;

            ClientMessageWriter.SendEncrpytedMessage(e.Client, ret, Tag.Tags.DELETE_ALL_FILE_RESULT);
        }

        public void RemoveDatas(string partyKey)
        {
            // Delete Files.
            DeleteFiles(partyKey, FileDirectoryInfo.tempPath);
            DeleteFiles(partyKey, FileDirectoryInfo.glTFPath);
            DeleteFiles(partyKey, FileDirectoryInfo.MP4Path);
            DeleteFiles(partyKey, FileDirectoryInfo.MOVPath);
            DeleteFiles(partyKey, FileDirectoryInfo.AVIPath);
            DeleteFiles(partyKey, FileDirectoryInfo.FBXPath);
            DeleteFiles(partyKey, FileDirectoryInfo.PDFPath);
            DeleteFiles(partyKey, FileDirectoryInfo.PNGPath);
            DeleteFiles(partyKey, FileDirectoryInfo.JPEGPath);
            // Delete Files Info
            FileLoader.RemoveFiles(LoadType.Upload,partyKey);
            FileLoader.RemoveFiles(LoadType.Download,partyKey);
            NetworkData.RemoveParty(partyKey);
        }
        private void DeleteFiles(string partyKey,string path)
        {
            // Set directory path.
            var dir = Path.Combine(path, partyKey);

            if (!Directory.Exists(dir)) return;

            var dirInfo = new DirectoryInfo(dir);
            var files = dirInfo.GetFiles();

            // Delete files
            if (files.Length != 0)
            {
                foreach (var file in files)
                {
                    file.IsReadOnly = false;
                    file.Attributes = FileAttributes.Normal;
                    file.Delete();
                }
            }
            dirInfo.Attributes = FileAttributes.Normal;
            dirInfo.Delete();
        }
}