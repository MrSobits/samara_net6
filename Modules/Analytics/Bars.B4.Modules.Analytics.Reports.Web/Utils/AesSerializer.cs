namespace Bars.B4.Modules.Analytics.Reports.Web.Utils
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// Сериализатор AES
    /// </summary>
    public class AesSerializer
    {
        private byte[] Key;
        private byte[] Iv;

        public AesSerializer(byte[] key, byte[] iv)
        {
            this.Key = key;
            this.Iv = iv;
        }

        /// <summary>
        /// Закодировать
        /// </summary>
        public byte[] Encrypt(string data)
        {
            using (var rij = new RijndaelManaged())
            {
                rij.Key = this.Key;
                rij.IV = this.Iv;

                var encryptor = rij.CreateEncryptor(rij.Key, rij.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(data);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        /// <summary>
        /// Раскодировать BASE64 строку
        /// </summary>
        public string Decrypt(string data)
        {
            var bytes = Convert.FromBase64String(data);
            return this.Decrypt(bytes);
        }

        /// <summary>
        /// Раскодировать
        /// </summary>
        public string Decrypt(byte[] data)
        {
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = this.Key;
                rijAlg.IV = this.Iv;

                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                using (var msDecrypt = new MemoryStream(data))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}