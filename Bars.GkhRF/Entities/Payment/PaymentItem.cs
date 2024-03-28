namespace Bars.GkhRf.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhRf.Enums;

    /// <summary>
    /// Оплата КР
    /// </summary>
    public class PaymentItem : BaseGkhEntity
    {
        /// <summary>
        /// Оплата
        /// </summary>
        public virtual Payment Payment { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        public virtual TypePayment TypePayment { get; set; }

        /// <summary>
        /// Дата начисления
        /// </summary>
        public virtual DateTime? ChargeDate { get; set; }

        /// <summary>
        /// Входящее сальдо
        /// </summary>
        public virtual decimal? IncomeBalance { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public virtual decimal? OutgoingBalance { get; set; }

        /// <summary>
        /// Начислено населению
        /// </summary>
        public virtual decimal? ChargePopulation { get; set; }

        /// <summary>
        /// Оплачено населением
        /// </summary>
        public virtual decimal? PaidPopulation { get; set; }

        /// <summary>
        /// Перерасчет прошлого периода
        /// </summary>
        public virtual decimal? Recalculation { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual decimal? TotalArea { get; set; }
    }
}