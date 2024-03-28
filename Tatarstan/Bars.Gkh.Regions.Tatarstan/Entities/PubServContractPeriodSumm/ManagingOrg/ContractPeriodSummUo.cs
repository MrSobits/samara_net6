namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Агрегация по Периоду для УО
    /// </summary>
    public class ContractPeriodSummUo : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Управляющая компания
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Входящее сальдо  (Долг на начало месяца)
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
        /// Сумма учтеной  недопоставки
        /// </summary>
        public virtual decimal NoDeliverySum { get; set; }

        /// <summary>
        /// Оплачено жителями
        /// </summary>
        public virtual decimal PaidResidents { get; set; }

        /// <summary>
        /// Исходящее сальдо  (Долг на конец месяца)
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