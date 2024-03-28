namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.File
{
    using System;

    /// <summary>
    /// Информация о сертификате
    /// </summary>
    public class CertificateInfo
    {
        /// <summary>
        /// Имя владельца сертификата
        /// </summary>
        public string PersonName { get; set; }
        
        /// <summary>
        /// Серийный номер
        /// </summary>
        public string SerialNumber { get; set; }
        
        /// <summary>
        /// Дата действия (от)
        /// </summary>
        public DateTime ValidFromDate { get; set; }
        
        /// <summary>
        /// Дата действия (по)
        /// </summary>
        public DateTime ValidToDate { get; set; }
    }
}