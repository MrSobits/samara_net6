namespace Bars.Gkh.Gis.Reports.UI.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Список районов для отчета
    /// </summary>
    public class ReportArea : PersistentObject
    {
        /// <summary>
        /// Название района
        /// </summary>
        public virtual string Name { get; set; }
    }
}
