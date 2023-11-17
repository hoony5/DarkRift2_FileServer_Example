public class UploadFileProcessor
{
    // if file name contains '.' one or more, it will be error.
    // blocking dot, when send message on the unity. 
    public void ProcessRequestUploadFile(RequestUploadAccept req, MessageReceivedEventArgs e)
    {
        ResponseUploadAccept res;

        if (req.SenderPartyKey.Equals(StringNullValue))
        {
            res = new ResponseUploadAccept(StringNullValue, StringNullValue, e.Client.ID);
            res.State = 0;
            res.Log = "you need to create or join party.";

            _ = new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_UPLOAD_ACCEPT);
            return;
        }

        UploadedFileManagement fileManagement
            = new UploadedFileManagement(DatabaseCenter.Instance.GetPartyDb().UploadedFilesMap);

        // Create Repository by using user partyKey
        fileManagement.AddPartyUploadFileInfo(
            partyKey: req.SenderPartyKey,
            fileName: $"{req.FileNameWithoutExtension}{req.FileExtension}",
            segmentsLength: req.LastSegmentIndex + 1);

        res = new ResponseUploadAccept(
            fileNameWithoutExtension: req.FileNameWithoutExtension,
            fileExtension: req.FileExtension,
            clientID: e.Client.ID);
        res.State = 1;

        _ = new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_UPLOAD_ACCEPT);

    }

    private static bool IsOfficeFile(string fileName)
    {
        return fileName.Contains(WordExtension) ||
               fileName.Contains(WordExtension2) ||
               fileName.Contains(PowerPointExtension) ||
               fileName.Contains(PowerPointExtension2) ||
               fileName.Contains(ExcelExtension);
    }

    private void ConvertByteToFile(byte[] totalByteArray,
        string finalPath,
        UploadedFileManagement fileManagement,
        string onlyFileName,
        string uploadPath,
        RequestUploadFile req,
        ResponseUploadFile res,
        IClient toRequester)
    {
        // Save Bytes to File.
        Task.Run(async () => await WriteAllBytesAsync(finalPath, totalByteArray));
        res.State = 1;

        // Copy to Download Record
        fileManagement.CopyToDownloadFileMap(
            DatabaseCenter.Instance.GetPartyDb().DownloadedFilesMap,
            req.PartyKey,
            onlyFileName);
        
        if (IsOfficeFile(onlyFileName))
        {
            string saveAsPdfPath = Path.Combine(uploadPath,
                Path.GetFileNameWithoutExtension(onlyFileName),
                PdfExtension);
                    
            ConvertToPdf(finalPath, saveAsPdfPath, req.Segment.FileNameWithoutExtension, req.Segment.FileExtension);
        }

        AdvertiseUploadCompletionToParty(onlyFileName, req, res, toRequester);
                
        FileServer.DebugLog($"Upload File Success :: {req.Segment.FileNameWithoutExtension}{req.Segment.FileExtension}");
    }

    private static void AdvertiseUploadCompletionToParty( string onlyFileName, RequestUploadFile req, ResponseUploadFile res, IClient toRequester)
    {
        if (DatabaseCenter.Instance.GetPartyDb().PartyMap.TryGetValue(req.PartyKey, out Party? party))
        {
            RequestAdvertiseUploadCompletion alarm =
                new RequestAdvertiseUploadCompletion(onlyFileName, req.AccountID);
                    
            _ = new ServerEncryptedDtoWriter().SendMessageToParty(party, alarm, Tags.REQUEST_ADVERTISE_UPLOAD_COMPLETION);
        }
        else
        {
            _ = new ServerEncryptedDtoWriter().SendMessage(toRequester, res, Tags.RESPONSE_UPLOAD_FILE);
                    
        }
    }

    public void ProcessUploadFile(RequestUploadFile req, MessageReceivedEventArgs e)
    {
        ResponseUploadFile res;
        if (req.PartyKey.Equals(StringNullValue))
        {
            res = new ResponseUploadFile(StringNullValue, StringNullValue, 0, e.Client.ID);
            res.State = 0;
            res.Log = "you need to create or join party.";
            _ = new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_UPLOAD_FILE);
            return;
        }

        string onlyFileName = $"{req.Segment.FileNameWithoutExtension}{req.Segment.FileExtension}";

        res = new ResponseUploadFile(
            fileNameWithoutExtension: req.Segment.FileNameWithoutExtension,
            fileExtension: req.Segment.FileExtension,
            uploadSegmentIndex: req.Segment.Index,
            clientID: e.Client.ID);
        res.State = 1;

        // Save On Server Computer
        UploadedFileManagement fileManagement
            = new UploadedFileManagement(DatabaseCenter.Instance.GetPartyDb().UploadedFilesMap);

        if (!fileManagement.TryGetSegmentsInfo(req.PartyKey, onlyFileName, out FileSegmentsInfo? segmentsInfo)) return;
        string uploadPath = new DirectoryCreator().MakeOrGetloadFileDirectory(req.PartyKey);

        if (segmentsInfo?.Count == segmentsInfo?.FileBytes.Length)
        {
            _ = new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_UPLOAD_FILE);
        }
        else
        {
            byte[] totalByteArray = fileManagement.GetUploadedFile(req.PartyKey, onlyFileName);
            string finalPath = Path.Combine(uploadPath, onlyFileName);

            ConvertByteToFile(
                totalByteArray,
                finalPath,
                fileManagement,
                onlyFileName,
                uploadPath,
                req,
                res,
                e.Client);
        }
    }

    private static async Task WriteAllBytesAsync(string finalPath, byte[] totalByteArray)
        {
            using (FileStream sourceStream = new FileStream(finalPath,
                       FileMode.Create, FileAccess.Write, FileShare.None,
                       bufferSize: 4096 * 8, useAsync: true))
            {
                Task task = sourceStream.WriteAsync(totalByteArray, 0, totalByteArray.Length);
                await task;
            }
        }
        private void ConvertToPdf(string officeFilePath, string saveAsPdfPath, string fileNameWithoutExtension, string fileExtension)
        {
            if (!IsOfficeFile($"{fileNameWithoutExtension}{fileExtension}")) return;
            PDFConverter converter = new PDFConverter();
            converter.Convert( fileExtension, officeFilePath, saveAsPdfPath);
        }
}