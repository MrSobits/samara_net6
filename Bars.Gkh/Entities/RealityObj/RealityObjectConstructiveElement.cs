namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Конструктивные элементы жилого дома
    /// </summary>
    public class RealityObjectConstructiveElement : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Год последнего кап. ремонта
        /// </summary>
        public virtual int LastYearOverhaul { get; set; }

        /// <summary>
        /// Конструктивный элемент
        /// </summary>
        public virtual ConstructiveElement ConstructiveElement { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal? Volume { get; set; }
    }
}
