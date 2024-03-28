namespace Bars.GkhRf.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhRf.Enums;

    /// <summary>
    /// Объект договора рег. фонда
    /// </summary>
    public class ContractRfObject : BaseGkhEntity
    {
        /// <summary>
        /// Договор рег. фонда
        /// </summary>
        public virtual ContractRf ContractRf { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Дата включения в договор
        /// </summary>
        public virtual DateTime? IncludeDate { get; set; }

        /// <summary>
        /// Дата исключения из договора
        /// </summary>
        public virtual DateTime? ExcludeDate { get; set; }

        /// <summary>
        /// Тип состояния объекта
        /// </summary>
        public virtual TypeCondition TypeCondition { get; set; }

        /// <summary>
        /// Общая площадь жилых и нежилых помещений в доме
        /// </summary>
        public virtual decimal? TotalArea { get; set; }

        /// <summary>
        /// Общая площадь жилых помещений в доме
        /// </summary>
        public virtual decimal? AreaLiving { get; set; }

        /// <summary>
        /// Общая площадь нежилых помещений в доме
        /// </summary>
        public virtual decimal? AreaNotLiving { get; set; }

        /// <summary>
        /// Общая площадь жилых помещений находящихся в собственности граждан
        /// </summary>
        public virtual decimal? AreaLivingOwned { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }
    }
}
