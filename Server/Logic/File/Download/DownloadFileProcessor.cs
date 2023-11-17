// rough code
public class DownloadFileProcessor
{
      public static void ProcessRequestDownloadFile(byte[] decryptedData, MessageReceivedEventArgs e)
        {
            var req = decryptedData.Deserializer<FileData.RequestDownloadFile>();
            var currentPartyKey = GetPartyByClient(e.Client.ID).connectionKey;
            Console.WriteLine($"{e.Client is null} What is null req ? { req is null} | currentPartyKey ? {string.IsNullOrEmpty(currentPartyKey)}");
            var ret = new RequestDownloadFileResult();
            Console.WriteLine($"Process Req DownloadFile :: {req.FileName} | {req.FileExtension}");
            ret.FileName = req.FileName;
            ret.FileExtension = req.FileExtension;
            ret.log = "Success";
            ret.isSuccess = currentPartyKey != "-1" && !string.IsNullOrEmpty(currentPartyKey) && !req.FileName.Contains("undecrypted_");

            if (!ret.isSuccess)
            {
                ret.log = "Failed.. current Partykey wrong";
                ClientMessageWriter.SendEncrpytedMessage(e.Client, ret, Tag.Tags.REQUSET_DOWNLOAD_FILE_RESULT);
                return;
            }

            var fileFullName = $"{ret.FileName}{Util.GetExtension(ret.FileExtension)}";
            var downloadPath = Combine(FileDirectoryInfo.GetPathByFileExtension(req.FileExtension), currentPartyKey);
            var finalPath = Combine(downloadPath
                , req.FileExtension is Tag.FileExtension.Word
                    or Tag.FileExtension.PPT
                    or Tag.FileExtension.PDF
                    or Tag.FileExtension.CSV ?
                    $"{req.FileName}.pdf" : fileFullName);

            Console.WriteLine($"finalPath : {finalPath}");
            FileSegmentsInfo fileSegment = null;
            var needRequestReload = req.FileExtension is Tag.FileExtension.Word or Tag.FileExtension.PPT || FileLoader.ExistDownloadRecords(currentPartyKey, req.FileName, req.FileExtension);
            if(needRequestReload)
            {
                var existFileOnPath = Exists(finalPath);

                if (existFileOnPath)
                {
                    var fileBytes = ReadAllBytes(finalPath);
                    fileSegment =
                        FileLoader.ProcessDownloadFile(currentPartyKey, req.FileName, req.FileExtension, fileBytes);
                    Console.WriteLine($"fileSegment is null ? {fileSegment is null} | Count ? {fileSegment?.count}");
                }
                else
                {
                    ret.log = "There is no Path.";
                    ret.isSuccess = false;
                }
            }
            else
                fileSegment = FileLoader.GetDownloadRecord(currentPartyKey, req.FileName, req.FileExtension);

            FileServerPlugin.Debug($" last length - {fileSegment?.lastBytesLength} | index - {fileSegment?.dataTotalLength - 1}");
            ret.LastDataBytesLength = fileSegment.lastBytesLength;
            ret.LastDataIndex = fileSegment.dataTotalLength - 1;

            ret.isSuccess = true;
            ret.senderClientID = e.Client.ID;

            if (fileSegment.fileToBytesData.Length == 0)
            {
                ret.log = "There is no File.";
                ret.isSuccess = false;
            }

            if (!fileSegment.IsCorrectByteIndex(0))
            {
                ret.log +=
                    $"Requesting index is not correct.| req : {0} / server : {fileSegment.fileToBytesData[0].byteArray.index} |";
                ret.isSuccess = false;
            }

            ClientMessageWriter.SendEncrpytedMessage(e.Client, ret, Tag.Tags.REQUSET_DOWNLOAD_FILE_RESULT);
        }
        public static void ProcessDownloadFile(byte[] decryptedData, MessageReceivedEventArgs e)
        {
            var currentPartyKey = GetPartyByClient(e.Client.ID).connectionKey;
            // Check PartyKey
            if (currentPartyKey == "-1" || string.IsNullOrEmpty(currentPartyKey))
            {
                var cancelRet = new RequestAccessFilesResult();
                cancelRet.isSuccess = false;
                cancelRet.log = "you need to create or join party.";
                cancelRet.senderClientID = e.Client.ID;
                ClientMessageWriter.SendEncrpytedMessage(e.Client, cancelRet, Tag.Tags.DOWNLOAD_FILE_RESULT);
                return;
            }

            // if Accept
            var req = decryptedData.Deserializer<FileData.DownloadFile>();
            var ret = new DownloadFileResult();
            ret.senderClientID = e.Client.ID;
            ret.log = "Success !!\n";

            FileSegment[] fileSegments;
            var existDownloadRecord = FileLoader.ExistDownloadRecords(currentPartyKey, req.FileName, req.FileExtension);
            FileServerPlugin.Debug($"Reqest Download :: {req.FileName}{req.FileExtension}");
            if (existDownloadRecord)
            {
                fileSegments =  FileLoader.GetDownloadRecord(currentPartyKey, req.FileName, req.FileExtension).fileToBytesData;

                if (fileSegments.Length == 0)
                {
                    ret.isSuccess = false;
                    ret.log += "fileSegments Count is Zero.";
                    FileServerPlugin.Debug($"{ret.log}");
                    ret.Data = new FileSegment();
                }
                else
                {
                    ret.isSuccess = true;
                    ret.log = fileSegments[req.FileSegmentIndex].byteArray.index != req.FileSegmentIndex
                        ? "file index is worng."
                        : ret.log;
                    ret.Data = fileSegments[req.FileSegmentIndex];

                    FileServerPlugin.Debug(
                        $"File Segments Index {ret.Data.byteArray.index} / {fileSegments.Length - 1}");
                }
                ClientMessageWriter.SendEncrpytedMessage(e.Client, ret, Tag.Tags.DOWNLOAD_FILE_RESULT);

                SaveLog(currentPartyKey, AccessType.Download, ret.Data, ret.isSuccess ? "Success" : "Failed");

            }
            else
            {
                ret.log = $"There is no record...";
                ret.isSuccess = false;
                ret.Data = new FileSegment();
                ClientMessageWriter.SendEncrpytedMessage(e.Client, ret, Tag.Tags.DOWNLOAD_FILE_RESULT);
                FileServerPlugin.Debug($"{ret.log}");
            }
        }
}