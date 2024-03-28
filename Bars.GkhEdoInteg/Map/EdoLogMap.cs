/// <mapping-converter-backup>
/// namespace Bars.GkhEdoInteg.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhEdoInteg.Entities;
/// 
///     public class LogRequestsMap : BaseEntityMap<LogRequests>
///     {
///         public LogRequestsMap()
///             : base("INTGEDO_LOGR")
///         {
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.Count, "COUNT");
///             Map(x => x.TimeExecution, "TIME_EXECUTION").Length(50);
///             Map(x => x.CountAdded, "COUNT_ADDED");
///             Map(x => x.CountUpdated, "COUNT_UPDATED");
///             Map(x => x.Uri, "URI");
/// 
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhEdoInteg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhEdoInteg.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhEdoInteg.Entities.LogRequests"</summary>
    public class LogRequestsMap : BaseEntityMap<LogRequests>
    {
        
        public LogRequestsMap() : 
                base("Bars.GkhEdoInteg.Entities.LogRequests", "INTGEDO_LOGR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Property(x => x.Count, "Count").Column("COUNT");
            Property(x => x.TimeExecution, "TimeExecution").Column("TIME_EXECUTION").Length(50);
            Property(x => x.CountAdded, "CountAdded").Column("COUNT_ADDED");
            Property(x => x.CountUpdated, "CountUpdated").Column("COUNT_UPDATED");
            Property(x => x.Uri, "Uri").Column("URI");
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
        }
    }
}
