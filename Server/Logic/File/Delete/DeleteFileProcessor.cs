// rough code
public class DeleteFileProcessor
{

        public void ProcessDeleteFiles(RequestDeleteAllFiles? req, MessageReceivedEventArgs e)
        {
            ResponseDeleteFiles res = new ResponseDeleteFiles()
            {
                State = FailedState,
                ClientID = e.Client.ID,
            };
            if (req.SenderPartyKey.Equals(StringNullValue) || string.IsNullOrEmpty(req.SenderPartyKey))
            {
                res.Log = "PartyKey is null or empty.";
                new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_DELETE_ALL_FILES);
                return;
            }
            RemovePartyUploadedFiles(req.SenderPartyKey);
            res.State = SuccessState;
            new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_DELETE_ALL_FILES);
        }

        public void RemovePartyUploadedFiles(string partyKey)
        {
            // Set directory path.
            string partyFilePath = FilePathManagement.GetPartyFilePath(partyKey);

            if (!Directory.Exists(partyFilePath)) return;

            DirectoryInfo dirInfo = new DirectoryInfo(partyFilePath);
            FileInfo[] files = dirInfo.GetFiles();
            // Delete files
            if (files.Length != 0)
            {
                foreach (FileInfo file in files)
                {
                    file.IsReadOnly = false;
                    file.Attributes = FileAttributes.Normal;
                    file.Delete();
                }
            }
            dirInfo.Attributes = FileAttributes.Normal;
            dirInfo.Delete();
            // Delete Files Info
            PartyDatabase partyDb = DatabaseCenter.Instance.GetPartyDb();
            new UploadedFileManagement(partyDb.UploadedFilesMap).RemoveUploadFiles(partyKey);
            new DownloadedFileManagement(partyDb.DownloadedFilesMap).RemoveDownloadFiles(partyKey);
        }
}