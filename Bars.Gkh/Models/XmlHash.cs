namespace Bars.Gkh.Models
{
    /// <summary>
    /// DTO для передачи данных в CreateXmlHash
    /// </summary>
    public class XmlHash
    {
        /// <summary>
        /// Ссылки на подписываемые блоки
        /// </summary>
        public ReferenceModel[] References { get; set; }

        /// <summary>
        /// Исходный документ
        /// </summary>
        public string XmlContent { get; set; }

        /// <summary>
        /// Метод подписи
        /// </summary>
        public string SignatureMethod { get; set; }
    }
}