namespace Bars.GisIntegration.Base.Entities.Bills
{
    using Bars.GisIntegration.Base.Entities;
    using Enums;

    /// <summary>
    /// Период
    /// </summary>
    public class OrgPaymentPeriod : BaseRisEntity
    {
        /// <summary>
        /// Месяц
        /// </summary>
        public virtual int Month { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Тип платежного периода
        /// </summary>
        public virtual RisPaymentPeriodType RisPaymentPeriodType { get; set; }

        /// <summary>
        /// Признак того, что период был успешно открыт через сервис ГИСа
        /// </summary>
        public virtual bool IsApplied { get; set; }
    }
}