namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    public class RealityObjectLiftSumMap : BaseImportableEntityMap<RealityObjectLiftSum>
    {
        public RealityObjectLiftSumMap() : base("GKH_RO_LIFT_SUM")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Hinged, "HINGED").Column("HINGED");
            this.Property(x => x.Lowerings, "LOWERINGS").Column("LOWERINGS");
            this.Property(x => x.MjiCount, "MJI_COUNT").Column("MJI_COUNT");
            this.Property(x => x.MjiPassenger, "MJI_PASSENGER").Column("MJI_PASSENGER");
            this.Property(x => x.MjiCargo, "MJI_CARGO").Column("MJI_CARGO");
            this.Property(x => x.MjiMixed, "MJI_MIXED").Column("MJI_MIXED");
            this.Property(x => x.Risers, "RISERS").Column("RISERS");
            this.Property(x => x.ShaftCount, "SHAFT_COUNT").Column("SHAFT_COUNT");
            this.Property(x => x.BtiCount, "BTI_COUNT").Column("BTI_COUNT");
            this.Property(x => x.BtiPassenger, "BTI_PASSENGER").Column("BTI_PASSENGER");
            this.Property(x  => x.BtiCargo, "BTI_CARGO").Column("BTI_CARGO");
            this.Property(x => x.BtiMixed, "BTI_MIXED").Column("BTI_MIXED");

            this.Reference(x => x.RealityObject, "RO_ID").Column("RO_ID").NotNull().Fetch();
        }
    }
}