// rough code
public class UploadFileProcessor
{
     public static void ProcessRequestUploadFile(byte[] decryptedData, MessageReceivedEventArgs e)
        {
            var req = decryptedData.Deserializer<FileData.RequestUploadFile>();

            var ret = new RequestUploadFileResult();

            if (req.SenderPartyKey is "-1" or "" or null)
            {
                ret.FileName = "-1";
                ret.FileType = Tag.FileType.Word;
                ret.log = "You need join the Party";
                ret.isSuccess = false;
                ret.senderClientID = e.Client.ID;

                ClientMessageWriter.SendEncrpytedMessage(e.Client, ret,
                    Tag.Tags.REQUEST_UPLOAD_FILE_RESULT);

                return;
            }

            ret.FileName = req.FileName;
            ret.FileType = req.FileType;
            ret.log = "Success\n";
            ret.isSuccess = true;
            ret.senderClientID = req.SenderClientID;
            FileServerPlugin.Debug($"req.LastDataIndex + 1  {req.LastDataIndex + 1}");

            FileLoader.AddUploadList(req.SenderPartyKey, $"{req.FileName}{Util.GetExtension(req.FileType)}", req.LastDataIndex + 1);
            ClientMessageWriter.SendEncrpytedMessage(e.Client, ret, Tag.Tags.REQUEST_UPLOAD_FILE_RESULT);
        }
        public static void ProcessUploadFile(byte[] decryptedData,byte[] originalData, MessageReceivedEventArgs e)
        {
            var currentPartyKey = NetworkData.GetPartyByClient(e.Client.ID).connectionKey;
            // Check Access Auth
            if (currentPartyKey is "-1" or "" or null)
            {
                var cancelRet = new RequestAccessFilesResult();
                cancelRet.isSuccess = false;
                cancelRet.log = "you need to create or join party.";
                cancelRet.senderClientID = e.Client.ID;
                ClientMessageWriter.SendEncrpytedMessage(e.Client, cancelRet, Tag.Tags.UPLOAD_FILE_RESULT);
                return;
            }

            // if access , accept File Data.
            var req = decryptedData.Deserializer<FileData.UploadFile>();
            var fileFullName = $"{req.Data.fileName}{Util.GetExtension(req.Data.type)}";

            // To Client
            var ret = new UploadFileResult
            {
                senderClientID = e.Client.ID,
                isSuccess = FileLoader.UpdateUploadFileList(currentPartyKey, fileFullName,req),
                log = "Success\n",
                FileName = req.Data.fileName,
                FileType = req.Data.type,
                UploadedDataIndex = req.Data.byteArray.index
            };

            if (!ret.isSuccess)
                ret.log = "There is no file or no your partyKey validation.";

            // Save On Server Computer
            var savePosition = FileLoader.GetUploadFileSegment(currentPartyKey, fileFullName);
            FileServerPlugin.Debug($"Recv Bytes :: {req.Data.byteArray.index} / {savePosition.dataTotalLength - 1}");

            // Complete
            if(FileLoader.GetUploadFileSegment(currentPartyKey, fileFullName).count == savePosition.fileToBytesData.Length)
            {
                // Create Directory
                var uploadPath = Path.Combine(FileDirectoryInfo.tempPath, currentPartyKey);
                NetworkData.CreateDirectory(uploadPath);
                // Save Bytes to File.
                (string name, byte[] datas) file = FileLoader.GetUploadedFileBytes(currentPartyKey, req.Data.fileName, req.Data.type);
                var undecryptedFileFullName = $"undecrypted_{req.Data.fileName}{Util.GetExtension(req.Data.type)}";
                (string name, byte[] datas) undecryptedFile = (undecryptedFileFullName, originalData);


                string finalPath = Path.Combine(uploadPath, file.name);
                string undecryptedFinalPath = Path.Combine(uploadPath, undecryptedFile.name);

                Console.WriteLine(finalPath);
                Console.WriteLine(undecryptedFinalPath);

                Task task = WirteAllBytesAsync(finalPath, file.datas);
                Task task2 = WirteAllBytesAsync(undecryptedFinalPath, undecryptedFile.datas);

                string uploadCopyPath = Path.Combine(FileDirectoryInfo.GetPathByFileType(req.Data.type), currentPartyKey);
                NetworkData.CreateDirectory(uploadCopyPath);
                string finalCopyPath = Path.Combine(uploadCopyPath, file.name);
                string finalUnDecryptedFileCopyPath = Path.Combine(uploadCopyPath, undecryptedFile.name);

                if (File.Exists(finalCopyPath))
                {
                    FileInfo info = new FileInfo(finalCopyPath);
                    info.IsReadOnly = false;
                    info.Attributes = FileAttributes.Normal;
                    info.Delete();
                    FileInfo otherInfo = new FileInfo(finalUnDecryptedFileCopyPath);
                    otherInfo.IsReadOnly = false;
                    otherInfo.Attributes = FileAttributes.Normal;
                    otherInfo.Delete();
                }

                // copy to other directory
                Thread.Sleep(1000);
                if(req.Data.type is not Tag.FileType.Bin)
                {
                    File.Copy(finalPath, finalCopyPath, true);
                    File.Copy(undecryptedFinalPath, finalUnDecryptedFileCopyPath, true);
                }
                ret.isSuccess = File.Exists(finalPath);
                ret.log = ret.isSuccess
                    ? "Success\n"
                    : $"Failed :: Check => [Upload Folder : {uploadPath}] | [Upload File FullName : {file.name}]";

                // for test
                if (finalPath.Contains(".txt"))
                {
                    var encryptedContent = File.ReadAllText(finalPath);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($@"::: {finalPath}'s File Content :::
{encryptedContent}");
                    Console.ForegroundColor = ConsoleColor.White;

                    File.WriteAllText(finalPath, Encoding.UTF8.GetString(AesAndRSAWrapper.AESDecrypt(Convert.FromBase64String(encryptedContent))));
                }
                string partyPDFsPath = Path.Combine(FileDirectoryInfo.PDFPath, currentPartyKey);
                // transform to word , ppt
                WhenOfficeToPDF(finalPath, partyPDFsPath, req.Data.fileName, req.Data.type);
                WhenOfficeToPDF(undecryptedFinalPath, partyPDFsPath, $"undecrypted_{req.Data.fileName}", req.Data.type);

                // Copy to Download Record
                FileLoader.CopyUploadToDownload(currentPartyKey, fileFullName);

                AdvertiseUploadedFiles alarm = new AdvertiseUploadedFiles();
                alarm.FileFullName = file.name;

                Party party = NetworkData.GetPartyByClient(e.Client.ID);
                if (party.membersInfos.Count == 0)
                {
                    ClientMessageWriter.SendEncrpytedMessage(e.Client, ret, Tag.Tags.UPLOAD_FILE_RESULT);
                    return;
                }
                for (var index = 0; index < party.membersInfos.Count; index++)
                {
                    if(party.membersInfos.Count <= index) continue;

                    string memberID = party.membersInfos[index].AccountID;
                    IClient member = NetworkData.GetClientByID(memberID);

                    if(e.Client != member)
                        ClientMessageWriter.SendEncrpytedMessage(member, alarm, Tag.Tags.ADVERTISE_UPLOADED_FILE);
                }

                IClient leader = NetworkData.GetClientByID(party.leaderInfo.AccountID);

                if(e.Client != leader)
                    ClientMessageWriter.SendEncrpytedMessage(leader, alarm, Tag.Tags.ADVERTISE_UPLOADED_FILE);

                Console.WriteLine($"ADVERTISE_UPLOADED_FILE :: {alarm}");
            }
            else
            {
                ret.isSuccess = true;
                ret.log = $"Uploaded Success :: {req.Data.byteArray.index} / {savePosition.dataTotalLength - 1}";
            }

            NetworkData.SaveLog(currentPartyKey,AccessType.Upload, req.Data,
                ret.isSuccess ? "Success" : "Failed");

            ClientMessageWriter.SendEncrpytedMessage(e.Client, ret, Tag.Tags.UPLOAD_FILE_RESULT);
        }

        private static async Task WirteAllBytesAsync(string finalPath, byte[] text)
        {
            using (FileStream sourceStream = new FileStream(finalPath,
                       FileMode.Create, FileAccess.Write, FileShare.None,
                       bufferSize: 4096 * 8, useAsync: true))
            {
                var t = sourceStream.WriteAsync(text, 0, text.Length);
                await t;

                if(t.IsCompleted)
                {
                    sourceStream.Dispose();
                    sourceStream.Close();
                }
            };
        }
        private static void WhenOfficeToPDF(string originalPath, string saveAsNewPath, string fileName, Tag.FileType type)
        {
            if (type is not (Tag.FileType.Word or Tag.FileType.PPT)) return;

            var pdfFilePath = Path.Combine(saveAsNewPath, $"{fileName}.pdf");
            NetworkData.CreateDirectory(saveAsNewPath);
            var finalPDfPath = Path.Combine(saveAsNewPath, pdfFilePath);
            var cf = new ConvertFile();
            cf.ConvertToPDF(type, originalPath, finalPDfPath);
        }
}