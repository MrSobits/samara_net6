namespace Bars.GisIntegration.Base.Entities.Bills
{
    using NHibernate.Type;

    /// <summary>
    /// Начисление по услуге - Жилищная услуга
    /// </summary>
    public class RisHousingServiceChargeInfo : BaseRisChargeInfo
    {
        /// <summary>
        /// Ссылка на платежный документ
        /// </summary>
        public virtual RisPaymentDocument PaymentDocument { get; set; }

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
        public virtual decimal TotalPayable { get; set; }

        /// <summary>
        /// Всего начислено за расчетный период (без перерасчетов и льгот), руб.
        /// </summary>
        public virtual decimal AccountingPeriodTotal { get; set; }

        /// <summary>
        /// Порядок расчетов
        /// </summary>
        public virtual string CalcExplanation { get; set; }

        /// <summary>
        /// Идентификатор зарегистрированной организации
        /// </summary>
        public virtual string OrgPpaguid { get; set; }  
    }
}
