/// <mapping-converter-backup>
/// namespace Bars.Gkh.Reforma.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Reforma.Entities.Log;
/// 
///     public class SessionLogItemMap : BaseEntityMap<SessionLogItem>
///     {
///         public SessionLogItemMap()
///             : base("RFRM_SESSION_LOG")
///         {
///             this.Map(x => x.SessionId, "SESSION_ID", true);
///             this.Map(x => x.StartTime, "START_TIME", true);
///             this.Map(x => x.EndTime, "END_TIME", false);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Reforma.Map.Log
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Reforma.Entities.Log;
    
    
    /// <summary>Маппинг для "Запись лога о сессии синхронизации"</summary>
    public class SessionLogItemMap : BaseEntityMap<SessionLogItem>
    {
        
        public SessionLogItemMap() : 
                base("Запись лога о сессии синхронизации", "RFRM_SESSION_LOG")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.SessionId, "Идентификатор сессии").Column("SESSION_ID").Length(250).NotNull();
            this.Property(x => x.StartTime, "Время открытия сессии").Column("START_TIME").NotNull();
            this.Property(x => x.EndTime, "Время закрытия сессии").Column("END_TIME");
            this.Property(x => x.TypeIntegration, "Тип интеграции").Column("TYPE_INTEGRATION").NotNull();
        }
    }
}
