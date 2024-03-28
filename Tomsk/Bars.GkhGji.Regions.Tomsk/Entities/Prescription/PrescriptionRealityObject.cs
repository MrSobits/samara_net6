namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Дом предписания
    /// </summary>
    public class PrescriptionRealityObject : BaseEntity
    {
        /// <summary>
        /// Предписание
        /// </summary>
        public virtual Prescription Prescription { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}