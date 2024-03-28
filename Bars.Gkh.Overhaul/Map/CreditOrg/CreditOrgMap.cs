namespace Bars.Gkh.Overhaul.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Entities.CreditOrg"</summary>
    public class CreditOrgMap : BaseImportableEntityMap<CreditOrg>
    {

        public CreditOrgMap() : base("OVRHL_CREDIT_ORG")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExportId, "ExportId").Column("EXPORT_ID").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            this.Property(x => x.PrintName, "Наименование для печати в квитанциях").Column("PRINTNAME").Length(300);
            this.Property(x => x.IsFilial, "Является филиалом").Column("IS_FILIAL").NotNull();
            this.Property(x => x.Address, "Строковый адрес фиас (Юридический адрес)").Column("ADDRESS").Length(500);
            this.Property(x => x.AddressOutSubject, "Адрес за пределами субъекта (Юридический адрес)").Column("ADDRESS_OUT_SUBJECT").Length(500);
            this.Property(x => x.MailingAddressOutSubject, "Адрес за пределами субъекта (Почтовый адрес)").Column("MAILADDR_OUT_SUBJ").Length(500);
            this.Property(x => x.IsAddressOut, "Адрес за пределами субъекта (Юридический адрес)").Column("IS_ADDRESS_OUT").NotNull();
            this.Property(x => x.IsMailingAddressOut, "Адрес за пределами субъекта (Почтовый адрес)").Column("IS_MAILADDR_OUT").NotNull();
            this.Property(x => x.Inn, "ИНН").Column("INN").Length(20);
            this.Property(x => x.Kpp, "КПП").Column("KPP").Length(20);
            this.Property(x => x.Bik, "БИК").Column("BIK").Length(20);
            this.Property(x => x.Okpo, "ОКПО").Column("OKPO").Length(20);
            this.Property(x => x.CorrAccount, "Корреспондентский счет").Column("CORR_ACCOUNT").Length(50);
            this.Property(x => x.Ogrn, "ОГРН").Column("OGRN").Length(250);
            this.Property(x => x.MailingAddress, "Почтовый адрес").Column("MAILING_ADDRESS").Length(500);
            this.Property(x => x.Oktmo, "OKTMO").Column("OKTMO");
            this.Property(x => x.GisGkhOrgRootEntityGUID, "Идентификатор корневой сущности организации в реестре организаций ГИС ЖКХ").Column("GIS_GKH_ROOT_GUID");
            this.Reference(x => x.FiasAddress, "Фактический адрес ФИАС").Column("FIAS_ID").Fetch();
            this.Reference(x => x.Parent, "Родительская кредитная организация").Column("PARENT_ID").Fetch();
            this.Reference(x => x.FiasMailingAddress, "Почтовый адрес ФИАС").Column("FIAS_MAIL_ADDRESS_ID").Fetch();
        }
    }

    /// <summary>ReadOnly ExportId</summary>
    public class CreditOrgNhMapping : BaseHaveExportIdMapping<CreditOrg>
    {
    }
}
