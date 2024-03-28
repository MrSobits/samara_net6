/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
/// 	using Bars.B4.DataAccess;
/// 	using Bars.GkhGji.Regions.Chelyabinsk.Entities;
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

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.RealityObjectFantom"</summary>
    public class RealityObjectFantomMap : BaseEntityMap<RealityObjectFantom>
    {
        
        public RealityObjectFantomMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.RealityObjectFantom", "GJI_REALITY_OBJECT_FANTOM")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Fantom, "Fantom").Column("FANTOM").Length(1000);
            this.Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").Fetch();
            this.Reference(x => x.MunicipalityFantom, "MunicipalityFantom").Column("MU_FANTOM_ID").Fetch();
            this.Reference(x => x.SettlementFantom, "SettlementFantom").Column("SETTLEMENT_FANTOM_ID").Fetch();
        }
    }
}
