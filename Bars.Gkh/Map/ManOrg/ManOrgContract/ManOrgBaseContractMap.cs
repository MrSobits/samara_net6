namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Базовый класс договоров управления"</summary>
    public class ManOrgBaseContractMap : BaseImportableEntityMap<ManOrgBaseContract>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ManOrgBaseContractMap() : 
                base("Базовый класс договоров управления", "GKH_MORG_CONTRACT")
        {
        }

        /// <summary>
        /// Map
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.TypeContractManOrgRealObj, "Тип договора управляющей организации").Column("TYPE_CONTRACT").NotNull();
            this.Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.StartDate, "Дата начала управления").Column("START_DATE");
            this.Property(x => x.EndDate, "Дата окончания управления").Column("END_DATE");
            this.Property(x => x.PlannedEndDate, "Плановая дата окончания").Column("PLANNED_END_DATE");
            this.Property(x => x.Note, "Примечание").Column("NOTE").Length(300);
            this.Property(x => x.TerminateReason, "Основание расторжения").Column("TERMINATE_REASON").Length(300);
            this.Property(x => x.TerminationDate, "Дата расторжения").Column("TERMINATION_DATE");
            this.Property(x => x.ContractStopReason, "Основание прекращения обслуживания").Column("CONTRACT_STOP_REASON").NotNull();
            this.Property(x => x.PaymentServicePeriodDate, "Периоды внесения платы за ЖКУ: День месяца").Column("PAYMENT_SERV_DATE");
            this.Property(x => x.IsLastDayPaymentServicePeriodDate, "Периоды внесения платы за ЖКУ: последний день месяца").Column("IS_LAST_DAY_PAYMENT_SERV_DATE");
            this.Property(x => x.ThisMonthPaymentDocDate, "День выставления платежных документов - этого месяца").Column("DRAWING_PD_DATE_THIS_MONTH");
            this.Property(x => x.ThisMonthPaymentServiceDate, "Периоды внесения платы за ЖКУ - этого месяца (если false - следующего месяца)").Column("PAYMENT_SERV_DATE_THIS_MONTH");

            this.Property(x => x.IsLastDayMeteringDeviceValuesBeginDate, "Последний день месяца").Column("IS_LAST_DAY_MDV_BEGIN_DATE");
            this.Property(x => x.IsLastDayMeteringDeviceValuesEndDate, "Последний день месяца").Column("IS_LAST_DAY_MDV_END_DATE");
            this.Property(x => x.IsLastDayDrawingPaymentDocument, "Последний день месяца").Column("IS_LAST_DAY_DRAWING_PD_DATE");

            this.Property(x => x.ThisMonthInputMeteringDeviceValuesBeginDate, "День месяца начала ввода показаний по приборам учета - этого месяца (если false - следующего месяца)")
                .Column("MD_BEGIN_DATE_THIS_MONTH");
            this.Property(x => x.ThisMonthInputMeteringDeviceValuesEndDate, "День месяца окончания ввода показаний по приборам учета - этого месяца (если false - следующего месяца)")
                .Column("MD_END_DATE_THIS_MONTH");

            this.Property(x => x.StartDatePaymentPeriod, "Сведения о плате - дата начала периода").Column("START_DATE_PAYMENT_PERIOD");
            this.Property(x => x.EndDatePaymentPeriod, "Сведения о плате - дата окончания периода").Column("END_DATE_PAYMENT_PERIOD");
            this.Property(x => x.SetPaymentsFoundation, "Сведения о плате - Основание установления размера платы за содержание жилого помещения").Column("SET_PAYMENTS_FOUNDATION");
            this.Property(x => x.RevocationReason, "Сведения о плате - Причина аннулирования").Column("REVOCATION_REASON");

            this.Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANAG_ORG_ID").Fetch();
            this.Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").Fetch();
        }
    }
}
