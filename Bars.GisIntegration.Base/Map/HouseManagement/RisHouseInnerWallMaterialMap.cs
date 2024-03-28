namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    public class RisHouseInnerWallMaterialMap : BaseRisEntityMap<RisHouseInnerWallMaterial>
    {
        public RisHouseInnerWallMaterialMap()
            : base("Bars.Gkh.Ris.Entities.HouseManagement.RisHouseInnerWallMaterial", "RIS_HOUSE_INNER_WALL_MATERIAL")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.House, "House").Column("HOUSE_ID").NotNull();
            this.Property(x => x.InnerWallMaterialCode, "InnerWallMaterialCode").Column("INNERWALLMATERIALCODE").NotNull();
            this.Property(x => x.InnerWallMaterialGuid, "InnerWallMaterialGuid").Column("INNERWALLMATERIALGUID").NotNull();
        }
    }
}
