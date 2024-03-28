namespace Bars.GisIntegration.Base.Entities.Bills
{
    /// <summary>
    /// Начисление по услуге - Главная коммунальная услуга с объемом потребления
    /// </summary>
    public class RisMunicipalServiceChargeInfo : BaseRisChargeInfo
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
        /// Сумма платы с учётом рассрочки платежа - от платы за расётный период, руб
        /// </summary>
        public virtual decimal? PaymentPeriodPiecemealPaymentSum { get; set; }

        /// <summary>
        /// Сумма платы с учётом рассрочки платежа - от платы за предыдущие расчётные периоды
        /// </summary>
        public virtual decimal? PastPaymentPeriodPiecemealPaymentSum { get; set; }

        /// <summary>
        /// Проценты за рассрочку, руб.
        /// </summary>
        public virtual decimal PiecemealPaymentPercentRub { get; set; }

        /// <summary>
        /// Проценты за рассрочку, %
        /// </summary>
        public virtual decimal PiecemealPaymentPercent { get; set; }

        /// <summary>
        /// Сумма к оплате с учётом рассрочки платежа и процентов за рассрочку
        /// </summary>
        public virtual decimal PiecemealPaymentSum { get; set; }

        /// <summary>
        /// Основания перерасчётов
        /// </summary>
        public virtual string PaymentRecalculationReason { get; set; }

        /// <summary>
        /// Сумма перерасчета
        /// </summary>
        public virtual decimal PaymentRecalculationSum { get; set; }

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
        /// К оплате за индивидуальное потребление коммунальной услуги, руб.
        /// </summary>
        public virtual decimal IndividualConsumptionPayable { get; set; }

        /// <summary>
        /// К оплате за общедомовое потребление коммунальной услуги, руб.
        /// </summary>
        public virtual decimal CommunalConsumptionPayable { get; set; }

        /// <summary>
        /// Идентификатор зарегистрированной организации
        /// </summary>
        public virtual string OrgPpaguid { get; set; }
    }
}
