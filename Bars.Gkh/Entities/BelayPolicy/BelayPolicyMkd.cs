namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Страховой полис МКД
    /// </summary>
    public class BelayPolicyMkd : BaseGkhEntity
    {
        /// <summary>
        /// Страховой полис
        /// </summary>
        public virtual BelayPolicy BelayPolicy { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Исключен из договора
        /// </summary>
        public virtual bool IsExcluded { get; set; }
    }
}