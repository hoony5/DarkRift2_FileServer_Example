// rough code
public class DirectoryManagement
{

        // Uploading => subPath is PartyKey
        // Exporting => subPath is PartyKey
        public static string tempPath = @"D:\FileServerData\FileServerData\Temp";
        public static string PDFPath = @"D:\FileServerData\FileServerData\PDF";
        public static string AVIPath= @"D:\FileServerData\FileServerData\AVI";
        public static string MOVPath= @"D:\FileServerData\FileServerData\MOV";
        public static string MP4Path= @"D:\FileServerData\FileServerData\MP4";
        public static string JPEGPath= @"D:\FileServerData\FileServerData\JPEG";
        public static string PNGPath= @"D:\FileServerData\FileServerData\PNG";
        public static string glTFPath= @"D:\FileServerData\FileServerData\glTF";
        public static string FBXPath= @"D:\FileServerData\FileServerData\FBX";
        public static string TextPath= @"D:\FileServerData\FileServerData\TXT";
        public static string LogPath = @"D:\FileServerData\FileServerLog";

        public static void LoadPathInfo()
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            path = Path.Combine(path, "Config","config.txt");
            bool exists = File.Exists(path);
            Console.WriteLine($"path : {path}");
            // TODO :: pathMarker -> getData
            string tempPathMarker = "tempPath";
            string PDFPathMarker = "pdfPath";
            string AVIPathMarker = "aviPath";
            string MP4PathMarker = "mp4Path";
            string MOVPathMarker = "movPath";
            string JPEGPathMarker = "jpegPath";
            string PNGPathMarker = "pngPath";
            string glTFPathMarker = "gltfPath";
            string FBXPathMarker = "fbxPath";
            string LogPathMarker = "logPath";
            string TxTPathMarker = "txtPath";

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                string line;
                using (StreamReader reader = new StreamReader(fs))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] words = line.Split(':');
                        if (words.Length <= 1) continue;
                        string data = $"{words[1].Trim()}:{words[2].Trim()}";

                        if (line.Contains(tempPathMarker))
                        {
                            tempPath = data;
                            if (!Directory.Exists(tempPath))
                                Directory.CreateDirectory(tempPath);
                        }

                        if (line.Contains(PDFPathMarker))
                        {
                            PDFPath = data;
                            if (!Directory.Exists(PDFPath))
                                Directory.CreateDirectory(PDFPath);
                        }

                        if (line.Contains(AVIPathMarker))
                        {
                            AVIPath = data;

                            if (!Directory.Exists(AVIPath))
                                Directory.CreateDirectory(AVIPath);
                        }

                        if (line.Contains(MP4PathMarker))
                        {
                            MP4Path = data;

                            if (!Directory.Exists(MP4Path))
                                Directory.CreateDirectory(MP4Path);
                        }


                        if (line.Contains(MOVPathMarker))
                        {
                            MOVPath = data;

                            if (!Directory.Exists(MOVPath))
                                Directory.CreateDirectory(MOVPath);
                        }

                        if (line.Contains(JPEGPathMarker))
                        {
                            JPEGPath = data;

                            if (!Directory.Exists(JPEGPath))
                                Directory.CreateDirectory(JPEGPath);
                        }

                        if (line.Contains(PNGPathMarker))
                        {
                            PNGPath = data;

                            if (!Directory.Exists(PNGPath))
                                Directory.CreateDirectory(PNGPath);
                        }

                        if (line.Contains(glTFPathMarker))
                        {
                            glTFPath = data;

                            if (!Directory.Exists(glTFPath))
                                Directory.CreateDirectory(glTFPath);
                        }

                        if (line.Contains(FBXPathMarker))
                        {
                            FBXPath = data;

                            if (!Directory.Exists(FBXPath))
                                Directory.CreateDirectory(FBXPath);
                        }
                        if (line.Contains(TxTPathMarker))
                        {
                            TextPath = data;

                            if (!Directory.Exists(TextPath))
                                Directory.CreateDirectory(TextPath);
                        }

                        if (line.Contains(LogPathMarker))
                        {
                            LogPath = data;

                            if (!Directory.Exists(LogPath))
                                Directory.CreateDirectory(LogPath);
                        }
                    }
                }
            }
        }
        public static string GetPathByFileType(Tag.FileType type)
        {
            return type switch
            {
                Tag.FileType.Word or Tag.FileType.PPT or Tag.FileType.PDF or Tag.FileType.CSV => PDFPath,
                Tag.FileType.MP4 => MP4Path,
                Tag.FileType.MOV => MOVPath,
                Tag.FileType.AVI => AVIPath,
                Tag.FileType.PNG => PNGPath,
                Tag.FileType.JPEG or Tag.FileType.JPG => JPEGPath,
                Tag.FileType.glTF => glTFPath,
                Tag.FileType.FBX => FBXPath,
                Tag.FileType.TXT => TextPath,
                _ => tempPath
            };
        }
}