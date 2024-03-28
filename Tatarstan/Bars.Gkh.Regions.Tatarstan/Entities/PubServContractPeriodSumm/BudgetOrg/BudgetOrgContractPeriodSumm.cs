namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;

    /// <summary>
    /// Договор "РСО и бюджет" за период с показателями
    /// </summary>
    public class BudgetOrgContractPeriodSumm : BaseEntity
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
        /// Услуга по договору
        /// </summary>
        public virtual PublicServiceOrgContractService ContractService { get; set; }

        /// <summary>
        /// Сторона договора "Бюджетная организация"
        /// </summary>
        public virtual BudgetOrgContract BudgetOrgContract { get; set; }

        /// <summary>
        /// РСО
        /// </summary>
        public virtual PublicServiceOrg PublicServiceOrg { get; set; }

        /// <summary>
        /// Начислено
        /// </summary>
        public virtual decimal Charged { get; set; }

        /// <summary>
        /// Оплачено
        /// </summary>
        public virtual decimal Paid { get; set; }

        /// <summary>
        /// Задолженность на конец месяца
        /// </summary>
        public virtual decimal EndDebt { get; set; }
    }
}