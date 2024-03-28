namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    public class PeriodNormConsumptionMap : BaseEntityMap<PeriodNormConsumption>
    {

        public PeriodNormConsumptionMap() :
            base("Договор для объекта строительства", "GKH_TAT_PERIOD_NORM_CONSUMPTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME").NotNull();
            this.Property(x => x.StartDate, "StartDate").Column("START_DATE");
            this.Property(x => x.EndDate, "EndDate").Column("END_DATE");
        }
    }
}


