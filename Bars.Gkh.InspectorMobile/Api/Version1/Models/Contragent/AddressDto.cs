using System;

namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Contragent
{
    /// <summary>
    /// Адрес дома
    /// </summary>
    public class AddressDto
    {
        /// <summary>
        /// Уникальный идентификатор дома
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Дата начала управления
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания управления
        /// </summary>
        public DateTime? EndtDate { get; set; }

        /// <summary>
        /// Основание
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        /// Основание завершения
        /// </summary>
        public string CompletionBasis { get; set; }
    }
}
