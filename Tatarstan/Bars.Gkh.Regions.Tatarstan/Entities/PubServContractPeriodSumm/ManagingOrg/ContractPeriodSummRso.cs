namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Агрегация по Периоду для РСО
    /// </summary>
    public class ContractPeriodSummRso : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Ресурсоснабжающая организация
        /// </summary>
        public virtual PublicServiceOrg PublicServiceOrg { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Начислено  УО
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
    }
}