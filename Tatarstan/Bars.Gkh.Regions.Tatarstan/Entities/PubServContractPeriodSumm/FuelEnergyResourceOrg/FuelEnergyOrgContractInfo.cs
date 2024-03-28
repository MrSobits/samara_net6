namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;

    /// <summary>
    /// Договор "РСО и ТЭР" за период с импортируемыми показателями
    /// </summary>
    public class FuelEnergyOrgContractInfo : BaseEntity
    {
        /// <summary>
        /// Агрегация по РСО в периоде
        /// </summary>
        public virtual ServiceOrgFuelEnergyResourcePeriodSumm PeriodSummary { get; set; }

        /// <summary>
        /// Ресурс по договору
        /// </summary>
        public virtual CommunalResource Resource { get; set; }

        /// <summary>
        /// Сторона договора "ТЭР"
        /// </summary>
        public virtual FuelEnergyResourceContract FuelEnergyResourceContract { get; set; }

        /// <summary>
        /// Задолженность на начало месяца
        /// </summary>
        public virtual decimal SaldoIn { get; set; }

        /// <summary>
        /// Входящая просроченная задолженность
        /// </summary>
        public virtual decimal DebtIn { get; set; }

        /// <summary>
        /// Начислено за месяц
        /// </summary>
        public virtual decimal Charged { get; set; }

        /// <summary>
        /// Оплачено за месяц
        /// </summary>
        public virtual decimal Paid { get; set; }

        /// <summary>
        /// Задолженность на конец месяца
        /// </summary>
        public virtual decimal SaldoOut { get; set; }

        /// <summary>
        /// Исходящая просроченная задолженность
        /// </summary>
        public virtual decimal DebtOut { get; set; }

        /// <summary>
        /// Планируемая оплата
        /// </summary>
        public virtual decimal PlanPaid { get; set; }

        /// <summary>
        /// Расхождение в оплатах
        /// </summary>
        public virtual decimal PaidDelta => this.PlanPaid - this.Paid;
    }
}