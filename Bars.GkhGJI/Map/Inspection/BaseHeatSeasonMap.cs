/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Основание проверки подготовки к отопительному сезону"
///     /// </summary>
///     public class BaseHeatSeasonMap : SubclassMap<BaseHeatSeason>
///     {
///         public BaseHeatSeasonMap()
///         {
///             Table("GJI_INSPECTION_HEATSEASON");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             References(x => x.HeatingSeason, "HEATSEASON_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Основание подготовка к отопительному сезону"</summary>
    public class BaseHeatSeasonMap : JoinedSubClassMap<BaseHeatSeason>
    {
        
        public BaseHeatSeasonMap() : 
                base("Основание подготовка к отопительному сезону", "GJI_INSPECTION_HEATSEASON")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.HeatingSeason, "Отопительный сезон").Column("HEATSEASON_ID").NotNull();
        }
    }
}
