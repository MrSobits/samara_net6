namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сведения об оплатах жилищных услуг
    /// </summary>
    public class InfoAboutPaymentHousing : BaseGkhEntity
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
        /// Общие начисления
        /// </summary>
        public virtual decimal? GeneralAccrual { get; set; }

        /// <summary>
        /// Сбор
        /// </summary>
        public virtual decimal? Collection { get; set; }
    }
}
