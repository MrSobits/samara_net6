namespace Bars.GisIntegration.Base.Entities.Bills
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Задолженность за капитальный ремонт
    /// </summary>
    public class RisCapitalRepairDebt : BaseRisEntity
    {
        /// <summary>
        /// Платежный документ
        /// </summary>
        public virtual RisPaymentDocument PaymentDocument { get; set; }

        /// <summary>
        /// Месяц
        /// </summary>
        public virtual int Month { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Итого к оплате за расчетный период, руб.
        /// </summary>
        public virtual decimal? TotalPayable { get; set; }

        /// <summary>
        /// Идентификатор зарегистрированной организации
        /// </summary>
        public virtual string OrgPpaguid { get; set; }
    }
}
