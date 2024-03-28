namespace Bars.GkhGji.Regions.Tomsk.Entities.Inspection
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Первичное обращение проверки
    /// </summary>
    public class PrimaryBaseStatementAppealCits : BaseEntity
    {
        /// <summary>
        /// Связь обращения граждан с проверкой
        /// </summary>
        public virtual InspectionAppealCits BaseStatementAppealCits { get; set; }
    }
}