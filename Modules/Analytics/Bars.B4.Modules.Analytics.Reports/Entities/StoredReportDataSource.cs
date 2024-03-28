namespace Bars.B4.Modules.Analytics.Reports.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Entities;

    /// <summary>
    /// 
    /// </summary>
    public class StoredReportDataSource: BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual DataSource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual StoredReport StoredReport { get; set; }
    }
}
