namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Все договоры РСО и Бюджет/УО за период по услугам (Агреграция по РСО по услуге по всем договорам за период) - Агрегатор
    /// </summary>
    public class ServiceOrgFuelEnergyResourcePeriodSumm : BaseEntity
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
        /// РСО
        /// </summary>
        public virtual PublicServiceOrg PublicServiceOrg { get; set; }

        /// <summary>
        /// Детализации по услугам
        /// </summary>
        public virtual IEnumerable<FuelEnergyOrgContractDetail> Details { get; set; }
    }
}