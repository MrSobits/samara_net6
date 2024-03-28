namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.GisIntegration.Base.Map;
    using Entities.Bills;

    public class HomePeriodMap : BaseRisEntityMap<HomePeriod>
    {
        public HomePeriodMap()
            : base("Bars.GisIntegration.RegOp.Entities.Bills.HomePeriod", "HOME_PERIOD")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.FIASHouseGuid, "Адрес ФИАС").Column("FIAS_HOUSE_GUID");
            this.Property(x => x.isUO, "Полномочия УО").Column("IS_UO");
        }
    }
}