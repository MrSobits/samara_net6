namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дома деятельности ТСЖ
    /// </summary>
    public class ActivityTsjRealObj : BaseGkhEntity
    {
        /// <summary>
        /// Деятельность ТСЖ
        /// </summary>
        public virtual ActivityTsj ActivityTsj { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}