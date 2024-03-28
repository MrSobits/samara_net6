/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tyumen.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tyumen.Entities;
/// 
///     public class NetworkOperatorRealityObjectTechDecisionMap : BaseEntityMap<NetworkOperatorRealityObjectTechDecision>
///     {
///         public NetworkOperatorRealityObjectTechDecisionMap() : base("GKH_NOP_RO_TDEC")
///         {
///             References(x => x.NetworkOperatorRealityObject, "NOP_RO_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             References(x => x.TechDecision, "TDEC_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tyumen.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tyumen.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tyumen.Entities.NetworkOperatorRealityObjectTechDecision"</summary>
    public class NetworkOperatorRealityObjectTechDecisionMap : BaseEntityMap<NetworkOperatorRealityObjectTechDecision>
    {
        
        public NetworkOperatorRealityObjectTechDecisionMap() : 
                base("Bars.GkhGji.Regions.Tyumen.Entities.NetworkOperatorRealityObjectTechDecision", "GKH_NOP_RO_TDEC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.NetworkOperatorRealityObject, "NetworkOperatorRealityObject").Column("NOP_RO_ID").NotNull().Fetch();
            Reference(x => x.TechDecision, "TechDecision").Column("TDEC_ID").NotNull().Fetch();
        }
    }
}
