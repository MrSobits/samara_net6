namespace Bars.Esia.OAuth20.App.Providers.Impl
{
    using System;
    using System.Security.Cryptography;
    using System.Security.Cryptography.Pkcs;
    using System.Security.Cryptography.X509Certificates;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Поставщик безопасности приложения
    /// </summary>
    public class AuthAppSecurityProvider : IAuthAppSecurityProvider
    {
        /// <summary>
        /// Поставщик параметров приложения
        /// </summary>
        public IAuthAppOptionProvider AuthAppOptionProvider { get; set; }

        /// <inheritdoc />
        public X509Certificate2 GetSystemCertificate()
        {
            var esiaOptions = this.AuthAppOptionProvider.GetEsiaOptions();

            var myX509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            myX509Store.Open(OpenFlags.OpenExistingOnly);

            var regexCertThumbPrint = Regex.Replace(esiaOptions.CertificateThumbPrint, @"[^\da-zA-z]", string.Empty).ToUpper();
            var certColl = myX509Store.Certificates.Find(X509FindType.FindByThumbprint, regexCertThumbPrint, false);

            myX509Store.Close();

            if (certColl == null || certColl.Count == 0)
            {
                throw new Exception("Не удалось найти сертификат с указанным отпечатком");
            }

            var cert = certColl[0];

            if (!cert.HasPrivateKey)
            {
                throw new Exception("Указанный в настройках приложения сертификат не имеет закрытого ключа");
            }

            return cert;
        }

        /// <inheritdoc />
        public X509Certificate2 GetEsiaCertificate()
        {
            // При возникновении необходимости дополнительной защиты нужно
            // 1. указать действующий публичный сертификат портала ЕСИА
            // 2. раскомментить вызовы метода VerifyToken
            // (можно найти через использования метода VerifyMessage)
            return new X509Certificate2();
        }

        /// <inheritdoc />
        public byte[] SignMessage(byte[] message, X509Certificate2 certificate)
        {
            var signedCms = new SignedCms(new ContentInfo(message), true);
            signedCms.ComputeSignature(new CmsSigner(certificate));
            return signedCms.Encode();
        }

        /// <inheritdoc />
        public bool VerifyMessage(string alg, byte[] message, byte[] signature, X509Certificate2 certificate)
        {
            return alg.ToUpperInvariant() == "RS256" && ((RSACryptoServiceProvider) certificate.PublicKey.Key).VerifyData(message, CryptoConfig.MapNameToOID("SHA256"), signature);
        }
    }
}