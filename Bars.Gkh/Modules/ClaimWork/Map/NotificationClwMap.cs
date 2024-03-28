/// <mapping-converter-backup>
/// namespace Bars.Gkh.ClaimWork.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Modules.ClaimWork.Entities;
/// 
///     public class NotificationClwMap : BaseJoinedSubclassMap<NotificationClw>
///     {
///         public NotificationClwMap()
///             : base("CLW_NOTIFICATION", "ID")
///         {
///             Map(x => x.SendDate, "SEND_DATE");
///             Map(x => x.DateElimination, "ELIMINATION_DATE");
///             Map(x => x.EliminationMethod, "ELIMINATION_METHOD", false, 1000);
/// 
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    
    
    /// <summary>Маппинг для "Уведомление ПИР"</summary>
    public class NotificationClwMap : JoinedSubClassMap<NotificationClw>
    {
        
        public NotificationClwMap() : 
                base("Уведомление ПИР", "CLW_NOTIFICATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.SendDate, "Дата отправки").Column("SEND_DATE");
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Property(x => x.DateElimination, "Срок устранения").Column("ELIMINATION_DATE");
            Property(x => x.EliminationMethod, "Способ устранения").Column("ELIMINATION_METHOD").Length(1000);
        }
    }
}
