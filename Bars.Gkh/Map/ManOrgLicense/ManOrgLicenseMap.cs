namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    /// <summary>Маппинг для "Лицензия управляющей организации"</summary>
    public class ManOrgLicenseMap : BaseImportableEntityMap<ManOrgLicense>
    {
        public ManOrgLicenseMap()
            :
            base("Лицензия управляющей организации", "GKH_MANORG_LICENSE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.LicNum, "Номер для авто генерации").Column("LIC_NUM");
            this.Property(x => x.LicNumber, "строковй номер лицензии на случай если захотят в каком то формате сохранять номер").Column("LIC_NUMBER")
                .Length(100);
            this.Property(x => x.DateIssued, "Дата выдачи").Column("DATE_ISSUED");
            this.Property(x => x.DateValidity, "Срок действия").Column("DATE_VALIDITY");
            this.Property(x => x.DisposalNumber, "Номер приказа о предоставлении лиценизии").Column("NUM_DISPOSAL");
            this.Property(x => x.DateDisposal, "Дата приказа о предоставлении лиценизии").Column("DATE_DISPOSAL");
            this.Property(x => x.DateRegister, "Дата внесения в реестр лицензии").Column("DATE_REGISTER");
            this.Property(x => x.DateTermination, "Дата прекращения").Column("DATE_TERMINATION");
            this.Property(x => x.TypeTermination, "Основание прекращения деятельности").Column("TYPE_TERMINATION");
            this.Property(x => x.OrganizationMadeDecisionTermination, "Организация, принявшая решение").Column("ORG_MADE_DEC_TERMINATION");
            this.Property(x => x.DocumentTermination, "Документ").Column("DOCUMENT_TERMINATION");
            this.Property(x => x.DocumentNumberTermination, "Номер").Column("DOCUMENT_NUMBER_TERMINATION");
            this.Property(x => x.DocumentDateTermination, "Дата").Column("DOCUMENT_DATE_TERMINATION");
            this.Reference(x => x.State, "State").Column("STATE_ID").Fetch();
            this.Reference(x => x.Request, "Ссылка на заявку").Column("REQUEST_ID").Fetch();
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull();
            this.Reference(x => x.TerminationFile, "Файл документа").Column("TERMINATION_FILE_ID");
            this.Reference(x => x.HousingInspection, "Лицензирующий орган").Column("HOUSING_INSPECTION_ID");
            this.Property(x => x.IdSerial, "Серия документа удостоверяющего личность").Column("IDENT_SERIAL").Length(10);
            this.Property(x => x.IdNumber, "Номер документа удостоверяющего личность").Column("IDENT_NUMBER").Length(10);
            this.Property(x => x.IdIssuedDate, "Дата выдачи документа удостоверяющег оличность").Column("IDENT_ISSUEDDATE");
            this.Property(x => x.IdIssuedBy, "Кем выдан документ удостоверяющий личность").Column("IDENT_ISSUEDBY").Length(2000);
            this.Property(x => x.TypeIdentityDocument, "Документ удостоверяющий личность").Column("IDENT_TYPE");
            this.Property(x => x.ERULDate, "ERULDate").Column("ERUL_DATE");
            this.Property(x => x.ERULNumber, "ERULDate").Column("ERUL_NUMBER");
        }
    }
}