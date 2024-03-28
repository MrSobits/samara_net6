/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Конструктивные элементы жилого дома"
///     /// </summary>
///     public class RealityObjectConstructiveElementMap : BaseGkhEntityMap<RealityObjectConstructiveElement>
///     {
///         public RealityObjectConstructiveElementMap() : base("GKH_OBJ_CONST_ELEMENT")
///         {
///             Map(x => x.LastYearOverhaul, "LAST_YEAR_OVERHAUL");
///             Map(x => x.Volume, "VOLUME");
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ConstructiveElement, "CONST_ELEMENT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Конструктивные элементы жилого дома"</summary>
    public class RealityObjectConstructiveElementMap : BaseImportableEntityMap<RealityObjectConstructiveElement>
    {
        
        public RealityObjectConstructiveElementMap() : 
                base("Конструктивные элементы жилого дома", "GKH_OBJ_CONST_ELEMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.LastYearOverhaul, "Год последнего кап. ремонта").Column("LAST_YEAR_OVERHAUL");
            Property(x => x.Volume, "Объем").Column("VOLUME");
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.ConstructiveElement, "Конструктивный элемент").Column("CONST_ELEMENT_ID").NotNull().Fetch();
        }
    }
}
