namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сведения об оплатах коммунальных услуг
    /// </summary>
    public class InfoAboutPaymentCommunal : BaseGkhEntity
    {
        /// <summary>
        /// Объект в управление
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual BaseService BaseService { get; set; }

        /// <summary>
        /// Показания счетчика на начало периода
        /// </summary>
        public virtual decimal? CounterValuePeriodStart { get; set; }

        /// <summary>
        /// Показания счетчика на конец периода
        /// </summary>
        public virtual decimal? CounterValuePeriodEnd { get; set; }

        /// <summary>
        /// Общий объем потребления
        /// </summary>
        public virtual decimal? TotalConsumption { get; set; }

        /// <summary>
        /// Начислено потребителям (руб.)
        /// </summary>
        public virtual decimal? Accrual { get; set; }

        /// <summary>
        /// Оплачено потребителями (руб.)
        /// </summary>
        public virtual decimal? Payed { get; set; }

        /// <summary>
        /// Задолженность потребителей
        /// </summary>
        public virtual decimal? Debt { get; set; }

        /// <summary>
        /// Начислено поставщиком (поставщиками) коммунального ресурса (руб.)
        /// </summary>
        public virtual decimal? AccrualByProvider { get; set; }

        /// <summary>
        /// Оплачено поставщику (поставщикам) коммунального ресурса (руб.)
        /// </summary>
        public virtual decimal? PayedToProvider { get; set; }

        /// <summary>
        /// Задолженность перед поставщиком (поставщиками) коммунального ресурса (руб.)
        /// </summary>
        public virtual decimal? DebtToProvider { get; set; }

        /// <summary>
        /// Сумма пени и штрафов, полученных от потребителей (руб.)
        /// </summary>
        public virtual decimal? ReceivedPenaltySum { get; set; }
    }
}
