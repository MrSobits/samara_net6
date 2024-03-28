/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Инспектируемая часть"
///     /// </summary>
///     public class InspectedPartGjiMap : BaseGkhEntityMap<InspectedPartGji>
///     {
///         public InspectedPartGjiMap() : base("GJI_DICT_INSPECTEDPART")
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
    
    
    /// <summary>Маппинг для "Инспектируемая часть ГЖИ"</summary>
    public class InspectedPartGjiMap : BaseEntityMap<InspectedPartGji>
    {
        
        public InspectedPartGjiMap() : 
                base("Инспектируемая часть ГЖИ", "GJI_DICT_INSPECTEDPART")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
