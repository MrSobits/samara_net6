namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.GisIntegration.Base.Map;
    using Entities.Bills;

    /// <summary>
    /// Маппинг сущности Bars.Gkh.Ris.Entities.Bills.RisInsurance
    /// </summary>
    public class RisInsuranceMap : BaseRisEntityMap<RisInsurance>
    {
        /// <summary>
        /// Конструктор маппинга сущности Bars.GisIntegration.RegOp.Map.Bills.RisAddressInfo
        /// </summary>
        public RisInsuranceMap() :
            base("Bars.GisIntegration.RegOp.Map.Bills.RisInsurance", "RIS_INSURANCE")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.InsuranceProductGuid, "InsuranceProductGuid").Column("INSURANCE_PRODUCT_GUID");
            this.Property(x => x.Rate, "Rate").Column("RATE");
            this.Property(x => x.AccountingPeriodTotal, "AccountingPeriodTotal").Column("ACCOUNTING_PERIOD_TOTAL");
            this.Property(x => x.CalcExplanation, "CalcExplanation").Column("CALC_EXPLANATION");
            this.Property(x => x.TotalPayable, "TotalPayable").Column("TOTAL_PAYABLE");
            this.Property(x => x.MoneyDiscount, "MoneyDiscount").Column("MONEY_DISCOUNT");
            this.Property(x => x.MoneyRecalculation, "MoneyRecalculation").Column("MONEY_RECALCULATION");
        }
    }
}
