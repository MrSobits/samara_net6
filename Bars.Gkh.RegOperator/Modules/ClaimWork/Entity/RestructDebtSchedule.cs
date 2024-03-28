namespace Bars.Gkh.ClaimWork.Entities
{
    using System;
    using B4.DataAccess;

    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// График реструктуризации
    /// </summary>
    public class RestructDebtSchedule : BaseEntity
    {
        /// <summary>
        /// Реструктуризация
        /// </summary>
        public virtual RestructDebt RestructDebt { get; set; }

        /// <summary>
        /// Абонент
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Общая сумма долга
        /// </summary>
        public virtual decimal TotalDebtSum { get; set; }

        /// <summary>
        /// Планируемая дата оплаты
        /// </summary>
        public virtual DateTime PlanedPaymentDate { get; set; }

        /// <summary>
        /// Сумма к оплате
        /// </summary>
        public virtual decimal PlanedPaymentSum { get; set; }

        /// <summary>
        /// Фактическая дата оплаты
        /// </summary>
        public virtual DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Оплачено
        /// </summary>
        public virtual decimal PaymentSum { get; set; }

        /// <summary>
        /// Признак просроченной оплаты
        /// </summary>
        public virtual bool IsExpired { get; set; }
    }
}