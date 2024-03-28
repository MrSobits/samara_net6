namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities.HouseManagement;

    /// <summary>
    /// Маппинг Bars.Gkh.Ris.Entities.HouseManagement.RisAddAgreementPayment
    /// </summary>
    public class RisAddAgreementPaymentMap : BaseEntityMap<RisAddAgreementPayment>
    {
        public RisAddAgreementPaymentMap()
            : base("Bars.Gkh.Ris.Entities.HouseManagement.RisAddAgreementPayment", "RIS_ADDAGREEMENTPAYMENT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.AgreementPaymentVersion, "AgreementPaymentVersion").Column("AGREEMENTPAYMENTVERSION").Length(50).NotNull();
            this.Property(x => x.DateFrom, "DateFrom").Column("DATEFROM").NotNull();
            this.Property(x => x.DateTo, "DateTo").Column("DATETO").NotNull();
            this.Property(x => x.Bill, "Bill").Column("BILL");
            this.Property(x => x.Debt, "Debt").Column("DEBT");
            this.Property(x => x.Paid, "Paid").Column("PAID");
        }
    }
}
