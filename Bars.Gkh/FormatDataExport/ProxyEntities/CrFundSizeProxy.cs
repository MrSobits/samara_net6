namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Информация о размере фонда КР по дому (crfundsize.csv)
    /// </summary>
    public class CrFundSizeProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Идентификатор дома
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long? HouseId { get; set; }

        /// <summary>
        /// 3. Отчетный период (месяц.год)
        /// </summary>
        public DateTime? Period { get; set; }

        /// <summary>
        /// 4. Статус
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 5. Размер фонда на начало периода
        /// </summary>
        public decimal? FundOnStartPeriod { get; set; }

        /// <summary>
        /// 6. Размер фонда на конец периода
        /// </summary>
        public decimal? FundOnEndPeriod => this.FundOnStartPeriod;

        /// <summary>
        /// 7. Сумма средств, направленных на КР за отчетный период
        /// </summary>
        public decimal? AmountFund { get; set; }

        /// <summary>
        /// 8. Сумма задолженности за выполнение работ по КР на конец отчетного периода
        /// </summary>
        public decimal? AmountDept { get; set; }

    }
}