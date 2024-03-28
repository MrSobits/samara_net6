namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;

    /// <summary>
    /// Базовые параметры для запроса документа
    /// </summary>
    public class BaseDocQueryParams 
    {
        /// <summary>
        /// Период запроса данных
        /// </summary>
        [Required]
        public PeriodParameter PeriodParameter { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }
        
        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime? DocumentDate { get; set; }
        
        /// <summary>
        /// Контролируемое лицо - юридическое лицо
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// GUID дома
        /// </summary>
        public string Address { get; set; }
    }
}