namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    ///  Информация о размере фонда КР по помещениям (crfundsizepremises.csv)
    /// </summary>
    public class CrFundSizePremisesProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Информация о размере фонда КР по дому
        /// </summary>
        [ProxyId(typeof(CrFundSizeProxy))]
        public long? CrFundSizeId { get; set; }

        /// <summary>
        /// 3. Лицевой счет
        /// </summary>
        [ProxyId(typeof(KvarProxy))]
        public long? PersAccountId { get; set; }

        /// <summary>
        /// 4. Задолженность/переплата на начало периода
        /// </summary>
        public decimal? OverpaymentOrDebtOnStartPeriod { get; set; }

        /// <summary>
        /// 5. Начислено взносов за период
        /// </summary>
        public decimal? Contribution { get; set; }

        /// <summary>
        /// 6. Начислено пени за период
        /// </summary>
        public decimal? Penalty { get; set; }

        /// <summary>
        /// 7. Уплачено взносов за период
        /// </summary>
        public decimal? PaidContribution { get; set; }

        /// <summary>
        /// 8. Уплачено пени за период
        /// </summary>
        public decimal? PaidPenalty { get; set; }

        /// <summary>
        /// 9. Задолженность/переплата на конец периода
        /// </summary>
        public decimal? OverpaymentOrDebtOnEndPeriod { get; set; }
    }
}