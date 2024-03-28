/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tyumen.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tyumen.Entities;
/// 
///     public class NetworkOperatorRealityObjectMap : BaseEntityMap<NetworkOperatorRealityObject>
///     {
///         public NetworkOperatorRealityObjectMap() : base("GKH_NOP_RO")
///         {
///             References(x => x.NetworkOperator, "NOPERATOR_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             Map(x => x.Bandwidth, "BANDWIDTH", true, 100);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tyumen.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tyumen.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tyumen.Entities.NetworkOperatorRealityObject"</summary>
    public class NetworkOperatorRealityObjectMap : BaseEntityMap<NetworkOperatorRealityObject>
    {
        
        public NetworkOperatorRealityObjectMap() : 
                base("Bars.GkhGji.Regions.Tyumen.Entities.NetworkOperatorRealityObject", "GKH_NOP_RO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.NetworkOperator, "NetworkOperator").Column("NOPERATOR_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();
            Property(x => x.Bandwidth, "Bandwidth").Column("BANDWIDTH").Length(100).NotNull();
        }
    }
}
