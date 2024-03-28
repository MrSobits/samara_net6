/// <mapping-converter-backup>
/// namespace Bars.GkhEdoInteg.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhEdoInteg.Entities;
/// 
///     public class LogRequestsAppCitsEdoMap : BaseEntityMap<LogRequestsAppCitsEdo>
///     {
///         public LogRequestsAppCitsEdoMap()
///             : base("INTGEDO_LOGR_APPCITS")
///         {
///             References(x => x.LogRequests, "LOGR_ID").Not.Nullable().Fetch.Join();
///             References(x => x.AppealCitsCompareEdo, "APPCITS_COMPEDO_ID").Not.Nullable().Fetch.Join();
/// 
///             Map(x => x.ActionIntegrationRow, "ACTION_IMPORT_ROW");
///             Map(x => x.DateActual, "DATE_ACTUAL");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhEdoInteg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhEdoInteg.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhEdoInteg.Entities.LogRequestsAppCitsEdo"</summary>
    public class LogRequestsAppCitsEdoMap : BaseEntityMap<LogRequestsAppCitsEdo>
    {
        
        public LogRequestsAppCitsEdoMap() : 
                base("Bars.GkhEdoInteg.Entities.LogRequestsAppCitsEdo", "INTGEDO_LOGR_APPCITS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ActionIntegrationRow, "ActionIntegrationRow").Column("ACTION_IMPORT_ROW");
            Property(x => x.DateActual, "DateActual").Column("DATE_ACTUAL");
            Reference(x => x.LogRequests, "LogRequests").Column("LOGR_ID").NotNull().Fetch();
            Reference(x => x.AppealCitsCompareEdo, "AppealCitsCompareEdo").Column("APPCITS_COMPEDO_ID").NotNull().Fetch();
        }
    }
}
