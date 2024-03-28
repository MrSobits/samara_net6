/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.RealityObj
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities.RealityObj;
/// 
///     public class RealityObjectBuildingFeatureMap: BaseImportableEntityMap<RealityObjectBuildingFeature>
///     {
///         public RealityObjectBuildingFeatureMap() : base("GKH_RO_BUILD_FEATURE")
///         {
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.BuildingFeature, "BUILDING_FEATURE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.RealityObj
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.RealityObj;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.RealityObj.RealityObjectBuildingFeature"</summary>
    public class RealityObjectBuildingFeatureMap : BaseImportableEntityMap<RealityObjectBuildingFeature>
    {
        
        public RealityObjectBuildingFeatureMap() : 
                base("Bars.Gkh.Entities.RealityObj.RealityObjectBuildingFeature", "GKH_RO_BUILD_FEATURE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.BuildingFeature, "BuildingFeature").Column("BUILDING_FEATURE_ID").NotNull().Fetch();
        }
    }
}
