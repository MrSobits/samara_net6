namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Запись Мониторинга жилищного фонда
    /// </summary>
    public class HousingFundMonitoringInfo : BaseImportableEntity
    {
        /// <summary>
        /// Период Мониторинга жилищного фонда
        /// </summary>
        public virtual HousingFundMonitoringPeriod Period { get; set; }

        /// <summary>
        /// Номер по порядку
        /// </summary>
        public virtual string RowNumber { get; set; }

        /// <summary>
        /// Наименование показателя
        /// </summary>
        public virtual string Mark { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual string UnitMeasure { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual decimal? Value { get; set; }

        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual string DataProvider { get; set; }
    }
}