/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Эксперты"
///     /// </summary>
///     public class ExpertGjiMap : BaseGkhEntityMap<ExpertGji>
///     {
///         public ExpertGjiMap(): base("GJI_DICT_EXPERT")
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
    
    
    /// <summary>Маппинг для "Эксперты ГЖИ"</summary>
    public class ExpertGjiMap : BaseEntityMap<ExpertGji>
    {
        
        public ExpertGjiMap() : 
                base("Эксперты ГЖИ", "GJI_DICT_EXPERT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.ExpertType, "Тип эксперта").Column("EXPERT_TYPE");
        }
    }
}
