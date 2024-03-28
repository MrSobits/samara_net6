namespace Bars.GkhGji.Regions.Nnovgorod.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Формулировка нарушения
    /// </summary>
    public class InspectionGjiViolWording : BaseEntity
    {
        /// <summary>
        /// Нарушение проверки
        /// </summary>
        public virtual InspectionGjiViol InspectionViolation { get; set; }
        
        /// <summary>
        /// Формулировка
        /// </summary>
        public virtual string Wording { get; set; }
    }
}