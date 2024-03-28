namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг сущности Bars.GisIntegration.RegOp.Entities.Bills.RisCapitalRepairCharge
    /// </summary>
    public class RisCapitalRepairChargeMap : BaseRisEntityMap<RisCapitalRepairCharge>
    {
        /// <summary>
        /// Конструктор маппинга сущности Bars.GisIntegration.RegOp.Entities.Bills.RisCapitalRepairCharge
        /// </summary>
        public RisCapitalRepairChargeMap() :
            base("Bars.GisIntegration.RegOp.Entities.Bills.RisCapitalRepairCharge", "RIS_CAPITAL_REPAIR_CHARGE")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.PaymentDocument, "PaymentDocument").Column("PAYMENT_DOC_ID");
            this.Property(x => x.Contribution, "Contribution").Column("CONTRIBUTION");
            this.Property(x => x.AccountingPeriodTotal, "AccountingPeriodTotal").Column("ACCOUNTING_PERIOD_TOTAL");
            this.Property(x => x.MoneyRecalculation, "MoneyRecalculation").Column("MONEY_RECALCULATION");
            this.Property(x => x.MoneyDiscount, "MoneyDiscount").Column("MONEY_DISCOUNT");
            this.Property(x => x.TotalPayable, "TotalPayable").Column("TOTAL_PAYABLE");
            this.Property(x => x.OrgPpaguidCapitalRepairCharge, "OrgPpaguidCapitalRepairCharge").Column("ORG_PPAGUID_REPAIRE_CHARGE");
        }
    }
}
