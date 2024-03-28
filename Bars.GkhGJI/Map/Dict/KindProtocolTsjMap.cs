/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Вид протокола ТСЖ ГЖИ"
///     /// </summary>
///     public class KindProtocolTsjMap : BaseGkhEntityMap<KindProtocolTsj>
///     {
///         public KindProtocolTsjMap()
///             : base("GJI_DICT_KIND_PROT")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(50);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Вид протокола ТСЖ ГЖИ"</summary>
    public class KindProtocolTsjMap : BaseEntityMap<KindProtocolTsj>
    {
        
        public KindProtocolTsjMap() : 
                base("Вид протокола ТСЖ ГЖИ", "GJI_DICT_KIND_PROT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(50);
        }
    }
}
