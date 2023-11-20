// rough code
public class DownloadFileProcessor
{
      public void ProcessRequestDownloadFile(RequestDownloadAccept? req, MessageReceivedEventArgs e)
      {
          ResponseDownloadAccept res = new ResponseDownloadAccept()
          {
              State = FailedState,
              ClientID = e.Client.ID,
          };
            if (req.PartyKey.Equals(StringNullValue) || string.IsNullOrEmpty(req.PartyKey))
            {
                res.Log = "PartyKey is null or empty.";
                new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_DOWNLOAD_ACCEPT);
                return;
            }
            string fileFullName = $"{req.FileNameWithoutExtension}{req.FileExtension}";
            string finalPath = Path.Combine(FilePathManagement.GetPartyFilePath(req.PartyKey), fileFullName);
            if (!File.Exists(finalPath))
            {
                res.Log = "File is not exist.";
                new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_DOWNLOAD_ACCEPT);
                return;
            }

            byte[] fileBytes = File.ReadAllBytes(finalPath);
            DownloadedFileManagement management = new DownloadedFileManagement(DatabaseCenter.Instance.GetPartyDb().DownloadedFilesMap);
            FileSegmentsInfo? fileSegment = null;
            fileSegment = management.SaveDownloadFileRecord(req.PartyKey, fileFullName, fileBytes);

            if (fileSegment.SegmentLength is 0)
            {
                res.Log = "FileSegment is null.";
                new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_DOWNLOAD_ACCEPT);
                return;
            }

            res = new ResponseDownloadAccept(
                req.FileNameWithoutExtension,
                req.FileExtension,
                fileSegment.LastTransactedSegmentIndex,
                fileSegment.SegmentLength,
                e.Client.ID,
                SuccessState,
                "success")
            {
                State = SuccessState
            };

            new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_DOWNLOAD_ACCEPT);
        }
        public void ProcessDownloadFile(RequestDownloadFile? req, MessageReceivedEventArgs e)
        {

            ResponseDownloadFile res = new ResponseDownloadFile(e.Client.ID, FailedState, string.Empty);
            // Check PartyKey
            if (req.PartyKey.Equals(StringNullValue) || string.IsNullOrEmpty(req.PartyKey))
            {
                res.Log = "PartyKey is null or empty.";
                new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_DOWNLOAD_FILE);
                return;
            }

            string fileFullName = $"{req.FileNameWithoutExtension}{req.FileExtension}";
            DownloadedFileManagement management = new DownloadedFileManagement(DatabaseCenter.Instance.GetPartyDb().DownloadedFilesMap);
            if (management.TryGetDownloadRecord(req.PartyKey, fileFullName, out FileSegmentsInfo? segmentsInfo))
            {
                if (segmentsInfo!.FileBytes.Length is 0)
                {
                   res.Log = "FileByte Length is Zero.";
                   res.Segment = new FileSegment();
                }
                else
                {
                    res.Segment = segmentsInfo.FileBytes[req.FileSegmentIndex];
                    res.State = SuccessState;
                }
            }
            else
            {
                res.Log = "FileSegment is null.";
            }
            new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_DOWNLOAD_FILE);

        }
}