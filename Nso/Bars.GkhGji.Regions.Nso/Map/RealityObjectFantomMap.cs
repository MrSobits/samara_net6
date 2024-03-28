/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
/// 	using Bars.B4.DataAccess;
/// 	using Bars.GkhGji.Regions.Nso.Entities;
/// 
/// 	public class RealityObjectFantomMap : BaseEntityMap<RealityObjectFantom>
/// 	{
/// 		public RealityObjectFantomMap()
/// 			: base("GJI_REALITY_OBJECT_FANTOM")
/// 		{
/// 			Map(x => x.Fantom, "FANTOM").Length(1000);
/// 
/// 			References(x => x.RealityObject, "REALITY_OBJECT_ID").Fetch.Join();
///             References(x => x.MunicipalityFantom, "MU_FANTOM_ID").Fetch.Join();
///             References(x => x.SettlementFantom, "SETTLEMENT_FANTOM_ID").Fetch.Join();
/// 		}
/// 	}
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.RealityObjectFantom"</summary>
    public class RealityObjectFantomMap : BaseEntityMap<RealityObjectFantom>
    {
        
        public RealityObjectFantomMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.RealityObjectFantom", "GJI_REALITY_OBJECT_FANTOM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Fantom, "Fantom").Column("FANTOM").Length(1000);
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").Fetch();
            Reference(x => x.MunicipalityFantom, "MunicipalityFantom").Column("MU_FANTOM_ID").Fetch();
            Reference(x => x.SettlementFantom, "SettlementFantom").Column("SETTLEMENT_FANTOM_ID").Fetch();
        }
    }
}
