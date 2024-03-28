namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Информация по периоду 
    /// для Договора УО с РСО на предоставление услуги
    /// </summary>
    public class PubServContractPeriodSumm : BaseEntity
    {
        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Отчетный период договора
        /// </summary>
        public virtual ContractPeriod ContractPeriod { get; set; }

        /// <summary>
        /// Агрегация по Периоду для РСО
        /// </summary>
        public virtual ContractPeriodSummRso ContractPeriodSummRso { get; set; }

        /// <summary>
        /// Агрегация по Периоду для УО
        /// </summary>
        public virtual ContractPeriodSummUo ContractPeriodSummUo { get; set; }

        /// <summary>
        /// Услуга по Договору РСО с Домами
        /// </summary>
        public virtual PublicServiceOrgContractService PublicService { get; set; }
    }
}