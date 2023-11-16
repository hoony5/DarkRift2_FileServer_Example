using System.Diagnostics;
using System.Reflection;

// rough code
public class PDFConverter : IDisposable
{
        private readonly Process? converter;
        private readonly ProcessStartInfo? converterInfo;
        private const string CONVERTER_PATH_HEADER = "converter";
        private const string FILE_TYPE_SIGNATURE = "type";
        private const string FILE_PATH_SIGNATURE = "path";
        private const string SAVE_PATH_SIGNATURE = "save";
        
        private bool IsRunning => converter?.HasExited == false;
        public PDFConverter()
        {
            converterInfo = new ProcessStartInfo();
            converterInfo.UseShellExecute = false;
            converterInfo.CreateNoWindow = true;
            converterInfo.WindowStyle = ProcessWindowStyle.Hidden;
            converterInfo.RedirectStandardInput = true;
            converterInfo.FileName = GetConverterPath();
            converter = Process.Start(converterInfo);
        }
        private string GetConverterPath()
        {
            string? path = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            if(path is null)
            {
                FileServer.DebugLog($"PDFConverter is null");
                return PreventExceptionStringValue;
            }
            
            path = Path.Combine(path, ConfigPath);
            
            string converterPath = "";
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                string? line;
                using (StreamReader reader = new StreamReader(fs))
                {
                    string text = reader.ReadToEnd();
                    string[] split = text.Split(DashSignature);
                    converterPath 
                        = new string(Array.Find(split,
                                line => line
                                    .Contains(CONVERTER_PATH_HEADER, StringComparison.OrdinalIgnoreCase))?
                            .Split(EqualSignature)[CommandKeywordIndex]
                            .Trim());
                }
            }

            return converterPath;
        }
        public void Convert(string fileType, string fileFullName, string saveAs)
        {
            if(!IsRunning)
            {
                FileServer.DebugLog($"PDFConverter is not running");
                return;
            }
            
            InputCommand(fileType, fileFullName, saveAs);
        }

        private void InputCommand(string fileType, string fileFullPath, string saveAs)
        {
            using (StreamWriter? sw = converter?.StandardInput)
            {
                string? consolePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
                string finalPath = Path.Combine(consolePath, SharedFileFolderName, saveAs);
                sw?.WriteLine
                    ($"-{FILE_TYPE_SIGNATURE}={fileType} -{FILE_PATH_SIGNATURE}={fileFullPath} -{SAVE_PATH_SIGNATURE}={finalPath}");
            }
        }
        public void Dispose()
        {
            converter?.Kill();
            converter?.Dispose();
            converter?.Close();
        }
}