namespace Bars.GisIntegration.Base.Entities.Bills
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Задолженность за капитальный ремонт
    /// </summary>
    public class RisCapitalRepairCharge : BaseRisEntity
    {
        /// <summary>
        /// Платежный документ
        /// </summary>
        public virtual RisPaymentDocument PaymentDocument { get; set; }

        /// <summary>
        /// Размер взноса на кв.м, руб
        /// </summary>
        public virtual decimal? Contribution { get; set; }

        /// <summary>
        /// Всего начислено за расчетный период (без перерасчетов и льгот), руб
        /// </summary>
        public virtual decimal? AccountingPeriodTotal { get; set; }

        /// <summary>
        /// Перерасчеты, корректировки (руб)
        /// </summary>
        public virtual decimal? MoneyRecalculation { get; set; }

        /// <summary>
        /// Льготы, субсидии, скидки (руб)
        /// </summary>
        public virtual decimal? MoneyDiscount { get; set; }

        /// <summary>
        /// Итого к оплате за расчетный период, руб.
        /// </summary>
        public virtual decimal? TotalPayable { get; set; }

        /// <summary>
        /// Идентификатор зарегистрированной организации
        /// </summary>
        public virtual string OrgPpaguidCapitalRepairCharge { get; set; }
    }
}
