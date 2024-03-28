namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.RegOp.Entities.HouseManagement.RisEntrance"
    /// </summary>
    public class RisEntranceMap : BaseRisEntityMap<RisEntrance>
    {
        public RisEntranceMap() :
            base("Bars.GisIntegration.RegOp.Entities.HouseManagement.RisEntrance", "RIS_ENTRANCE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ApartmentHouse, "ApartmentHouse").Column("HOUSE_ID").Fetch();
            this.Property(x => x.EntranceNum, "EntranceNum").Column("ENTRANCENUM");
            this.Property(x => x.StoreysCount, "StoreysCount").Column("STOREYSCOUNT");
            this.Property(x => x.CreationDate, "CreationDate").Column("CREATIONDATE");
            this.Property(x => x.TerminationDate, "TerminationDate").Column("TERMINATIONDATE");
        }
    }
}