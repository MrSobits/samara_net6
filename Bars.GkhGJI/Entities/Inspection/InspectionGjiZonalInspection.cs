namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Отделы в проверке
    /// </summary>
    public class InspectionGjiZonalInspection : BaseGkhEntity
    {
        /// <summary>
        /// Проверка ГЖИ
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Отдел
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }
    }
}