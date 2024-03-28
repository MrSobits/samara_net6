/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Вид работ уведомлений"
///     /// </summary>
///     public class KindWorkNotifGjiMap : BaseGkhEntityMap<KindWorkNotifGji>
///     {
///         public KindWorkNotifGjiMap()
///             : base("GJI_DICT_KIND_WORK")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Code, "CODE").Length(50);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Вид работ уведомлений ГЖИ"</summary>
    public class KindWorkNotifGjiMap : BaseEntityMap<KindWorkNotifGji>
    {
        
        public KindWorkNotifGjiMap() : 
                base("Вид работ уведомлений ГЖИ", "GJI_DICT_KIND_WORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Code, "Код").Column("CODE").Length(50);
        }
    }
}
