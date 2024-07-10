namespace Bars.Gkh.Gis.Reports.UI.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Список районов для отчета
    /// </summary>
    // TODO При необходимости адаптировать функционал после смены Billing.Core на Dapper
    public class ReportArea : PersistentObject
    {
        /// <summary>
        /// Название района
        /// </summary>
        public virtual string Name { get; set; }
    }
}
