// rough code
public class FileFinder
{
     private List<FileSegment> GetUploadedFiles(string path)
        {
            List<FileSegment> infos = new List<FileSegment>();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            for (int index = 0; index < files.Length; index++)
            {
                FileInfo current = files[index];
                FileSegment segment = new FileSegment();
                segment.SetFileNameWithoutExtension(current.Name);
                segment.SetFileExtension(current.Extension); 
                infos.Add(segment);
            }
            return infos;
        }
        public void ProcessSearchFile(RequestFileSearching? req, MessageReceivedEventArgs e)
        {
            ResponseFileSearching res;
            if (req.SenderPartyKey.Equals(StringNullValue))
            {
                res = new ResponseFileSearching(
                    StringNullArray,
                    e.Client.ID,
                    FailedState,
                    "you need to create or join party.");

                _ = new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_FILE_SEARCHING);
                return;
            }
            // return Search List
            string filePath = FilePathManagement.GetPartyFilePath(req.SenderPartyKey);
            FileInfo[] uploadedFileList = new DirectoryInfo(filePath).GetFiles();
            string[] fileNames = new string[uploadedFileList.Length];
            
            for (var index = 0; index < uploadedFileList.Length; index++)
            {
                FileInfo current = uploadedFileList[index];
                fileNames[index] = current.Name;
            }
            res = new ResponseFileSearching(
                fileNames,
                e.Client.ID,
                SuccessState,
                "success");

            _ = new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_FILE_SEARCHING);
        }
}