/// <mapping-converter-backup>
/// namespace Bars.Gkh.Reforma.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Reforma.Entities.Log;
/// 
///     public class ActionLogItemMap : BaseEntityMap<ActionLogItem>
///     {
///         public ActionLogItemMap()
///             : base("RFRM_ACTION_LOG")
///         {
///             this.Map(x => x.ErrorCode, "ERROR_CODE", false);
///             this.Map(x => x.ErrorDescription, "ERROR_DESCRIPTION", false);
///             this.Map(x => x.ErrorName, "ERROR_NAME", false);
///             this.Map(x => x.Name, "NAME", true);
///             this.Map(x => x.Parameters, "PARAMETERS", true);
///             this.Map(x => x.RequestTime, "REQUEST_TIME", true);
///             this.Map(x => x.ResponseTime, "RESPONSE_TIME", true);
///             this.Map(x => x.Success, "SUCCESS", true);
///             this.Map(x => x.Details, "DETAILS", false);
/// 
///             this.References(x => x.Session, "SESSION_LOG_ITEM_ID", ReferenceMapConfig.NotNull);
///             this.References(x => x.Packets, "PACKETS_FILE_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Reforma.Map.Log
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Reforma.Entities.Log;
    
    
    /// <summary>Маппинг для "Запись лога о произведенной в рамках сессии операции"</summary>
    public class ActionLogItemMap : BaseEntityMap<ActionLogItem>
    {
        
        public ActionLogItemMap() : 
                base("Запись лога о произведенной в рамках сессии операции", "RFRM_ACTION_LOG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Session, "Сессия").Column("SESSION_LOG_ITEM_ID").NotNull();
            Property(x => x.Name, "Имя действия").Column("NAME").Length(250).NotNull();
            Property(x => x.Parameters, "Параметры вызова действия").Column("PARAMETERS").NotNull();
            Property(x => x.Details, "Детали действия").Column("DETAILS").Length(250);
            Property(x => x.Success, "Признак успешности").Column("SUCCESS").NotNull();
            Property(x => x.ErrorCode, "Код ошибки").Column("ERROR_CODE").Length(250);
            Property(x => x.ErrorName, "Название ошибки").Column("ERROR_NAME").Length(250);
            Property(x => x.ErrorDescription, "Описание ошибки").Column("ERROR_DESCRIPTION").Length(250);
            Property(x => x.RequestTime, "Время запроса").Column("REQUEST_TIME").NotNull();
            Property(x => x.ResponseTime, "Время ответа").Column("RESPONSE_TIME").NotNull();
            Reference(x => x.Packets, "Отправленные/полученные пакеты").Column("PACKETS_FILE_ID");
        }
    }
}
