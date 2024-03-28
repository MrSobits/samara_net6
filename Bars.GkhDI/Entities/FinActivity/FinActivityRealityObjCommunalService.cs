namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Коммунальные услуги
    /// </summary>
    public class FinActivityRealityObjCommunalService : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО объекта недвижимости
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual TypeServiceDi TypeServiceDi { get; set; }

        /// <summary>
        /// Оплачено собственников
        /// </summary>
        public virtual decimal? PaidOwner { get; set; }

        /// <summary>
        /// Задолженность собственников
        /// </summary>
        public virtual decimal? DebtOwner { get; set; }

        /// <summary>
        /// Оплачено по показаниям
        /// </summary>
        public virtual decimal? PaidByIndicator { get; set; }

        /// <summary>
        /// Оплачено по счетам
        /// </summary>
        public virtual decimal? PaidByAccount { get; set; }
    }
}
