namespace Bars.Gkh.Dto
{
    using System;

    using Bars.B4.Utils;

    /// <summary>
    /// Сертификат
    /// </summary>
    [Display("Сертификат")]
    public class CertificateDto
    {
        /// <summary>
        /// Имя
        /// </summary>
        [Display("Имя")]
        public string Surname { get; set; }

        /// <summary>
        /// Инициалы
        /// </summary>
        [Display("Инициалы")]
        public string GivenName { get; set; }

        /// <summary>
        /// Отпечаток
        /// </summary>
        [Display("Отпечаток")]
        public string Thumbprint { get; set; }

        /// <summary>
        /// Серийный номер
        /// </summary>
        [Display("Серийный номер")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Дата акутальности с
        /// </summary>
        [Display("Дата акутальности с")]
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Дата актуальности по
        /// </summary>
        [Display("Дата актуальности по")]
        public DateTime ValidToDate { get; set; }

        /// <summary>
        /// Снилс
        /// </summary>
        [Display("Снилс")]
        public string Snils { get; set; }
    }
}