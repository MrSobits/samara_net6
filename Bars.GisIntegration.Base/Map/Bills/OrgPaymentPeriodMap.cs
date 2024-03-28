namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.GisIntegration.Base.Map;
    using Entities.Bills;

    public class OrgPaymentPeriodMap : BaseRisEntityMap<OrgPaymentPeriod>
    {
        public OrgPaymentPeriodMap()
            : base("Bars.Gkh.Ris.Entities.Bills.OrgPaymentPeriod", "RIS_PAYMENT_PERIOD")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Month, "Месяц").Column("MONTH");
            this.Property(x => x.Year, "Год").Column("YEAR");
            this.Property(x => x.RisPaymentPeriodType, "Тип платежного периода").Column("PAYMENT_PERIOD_TYPE");
            this.Property(x => x.IsApplied, "IsApplied").Column("IS_APPLIED");
        }
    }
}