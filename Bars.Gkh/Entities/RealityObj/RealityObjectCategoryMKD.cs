namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Подъезд
    /// </summary>
    public class RealityObjectCategoryMKD : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// КАтегория
        /// </summary>
        public virtual CategoryCSMKD CategoryCSMKD { get; set; }


    }
}