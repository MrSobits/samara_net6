namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.HeatingSeason
{
    using System.Collections.Generic;

    /// <summary>
    /// Отопительный сезон
    /// </summary>
    public class HeatingSeason
    {
        /// <summary>
        /// Уникальный идентификатор объекта по подготовке к отопительному сезону
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Период по отопительному сезону
        /// </summary>
        public HeatingSeasonPeriod HeatingSeasonPeriod { get; set; }

        /// <summary>
        /// Документы по отопительному сезону
        /// </summary>
        public IEnumerable<HeatingSeasonDocumentGet> Documents { get; set; }
    }
}