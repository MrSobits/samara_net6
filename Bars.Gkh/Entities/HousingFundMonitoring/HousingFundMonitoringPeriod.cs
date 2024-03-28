namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Период Мониторинга жилищного фонда
    /// </summary>
    public class HousingFundMonitoringPeriod : BaseImportableEntity
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }
    }
}