namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    public class NormConsumptionMap : BaseEntityMap<NormConsumption>
    {
        public NormConsumptionMap()
            : base("Нормативы потребления", "GKH_TAT_NORM_CONSUMPTION")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Municipality, "Муниципальныйы район").Column("MUNICIPALITY_ID").NotNull().Fetch();
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID").NotNull().Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.Type, "Вид норматива потребления").Column("TYPE").NotNull();
        }
    }
}