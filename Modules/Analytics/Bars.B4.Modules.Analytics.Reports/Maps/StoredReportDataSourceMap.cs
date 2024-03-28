/// <mapping-converter-backup>
/// namespace Bars.B4.Modules.Analytics.Reports.Maps
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.B4.Modules.Analytics.Reports.Entities;
/// 
///     public class StoredReportDataSourceMap : BaseEntityMap<StoredReportDataSource>
///     {
///         public StoredReportDataSourceMap()
///             : base("AL_REPORT_DATASOURCE")
///         {
///             References(x => x.DataSource, "DATA_SOURCE_ID");
///             References(x => x.StoredReport, "STORED_REPORT_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.B4.Modules.Analytics.Reports.Map
{
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Mapping.Mappers;
    
    
    /// <summary>Маппинг для "Bars.B4.Modules.Analytics.Reports.Entities.StoredReportDataSource"</summary>
    public class StoredReportDataSourceMap : BaseEntityMap<StoredReportDataSource>
    {
        
        public StoredReportDataSourceMap() : 
                base("Bars.B4.Modules.Analytics.Reports.Entities.StoredReportDataSource", "AL_REPORT_DATASOURCE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.DataSource, "DataSource").Column("DATA_SOURCE_ID");
            Reference(x => x.StoredReport, "StoredReport").Column("STORED_REPORT_ID");
        }
    }
}
