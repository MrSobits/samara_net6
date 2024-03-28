/// <mapping-converter-backup>
/// namespace Bars.B4.Modules.Analytics.Reports.Maps
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.B4.Modules.Analytics.Reports.Entities;
/// 
///     public class ReportCustomMap : BaseEntityMap<ReportCustom>
///     {
///         public ReportCustomMap()
///             : base("AL_REPORT_CUSTOM")
///         {
///             Map(x => x.CodedReportKey, "CODED_REPORT_KEY");
///             Map(x => x.Template, "TEMPLATE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.B4.Modules.Analytics.Reports.Map
{
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Mapping.Mappers;
    
    
    /// <summary>Маппинг для "Bars.B4.Modules.Analytics.Reports.Entities.ReportCustom"</summary>
    public class ReportCustomMap : BaseEntityMap<ReportCustom>
    {
        
        public ReportCustomMap() : 
                base("Bars.B4.Modules.Analytics.Reports.Entities.ReportCustom", "AL_REPORT_CUSTOM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CodedReportKey, "CodedReportKey").Column("CODED_REPORT_KEY").Length(250);
            Property(x => x.Template, "Template").Column("TEMPLATE");
        }
    }
}
