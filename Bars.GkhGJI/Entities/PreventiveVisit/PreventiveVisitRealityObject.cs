namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Инспекторы Акта профилактического визита
    /// </summary>
    public class PreventiveVisitRealityObject : BaseEntity
    {
        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual PreventiveVisit PreventiveVisit { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}