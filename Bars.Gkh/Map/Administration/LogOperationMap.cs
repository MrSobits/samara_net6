/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Entities;
/// 
///     public class LogOperationMap : BaseEntityMap<LogOperation>
///     {
///         public LogOperationMap()
///             : base("GKH_LOG_OPERATION")
///         {
///             Map(x => x.StartDate, "START_DATE", true);
///             Map(x => x.EndDate, "END_DATE", true);
/// 			Map(x => x.Comment, "LOG_COMMENT", false, 2000);
///             Map(x => x.OperationType, "OPERATION_TYPE");
///             
///             References(x => x.User, "USER_ID", ReferenceMapConfig.Fetch);
///             References(x => x.LogFile, "LOG_FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.LogOperation"</summary>
    public class LogOperationMap : BaseEntityMap<LogOperation>
    {
        
        public LogOperationMap() : 
                base("Bars.Gkh.Entities.LogOperation", "GKH_LOG_OPERATION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.User, "Пользователь").Column("USER_ID").Fetch();
            Property(x => x.StartDate, "Время старта").Column("START_DATE").NotNull();
            Property(x => x.EndDate, "Время окончания").Column("END_DATE").NotNull();
            Property(x => x.Comment, "Комментарий").Column("LOG_COMMENT").Length(2000);
            Reference(x => x.LogFile, "Файл лога").Column("LOG_FILE_ID").Fetch();
            Property(x => x.OperationType, "Тип операций").Column("OPERATION_TYPE");
        }
    }
}
