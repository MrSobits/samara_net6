namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сведения о случаях снижения платы
    /// </summary>
    public class InfoAboutReductionPayment : BaseGkhEntity
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
        /// Причина снижения
        /// </summary>
        public virtual string ReasonReduction { get; set; }

        /// <summary>
        /// Сумма перерасчета
        /// </summary>
        public virtual decimal? RecalculationSum { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Номер приказа
        /// </summary>
        public virtual string OrderNum { get; set; }

        /// <summary>
        /// Дата приказа
        /// </summary>
        public virtual DateTime? OrderDate { get; set; }
    }
}
