using System.Diagnostics;
using System.Reflection;

// rough code
public class PDFConverter
{
  // For PDF file.
        public Process officeToPDFConverter;
        // public string converterConsolePath = @"D:\FileServerData\OfficeToPDFConverter\MSConverter.exe";
        public ProcessStartInfo officeToPDFConverterStartInfo;

        public string ConverterConsolePath()
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            path = Path.Combine(path, "Config","config.txt");
            bool exists = File.Exists(path);
            Console.WriteLine($"{(exists ? "Ok" : "Not Ok")} | {path}");
            string pathMarker = "ConverterPath";
            string converterPath = "";
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                string line;
                using (StreamReader reader = new StreamReader(fs))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] words = line.Split(':');
                        string data = $"{words[1].Trim()}:{words[2].Trim()}";
                        if (line.Contains(pathMarker))
                        {
                            converterPath = data;

                            Console.WriteLine($"pathMarker _ converterPath : {converterPath}");
                        }
                    }
                }
            }

            return converterPath;
        }
        /// <summary>
        /// return saveAsFilePath
        /// </summary>
        /// <param name="fileLoadingPath"></param>
        /// <param name="saveAsFilePath"></param>
        /// <returns></returns>
        public void ConvertToPDF(Tag.FileType officeType, string fileLoadingPath, string saveAsFilePath)
        {
            RunConverter(officeType, fileLoadingPath, saveAsFilePath);
        }

        private void RunConverter(Tag.FileType officetype, string fileLoadingPath, string saveAsFilePath)
        {
            officeToPDFConverterStartInfo = new ProcessStartInfo();
            officeToPDFConverterStartInfo.UseShellExecute = false;
            officeToPDFConverterStartInfo.CreateNoWindow = true;
            officeToPDFConverterStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            officeToPDFConverterStartInfo.RedirectStandardInput = true;
            officeToPDFConverterStartInfo.FileName = ConverterConsolePath();
            officeToPDFConverter = Process.Start(officeToPDFConverterStartInfo);

            using (StreamWriter sw = officeToPDFConverter?.StandardInput)
            {
                sw?.WriteLine("Start");
                sw?.WriteLine("PDF");
                sw?.WriteLine(officetype.ToString());
                sw?.WriteLine(fileLoadingPath);
                sw?.WriteLine(saveAsFilePath);
                sw?.Dispose();
                sw?.Close();
            }

        }
        public void KillConverter()
        {
            officeToPDFConverter.Kill();
            officeToPDFConverter.Dispose();
            officeToPDFConverter.Close();
        }
}