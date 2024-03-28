/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.Gkh.Enums;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Подготовка к отопительному сезону"
///     /// </summary>
///     public class HeatSeasonMap : BaseGkhEntityMap<HeatSeason>
///     {
///         public HeatSeasonMap() : base("GJI_HEATSEASON")
///         {
///             Map(x => x.DateHeat, "DATE_HEAT");
///             Map(x => x.HeatingSystem, "HEATING_SYSTEM").Not.Nullable().CustomType<HeatingSystem>();
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().LazyLoad();
///             References(x => x.Period, "HEATSEASON_PERIOD_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Подготовка к отопительному сезону (не путать с проверкой по отопительному сезону)"</summary>
    public class HeatSeasonMap : BaseEntityMap<HeatSeason>
    {
        
        public HeatSeasonMap() : 
                base("Подготовка к отопительному сезону (не путать с проверкой по отопительному сезону)" +
                        "", "GJI_HEATSEASON")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DateHeat, "Дата пуска тепла в дом").Column("DATE_HEAT");
            Property(x => x.HeatingSystem, "Система отопления (по умолчанию тянется из Жилого дома)").Column("HEATING_SYSTEM").NotNull();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull();
            Reference(x => x.Period, "Период отопительного сезона").Column("HEATSEASON_PERIOD_ID").NotNull();
        }
    }
}
