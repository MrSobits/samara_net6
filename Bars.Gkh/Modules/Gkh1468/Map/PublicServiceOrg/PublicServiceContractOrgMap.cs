namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>Маппинг для "Договор поставщика ресурсов с домом"</summary>
    public class PublicServiceContractOrgMap : BaseImportableEntityMap<PublicServiceOrgContract>
    {
        
        public PublicServiceContractOrgMap() : 
                base("Договор поставщика ресурсов с домом", "GKH_RO_PUB_SERVORG")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DateStart, "Дата начала обслуживания дома").Column("DATE_START");
            this.Property(x => x.DateEnd, "Дата завершения обслуживания по договору").Column("DATE_END");
            this.Property(x => x.ContractNumber, "Номер договора").Column("CONTRACT_NUMBER");
            this.Property(x => x.ContractDate, "Дата договора").Column("CONTRACT_DATE");
            this.Property(x => x.Note, "Примечание").Column("NOTE").Length(300);
            this.Property(x => x.ResOrgReason, "Основание договора с жилым домом").Column("REASON").NotNull();
            this.Property(x => x.DateStop, "Дата расторжения").Column("DATE_STOP");

            this.Property(x => x.TermBillingPaymentNoLaterThan, "Срок выставления счетов к оплате, не позднее").Column("TERM_BILLING_PYMNT_LATER");
            this.Property(x => x.TermPaymentNoLaterThan, "Срок оплаты, не позднее").Column("TERM_PYMNT_LATER");
            this.Property(x => x.DeadlineInformationOfDebt, "Срок предоставления информации о поступивших задолженностях").Column("DEADLINE_INFO_DEBT");
            this.Property(x => x.DayStart, "Дата начала").Column("START_DAY").NotNull();
            this.Property(x => x.DayEnd, "Дата окончания").Column("END_DAY").NotNull();
            this.Property(x => x.StartDeviceMetteringIndication, "Период ввода показаний приборов учёта начинается с").Column("START_PERIOD").NotNull();
            this.Property(x => x.EndDeviceMetteringIndication, "Период ввода показаний приборов учёта оканчивается с").Column("END_PERIOD").NotNull();

            this.Reference(x => x.PublicServiceOrg, "Поставщик ресурсов").Column("PUB_SERVORG_ID").NotNull().Fetch();
            this.Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").Fetch();
            this.Reference(x => x.StopReason, "Основание расторжения").Column("STOP_REASON_ID").Fetch();
        }
    }
}
