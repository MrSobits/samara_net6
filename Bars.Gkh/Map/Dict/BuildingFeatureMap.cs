/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities.Dicts;
/// 
///     public class BuildingFeatureMap: BaseImportableEntityMap<BuildingFeature>
///     {
///         public BuildingFeatureMap() : base("GKH_DICT_BUILDING_FEATURE")
///         {
///             Map(x => x.Code, "CODE");
///             Map(x => x.Name, "NAME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.Dicts.BuildingFeature"</summary>
    public class BuildingFeatureMap : BaseImportableEntityMap<BuildingFeature>
    {
        
        public BuildingFeatureMap() : 
                base("Bars.Gkh.Entities.Dicts.BuildingFeature", "GKH_DICT_BUILDING_FEATURE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE");
            Property(x => x.Name, "Name").Column("NAME");
        }
    }
}
