/// <mapping-converter-backup>
/// namespace Bars.Gkh.Regions.Yanao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Regions.Yanao.Entities;
/// 
///     public class RealityObjectExtensionMap : BaseEntityMap<RealityObjectExtension> 
///     {
///         public RealityObjectExtensionMap()
///             : base("GKH_YANAO_ROBJ_EXTENSION")
///         {
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.TechPassportScanFile, "TECHPASSPORT_SCAN_FILE_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Regions.Yanao.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Yanao.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Regions.Yanao.Entities.RealityObjectExtension"</summary>
    public class RealityObjectExtensionMap : BaseEntityMap<RealityObjectExtension>
    {
        
        public RealityObjectExtensionMap() : 
                base("Bars.Gkh.Regions.Yanao.Entities.RealityObjectExtension", "GKH_YANAO_ROBJ_EXTENSION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.TechPassportScanFile, "TechPassportScanFile").Column("TECHPASSPORT_SCAN_FILE_ID").NotNull().Fetch();
        }
    }
}
