namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг сущности Bars.GisIntegration.RegOp.Entities.Bills.RisPaymentDocument
    /// </summary>
    public class RisPaymentDocumentMap : BaseRisEntityMap<RisPaymentDocument>
    {
        /// <summary>
        /// Конструктор маппинга сущности Bars.GisIntegration.RegOp.Entities.Bills.RisPaymentDocument
        /// </summary>
        public RisPaymentDocumentMap()
            : base("Bars.GisIntegration.RegOp.Entities.Bills.RisPaymentDocument", "RIS_PAYMENT_DOC")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Account, "Account").Column("ACCOUNT_ID");
            this.Reference(x => x.PaymentInformation, "PaymentInformation").Column("PAYMENT_INFO_ID");
            this.Reference(x => x.AddressInfo, "AddressInfo").Column("ADDRESS_INFO_ID");
            this.Property(x => x.State, "State").Column("STATE");
            this.Property(x => x.PaymentDocumentNumber, "PaymentDocumentNumber").Column("PAYMENT_DOC_NUM").Length(30);
            this.Property(x => x.TotalPiecemealPaymentSum, "TotalPiecemealPaymentSum").Column("TOTAL_PIECEMEAL_SUM");
            this.Property(x => x.Date, "Date").Column("DATE");
            this.Property(x => x.PeriodMonth, "PeriodMonth").Column("PERIODMONTH");
            this.Property(x => x.PeriodYear, "PeriodYear").Column("PERIODYEAR");
            this.Property(x => x.AdvanceBllingPeriod, "AdvanceBllingPeriod").Column("ADVANCE_BLLING_PERIOD");
            this.Property(x => x.DebtPreviousPeriods, "DebtPreviousPeriods").Column("DEBT_PREVIOUS_PERIODS");
            this.Property(x => x.PaymentsTaken, "PaymentsTaken").Column("PAYMENTS_TAKEN");
        }
    }
}
