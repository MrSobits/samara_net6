namespace Bars.Gkh.Models
{
    /// <summary>
    /// Результат подписи
    /// </summary>
    public class SignResponse
    {
        /// <summary>
        /// Вычисленная подпись
        /// </summary>
        public byte[] Signature { get; set; }

        /// <summary>
        /// Подпись XML
        /// </summary>
        public string XmlSignature { get; set; }

        /// <summary>
        /// Вычисленный хэш
        /// </summary>
        public byte[] Hash { get; set; }
    }
}