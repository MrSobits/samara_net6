﻿namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisContract"
    /// </summary>
    public class RisContractMap : BaseRisEntityMap<RisContract>
    {

        public RisContractMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisContract", "RIS_CONTRACT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocNum, "DocNum").Column("DOCNUM").Length(200);
            this.Property(x => x.SigningDate, "SigningDate").Column("SIGNINGDATE");
            this.Property(x => x.EffectiveDate, "EffectiveDate").Column("EFFECTIVEDATE");
            this.Property(x => x.PlanDateComptetion, "PlanDateComptetion").Column("PLANDATECOMPTETION");
            this.Property(x => x.ValidityMonth, "ValidityMonth").Column("VALIDITY_MONTH");
            this.Property(x => x.ValidityYear, "ValidityYear").Column("VALIDITY_YEAR");
            this.Property(x => x.OwnersType, "OwnersType").Column("OWNERS_TYPE");
            this.Reference(x => x.Org, "Org").Column("ORG_ID").Fetch();
            this.Property(x => x.ProtocolPurchaseNumber, "ProtocolPurchaseNumber").Column("PROTOCOL_PURCHASENUMBER").Length(200);
            this.Property(x => x.ContractBaseCode, "ContractBaseCode").Column("CONTRACTBASE_CODE").Length(200);
            this.Property(x => x.ContractBaseGuid, "ContractBaseGuid").Column("CONTRACTBASE_GUID").Length(200);
            this.Property(x => x.InputMeteringDeviceValuesBeginDate, "InputMeteringDeviceValuesBeginDate").Column("IMD_VALUES_BEGINDATE");
            this.Property(x => x.InputMeteringDeviceValuesEndDate, "InputMeteringDeviceValuesEndDate").Column("IMD_VALUES_ENDDATE");
            this.Property(x => x.DrawingPaymentDocumentDate, "DrawingPaymentDocumentDate").Column("PAYMENT_DOC_DATE");
            this.Property(x => x.ThisMonthPaymentDocDate, "ThisMonthPaymentDocDate")
                .Column("DRAWING_PD_DATE_THIS_MONTH");
            this.Property(x => x.LicenseRequest, "LicenseRequest").Column("LICENSE_REQUEST");
            this.Property(x => x.PeriodMeteringStartDateThisMonth, "PeriodMeteringStartDateThisMonth").Column("PERIOD_METERING_STARTDATE_THIS_MONTH");
            this.Property(x => x.PeriodMeteringEndDateThisMonth, "PeriodMeteringEndDateThisMonth").Column("PERIOD_METERING_ENDDATE_THIS_MONTH");
            this.Property(x => x.PaymentServicePeriodDate, "Периоды внесения платы за ЖКУ: День месяца").Column("PAYMENT_SERV_DATE");
            this.Property(x => x.ThisMonthPaymentServiceDate, "Периоды внесения платы за ЖКУ - этого месяца (если false - следующего месяца)").Column("PAYMENT_SERV_DATE_THIS_MONTH");
        }
    }
}
