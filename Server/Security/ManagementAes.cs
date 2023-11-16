﻿using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Server.Security;

internal static class ManagementAes
{
    // TODO :: Add access authority
    private static string SecurityConfigPath
    {
        get
        {
            string? serverConsolePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            if (string.IsNullOrEmpty(serverConsolePath)) return PreventExceptionStringValue;

            return Path.Combine(serverConsolePath, "Config", "securityConfig.txt");
        }
    }

    private static string Aes256Key
    {
        get
        {
            using(FileStream fs = new FileStream(SecurityConfigPath, FileMode.Open))
            {
                using(StreamReader reader = new StreamReader(fs))
                {
                    string text = reader.ReadToEnd();
                    string[] lines = text.Split('\n');
                    foreach (string line in lines)
                    {
                        string[] split = line.Split(':');
                        string header = split[0].Trim();
                        if(!header.Contains("Key", StringComparison.OrdinalIgnoreCase))  continue;
                        return split[1].Trim();
                    }
                }
            }

            return PreventExceptionStringValue;
        }
    }

    private static string Aes256Iv
    {
        get
        {
            using(FileStream fs = new FileStream(SecurityConfigPath, FileMode.Open))
            {
                using(StreamReader reader = new StreamReader(fs))
                {
                    string text = reader.ReadToEnd();
                    string[] lines = text.Split('\n');
                    foreach (string line in lines)
                    {
                        string[] split = line.Split(':');
                        string header = split[0].Trim();
                        if(!header.Contains("IV", StringComparison.OrdinalIgnoreCase))  continue;
                        return split[1].Trim();
                    }
                }
            }

            return PreventExceptionStringValue;
        }
    }

    internal static byte[] AesEncrypt<TDto>(this TDto obj) where TDto : IDarkRiftSerializable
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Aes256Key);
            aes.IV = Encoding.UTF8.GetBytes(Aes256Iv);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            string json = JsonConvert.SerializeObject(obj);
            byte[] plainText = Encoding.UTF8.GetBytes(json);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(plainText, 0, plainText.Length);
                }
                return ms.ToArray();
            }
        }
    }

    internal static TDto? AesDecrypt<TDto>(this byte[] encryptedData) where TDto : IDarkRiftSerializable
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Aes256Key);
            aes.IV = Encoding.UTF8.GetBytes(Aes256Iv);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(encryptedData))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (MemoryStream resultStream = new MemoryStream())
                    {
                        cs.CopyTo(resultStream);
                        string json = Encoding.UTF8.GetString(resultStream.ToArray());
                        return JsonConvert.DeserializeObject<TDto>(json);
                    }
                }
            }

        }
    }
}