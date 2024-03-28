namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.Charter"
    /// </summary>
    public class CharterMap : BaseRisEntityMap<Charter>
    {
        public CharterMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.Charter", "RIS_CHARTER")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocNum, "DocNum").Column("DOCNUM").Length(1000);
            this.Property(x => x.DocDate, "DocDate").Column("DOCDATE");
            this.Property(x => x.PeriodMeteringStartDate, "PeriodMeteringStartDate").Column("PERIOD_METERING_STARTDATE");
            this.Property(x => x.PeriodMeteringEndDate, "PeriodMeteringEndDate").Column("PERIOD_METERING_ENDDATE");
            this.Property(x => x.PeriodMeteringLastDay, "PeriodMeteringLastDay").Column("PERIOD_METERING_LASTDAY");
            this.Property(x => x.PaymentDateStartDate, "PaymentDateStartDate").Column("PAYMENT_DATE_STARTDATE");
            this.Property(x => x.PaymentDateLastDay, "PaymentDateLastDay").Column("PAYMENT_DATE_LASTDAY");
            this.Property(x => x.Managers, "Managers").Column("MANAGERS").Length(1000);
            this.Reference(x => x.Head, "Head").Column("HEAD_ID").Fetch();
            this.Reference(x => x.Attachment, "Attachment").Column("ATTACHMENT_ID").Fetch();
            this.Property(x => x.ApprovalCharter, "ApprovalCharter").Column("APPROVALCHARTER");
            this.Property(x => x.RollOverCharter, "RollOverCharter").Column("ROLLOVERCHARTER");
            this.Property(x => x.TerminateCharterDate, "TerminateCharterDate").Column("TERMINATE_CHARTER_DATE");
            this.Property(x => x.TerminateCharterReason, "TerminateCharterReason").Column("TERMINATE_CHARTER_REASON");
            this.Property(x => x.ThisMonthPaymentDocDate, "ThisMonthPaymentDocDate").Column("THIS_MONTH_PAYMENT_DOCDATE");
            this.Property(x => x.PeriodMeteringStartDateThisMonth, "PeriodMeteringStartDateThisMonth").Column("PERIOD_METERING_STARTDATE_THIS_MONTH");
            this.Property(x => x.PeriodMeteringEndDateThisMonth, "PeriodMeteringEndDateThisMonth").Column("PERIOD_METERING_ENDDATE_THIS_MONTH");
            this.Property(x => x.PaymentServicePeriodDate, "Периоды внесения платы за ЖКУ: День месяца").Column("PAYMENT_SERV_DATE");
            this.Property(x => x.ThisMonthPaymentServiceDate, "Периоды внесения платы за ЖКУ - этого месяца (если false - следующего месяца)").Column("PAYMENT_SERV_DATE_THIS_MONTH");
            this.Property(x => x.IsManagedByContract, "Управление многоквартирным домом осуществляется управляющей организацией по договору управления").Column("IS_MANAGED_BY_CONTRACT");
        }
    }
}