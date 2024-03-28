namespace Bars.GisIntegration.Base.Map.CapitalRepair
{
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Map;

    public class RisCrContractMap : BaseRisEntityMap<RisCrContract>
    {
        public RisCrContractMap()
            : base(
                "Bars.GisIntegration.CapitalReapair.Entities.RisCrContract",
                "GI_CR_CONTRACT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Number, "Number").Column("NUMBER").Length(100);
            this.Property(x => x.PlanGUID, "PlanGUID").Column("PLAN_GUID").Length(50);
            this.Property(x => x.Date, "Date").Column("DATE");
            this.Property(x => x.StartDate, "StartDate").Column("START_DATE");
            this.Property(x => x.EndDate, "EndDate").Column("END_DATE");
            this.Property(x => x.Sum, "Sum").Column("SUM");
            this.Property(x => x.Customer, "Customer").Column("CUSTOMER").Length(500);
            this.Property(x => x.Performer, "Performer").Column("PERFORMER").Length(500);
            this.Property(x => x.OutlayMissing, "OutlayMissing").Column("OUTLAY_MISSING");
        }
    }
}
