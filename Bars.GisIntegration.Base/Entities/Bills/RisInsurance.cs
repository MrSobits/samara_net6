namespace Bars.GisIntegration.Base.Entities.Bills
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Страхование
    /// </summary>
    public class RisInsurance : BaseRisEntity
    {
        /// <summary>
        /// Идентификатор страхового продукта
        /// </summary>
        public virtual string InsuranceProductGuid { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        public virtual decimal Rate { get; set; }

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
        /// Порядок рассчетов
        /// </summary>
        public virtual string CalcExplanation { get; set; }
    }
}