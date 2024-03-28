namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Коммунальные услуги
    /// </summary>
    public class FinActivityCommunalService : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual TypeServiceDi TypeServiceDi { get; set; }

        /// <summary>
        /// Взыскано
        /// </summary>
        public virtual decimal? Exact { get; set; }

        /// <summary>
        /// Доход от представления
        /// </summary>
        public virtual decimal? IncomeFromProviding { get; set; }

        /// <summary>
        /// Задолженность населения на начало
        /// </summary>
        public virtual decimal? DebtPopulationStart { get; set; }

        /// <summary>
        /// Задолженность населения на конец
        /// </summary>
        public virtual decimal? DebtPopulationEnd { get; set; }

        /// <summary>
        /// Задолженность упр орг за коммунальные услуги
        /// </summary>
        public virtual decimal? DebtManOrgCommunalService { get; set; }

        /// <summary>
        /// Оплачено по показаниям общедомовых приборов учета
        /// </summary>
        public virtual decimal? PaidByMeteringDevice { get; set; }

        /// <summary>
        /// Оплачено по счетам на общедомовые нужды
        /// </summary>
        public virtual decimal? PaidByGeneralNeeds { get; set; }

        /// <summary>
        /// Выплаты по искам
        /// </summary>
        public virtual decimal? PaymentByClaim { get; set; }
    }
}
