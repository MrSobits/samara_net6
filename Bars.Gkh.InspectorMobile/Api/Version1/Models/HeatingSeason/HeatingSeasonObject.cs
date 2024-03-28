namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.HeatingSeason
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Объект отопительного сезона. Модель выборки
    /// </summary>
    public class HeatingSeasonObjectGet : BaseHeatingSeasonObject<HeatingSeasonDocumentGet>
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор периода по отопительному сезону
        /// </summary>
        public long HeatingSeasonPeriodId { get; set; }

        /// <summary>
        /// Уникальный идентификатор дома
        /// </summary>
        public long AddressId { get; set; }
    }

    /// <summary>
    /// Объект отопительного сезона. Модель выборки
    /// </summary>
    public class HeatingSeasonObjectUpdate : BaseHeatingSeasonObject<HeatingSeasonDocumentUpdate>
    {
    }

    /// <summary>
    /// Объект отопительного сезона. Базовая модель
    /// </summary>
    public class BaseHeatingSeasonObject<TDocument>
    {
        /// <summary>
        /// Документы по отопительному сезону
        /// </summary>
        [Required]
        public IEnumerable<TDocument> Documents { get; set; }
    }
}