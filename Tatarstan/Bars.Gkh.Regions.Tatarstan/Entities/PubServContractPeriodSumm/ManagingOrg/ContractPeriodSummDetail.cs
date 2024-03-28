namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Детализация для РСО и УО по Дому в Периоде
    /// </summary>
    public class ContractPeriodSummDetail : BaseEntity
    {
        /// <summary>
        /// Дом в Договоре РСО с Домами
        /// </summary>
        public virtual PublicServiceOrgContractRealObj PublicServiceOrgContractRealObjInContract { get; set; }

        /// <summary>
        /// Информация по периоду
        /// для Договора УО с РСО на предоставление услуги
        /// </summary>
        public virtual PubServContractPeriodSumm ContractPeriodSumm { get; set; }

        /// <summary>
        /// Начислено УО
        /// </summary>
        public virtual decimal ChargedManOrg { get; set; }

        /// <summary>
        /// Оплачено УО
        /// </summary>
        public virtual decimal PaidManOrg { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public virtual decimal SaldoOut { get; set; }

        /// <summary>
        /// Входящее сальдо (Долг на начало месяца)
        /// </summary>
        public virtual decimal StartDebt { get; set; }

        /// <summary>
        /// Начислено жителям
        /// </summary>
        public virtual decimal ChargedResidents { get; set; }

        /// <summary>
        /// Сумма перерасчета начисления за предыдущий период
        /// </summary>
        public virtual decimal RecalcPrevPeriod { get; set; }

        /// <summary>
        /// Сумма изменений (перекидки)
        /// </summary>
        public virtual decimal ChangeSum { get; set; }

        /// <summary>
        /// Сумма учтеной недопоставки
        /// </summary>
        public virtual decimal NoDeliverySum { get; set; }

        /// <summary>
        /// Оплачено жителями
        /// </summary>
        public virtual decimal PaidResidents { get; set; }

        /// <summary>
        /// Исходящее сальдо (Долг на конец месяца)
        /// </summary>
        public virtual decimal EndDebt { get; set; }

        /// <summary>
        /// Начислено к оплате
        /// </summary>
        public virtual decimal ChargedToPay { get; set; }

        /// <summary>
        /// Перечислено РСО
        /// </summary>
        public virtual decimal TransferredPubServOrg { get; set; }
    }
}