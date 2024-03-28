namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг сущности Bars.GisIntegration.RegOp.Entities.Bills.RisPaymentInformation
    /// </summary>
    public class RisPaymentInformationMap : BaseRisEntityMap<RisPaymentInformation>
    {
        /// <summary>
        /// Конструктор маппинга сущности Bars.GisIntegration.RegOp.Entities.Bills.RisPaymentInformation
        /// </summary>
        public RisPaymentInformationMap() :
            base("Bars.GisIntegration.RegOp.Entities.Bills.RisPaymentInformation", "RIS_PAYMENT_INFO")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Recipient, "Recipient").Column("RECIPIENT").Length(160);
            this.Property(x => x.BankBik, "BankBik").Column("BANK_BIK").Length(9);
            this.Property(x => x.BankName, "BankName").Column("BANK_NAME").Length(160);
            this.Property(x => x.RecipientInn, "RecipientInn").Column("RECIPIENT_INN").Length(12);
            this.Property(x => x.RecipientKpp, "RecipientKpp").Column("RECIPIENT_KPP").Length(9);
            this.Property(x => x.OperatingAccountNumber, "OperatingAccountNumber").Column("OPERATING_ACCOUNT_NUMBER").Length(20);
            this.Property(x => x.CorrespondentBankAccount, "CorrespondentBankAccount").Column("CORRESPONDENT_BANK_ACCOUNT").Length(20);
        }
    }
}
