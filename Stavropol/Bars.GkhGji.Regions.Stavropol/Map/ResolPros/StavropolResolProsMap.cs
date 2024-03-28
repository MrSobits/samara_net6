/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Stavropol.Map
/// {
///     using Bars.GkhGji.Regions.Stavropol.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Постановление прокуратуры" для Ставрополя
///     /// </summary>
///     public class StavropolResolProsMap : SubclassMap<StavropolResolPros>
///     {
///         public StavropolResolProsMap()
///         {
///             Table("GJI_RESOL_PROS_STAVROPOL");
///             KeyColumn("ID");
///             Map(x => x.Official, "OFFICIAL");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Stavropol.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Stavropol.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Stavropol.Entities.StavropolResolPros"</summary>
    public class StavropolResolProsMap : JoinedSubClassMap<StavropolResolPros>
    {
        
        public StavropolResolProsMap() : 
                base("Bars.GkhGji.Regions.Stavropol.Entities.StavropolResolPros", "GJI_RESOL_PROS_STAVROPOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Official, "Official").Column("OFFICIAL");
        }
    }
}
