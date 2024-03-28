namespace Bars.GisIntegration.Gkh.Cryptography
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Провайдер криптографии для алгоритма AES
    /// </summary>
    public class RijndaelCryptoProvider
    {
        /// <summary>
        /// Encryption key [32 bytes/256 bits]
        /// </summary>
        private static string _encryptKey = "Aq3zKGGz4Kmi6i1Hm901FtL4400oI2GL";

        /// <summary>
        /// Initialization vector [16 bytes/128 bits]
        /// </summary>
        private static string _iv = "46pX54YNgKsvD3tB";

        /// <inherit/>
        public static string Encrypt(byte[] bytes)
        {
            using (var rijndael = Rijndael.Create())
            {
                rijndael.Key = Encoding.UTF8.GetBytes(RijndaelCryptoProvider._encryptKey);
                rijndael.IV = Encoding.UTF8.GetBytes(RijndaelCryptoProvider._iv);
                var encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);
                using (var msData = new MemoryStream())
                using (var csData = new CryptoStream(msData, encryptor, CryptoStreamMode.Write))
                {
                    csData.Write(bytes, 0, bytes.Length);
                    csData.Flush();
                    csData.Close();
                    var result = msData.ToArray();
                    return Convert.ToBase64String(result);
                }
            }
        }

        /// <inherit/>
        public static byte[] Decrypt(string data)
        {
            var bytes = Convert.FromBase64String(data);
            using (var rijndael = Rijndael.Create())
            {
                rijndael.Key = Encoding.UTF8.GetBytes(RijndaelCryptoProvider._encryptKey);
                rijndael.IV = Encoding.UTF8.GetBytes(RijndaelCryptoProvider._iv);
                var decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);
                using (var msData = new MemoryStream(bytes))
                using (var csData = new CryptoStream(msData, decryptor, CryptoStreamMode.Read))
                {
                    var readedbytes = new byte[bytes.Length];
                    var readed = csData.Read(readedbytes, 0, bytes.Length);
                    var result = new byte[readed];
                    Array.Copy(readedbytes, 0, result, 0, readed);
                    return result;
                }
            }
        }
    }
}
