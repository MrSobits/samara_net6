namespace Bars.Gkh.Utils
{
    using System.Linq;

    /// <summary>
    /// Алгоритмы для хэша и подписи
    /// </summary>
    public static class SignatureAlgorithm
    {
        /// <summary>
        ///     Алгоритм хэша SHA1
        /// </summary>
        public const string DsigSHA1 = "http://www.w3.org/2000/09/xmldsig#sha1";

        /// <summary>
        ///     Алгоритм хэша MD5
        /// </summary>
        public const string DsigMD5 = "http://www.w3.org/2001/04/xmldsig-more#md5";

        /// <summary>
        ///     Алгоритм подписи DSA-SHA1
        /// </summary>
        public const string DsigDSA = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";

        /// <summary>
        ///     Алгоритм  подписи RSA-SHA1
        /// </summary>
        public const string DsigRSASHA1 = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

        /// <summary>
        ///     Алгоритм подписи HMAC-SHA1
        /// </summary>
        public const string DsigHMACSHA1 = "http://www.w3.org/2000/09/xmldsig#hmac-sha1";

        /// <summary>
        ///     Алгоритм хэша SHA256
        /// </summary>
        public const string DsigSHA256 = "http://www.w3.org/2001/04/xmlenc#sha256";

        /// <summary>
        ///     Алгоритм подписи RSA-SHA256
        /// </summary>
        public const string DsigRSASHA256 = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

        /// <summary>
        ///     Алгоритм хэша SHA384
        /// </summary>
        public const string DsigSHA384 = "http://www.w3.org/2001/04/xmldsig-more#sha384";

        /// <summary>
        ///     Алгоритм подписи RSA-SHA384
        /// </summary>
        public const string DsigRSASHA384 = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384";

        /// <summary>
        ///     Алгоритм хэша SHA-512
        /// </summary>
        public const string DsigSHA512 = "http://www.w3.org/2001/04/xmlenc#sha512";

        /// <summary>
        ///     Алгоритм подписи RSA-SHA512
        /// </summary>
        public const string DsigRSASHA512 = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";

        /// <summary>
        ///     Алгоритм хэша ГОСТ-3411
        /// </summary>
        public const string DsigGost3411 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr3411";

        /// <summary>
        ///     Алгоритм подписи ГОСТ-3410-2001-ГОСТ-3411
        /// </summary>
        public const string DsigGost3410 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102001-gostr3411";

        /// <summary>
        ///     Алгоритм подписи HMAC-ГОСТ-3411
        /// </summary>
        public const string DsigGost3411HMAC = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:hmac-gostr3411";

        /// <summary>
        ///     Алгоритм хэша ГОСТ-3411-2012-256
        /// </summary>
        public const string DsigGost3411_2012_256 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-256";

        /// <summary>
        ///     Алгоритм подписи ГОСТ-3410-2012-ГОСТ-3411-2012-256
        /// </summary>
        public const string DsigGost3410_2012_256 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-256";

        /// <summary>
        ///     Алгоритм подписи HMAC-ГОСТ-3411-2012-256
        /// </summary>
        public const string DsigGost3411_2012_256HMAC = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:hmac-gostr3411-2012-256";

        /// <summary>
        ///     Алгоритм хэша ГОСТ-3411-2012-512
        /// </summary>
        public const string DsigGost3411_2012_512 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34112012-512";

        /// <summary>
        ///     Алгоритм подписи ГОСТ-3410-2012-ГОСТ-3411-2012-512
        /// </summary>
        public const string DsigGost3410_2012_512 = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:gostr34102012-gostr34112012-512";

        /// <summary>
        ///     Алгоритм подписи HMAC-ГОСТ-3411-2012-512
        /// </summary>
        public const string DsigGost3411_2012_512HMAC = "urn:ietf:params:xml:ns:cpxmlsec:algorithms:hmac-gostr3411-2012-512";

        /// <summary>
        ///     Проверка поддерживаемых алгоритмов подписи
        /// </summary>
        public static bool IsSignatureAlgorithm(string algorithm)
        {
            var algorithms = new[]
            {
                DsigGost3410, DsigGost3410_2012_256, DsigGost3410_2012_512, DsigGost3411_2012_256HMAC, DsigGost3411_2012_512HMAC,
                DsigGost3411HMAC, DsigRSASHA1, DsigRSASHA256, DsigRSASHA384, DsigRSASHA512, DsigHMACSHA1
            };

            return algorithms.Any(x => x == algorithm);
        }

        /// <summary>
        ///     Получить алгоритм хэширования по алгоритму подписания
        /// </summary>
        /// <param name="signatureAlgorithm">Алгоритм подписания</param>
        /// <returns>Алгоритм хэширования</returns>
        public static string GetHashAlgorithm(string signatureAlgorithm)
        {
            switch (signatureAlgorithm)
            {
                case DsigDSA:
                case DsigRSASHA1:
                case DsigHMACSHA1:
                    return DsigSHA1;
                case DsigRSASHA256:
                    return DsigSHA256;
                case DsigRSASHA384:
                    return DsigSHA384;
                case DsigRSASHA512:
                    return DsigSHA512;
                case DsigGost3410:
                case DsigGost3411HMAC:
                    return DsigGost3411;
                case DsigGost3410_2012_256:
                case DsigGost3411_2012_256HMAC:
                    return DsigGost3411_2012_256;
                case DsigGost3410_2012_512:
                case DsigGost3411_2012_512HMAC:
                    return DsigGost3411_2012_512;
                default:
                    return null;
            }
        }
    }
}