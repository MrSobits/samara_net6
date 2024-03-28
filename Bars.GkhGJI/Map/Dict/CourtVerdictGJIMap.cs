/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Решение суда ГЖИ"
///     /// </summary>
///     public class CourtVerdictGjiMap : BaseGkhEntityMap<CourtVerdictGji>
///     {
///         public CourtVerdictGjiMap()
///             : base("GJI_DICT_COURTVERDICT")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Решение суда ГЖИ"</summary>
    public class CourtVerdictGjiMap : BaseEntityMap<CourtVerdictGji>
    {
        
        public CourtVerdictGjiMap() : 
                base("Решение суда ГЖИ", "GJI_DICT_COURTVERDICT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
