namespace Bars.Gkh.Regions.Tatarstan.Entities.RealityObjectOutdoor
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Элементы двора
    /// </summary>
    public class RealityObjectOutdoorElementOutdoor : BaseEntity
    {
        /// <summary>
        /// Двор
        /// </summary>
        public virtual RealityObjectOutdoor Outdoor { get; set; }

        /// <summary>
        /// Элемент
        /// </summary>
         public virtual ElementOutdoor Element { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public virtual ConditionElementOutdoor Condition { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal Volume { get; set; }

    }
}
