namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг сущности Bars.GisIntegration.RegOp.Entities.Bills.RisCapitalRepairDebt
    /// </summary>
    public class RisCapitalRepairDebtMap : BaseRisEntityMap<RisCapitalRepairDebt>
    {
        /// <summary>
        /// Конструктор маппинга сущности Bars.GisIntegration.RegOp.Entities.Bills.RisCapitalRepairDebt
        /// </summary>
        public RisCapitalRepairDebtMap() :
            base("Bars.GisIntegration.RegOp.Entities.Bills.RisCapitalRepairDebt", "RIS_CAPITAL_REPAIR_DEBT")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.PaymentDocument, "PaymentDocument").Column("PAYMENT_DOC_ID");
            this.Property(x => x.Month, "Month").Column("MONTH");
            this.Property(x => x.Year, "Year").Column("YEAR");
            this.Property(x => x.TotalPayable, "TotalPayable").Column("TOTAL_PAYABLE");
            this.Property(x => x.OrgPpaguid, "OrgPpaguid").Column("ORG_PPAGUID");
        }
    }
}
