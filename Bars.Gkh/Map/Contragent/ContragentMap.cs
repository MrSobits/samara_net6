namespace Bars.Gkh.Map.Contragent
{
    using Bars.Gkh.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг для "Контрагент"
    /// </summary>
    public class ContragentMap : BaseImportableEntityMap<Contragent>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ContragentMap() : base("Контрагент", "GKH_CONTRAGENT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExportId, "ExportId").Column("EXPORT_ID").NotNull();
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.ContragentState, "Статус").Column("CONTRAGENT_STATE").NotNull();
            this.Property(x => x.AddressOutsideSubject, "Адрес за пределами субъекта").Column("ADDRESS_OUT_SUBJECT").Length(500);
            this.Property(x => x.FactAddress, "Фактический адрес").Column("FACT_ADDRESS").Length(500);
            this.Property(x => x.JuridicalAddress, "Юридический адрес").Column("JURIDICAL_ADDRESS").Length(500);
            this.Property(x => x.MailingAddress, "Почтовый адрес").Column("MAILING_ADDRESS").Length(500);
            this.Property(x => x.DateTermination, "Дата прекращения деятельности").Column("DATE_TERMINATION");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.Email, "Электронный адрес").Column("EMAIL").Length(200);
            this.Property(x => x.Inn, "ИНН").Column("INN").Length(20);
            this.Property(x => x.Kpp, "КПП").Column("KPP").Length(20);
            this.Property(x => x.IsSite, "Официальный сайт Да/Нет").Column("IS_SITE").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            this.Property(x => x.ShortName, "ShortName").Column("SHORT_NAME").Length(300);
            this.Property(x => x.OfficialWebsite, "Официальный сайт").Column("OFFICIAL_WEBSITE").Length(250);
            this.Property(x => x.Ogrn, "ОГРН").Column("OGRN").Length(250);
            this.Property(x => x.OgrnRegistration, "ОГРН, принявший решение о регистрации").Column("OGRN_REG").Length(300);
            this.Property(x => x.EgrulExcNumber, "Номер выписки из ЕГРЮЛ").Column("EGRUL_EXC_NUMBER").Length(100);
            this.Property(x => x.EgrulExcDate, "Дата выписки из ЕГРЮЛ").Column("EGRUL_EXC_DATE");
            this.Property(x => x.Phone, "Телефон").Column("PHONE").Length(2000);
            this.Property(x => x.PhoneDispatchService, "Телефон дисп. службы").Column("PHONE_DISPATCH_SERVICE").Length(100);
            this.Property(x => x.SubscriberBox, "Абонентский ящик").Column("SUBSCRIBER_BOX").Length(300);
            this.Property(x => x.TweeterAccount, "Твитер акаунт").Column("TWEETER_ACCOUNT").Length(300);
            this.Property(x => x.FrguRegNumber, "Реестровый номер функции в ФРГУ").Column("FRGU_REG_NUMBER").Length(36);
            this.Property(x => x.FrguOrgNumber, "Номер организации в ФРГУ").Column("FRGU_ORG_NUMBER").Length(36);
            this.Property(x => x.FrguServiceNumber, "Номер услуги в ФРГУ").Column("FRGU_SERVICE_NUMBER").Length(36);
            this.Property(x => x.YearRegistration, "Год регистрации").Column("YEAR_REG");
            this.Property(x => x.DateRegistration, "Дата регистрации").Column("DATE_REG");
            this.Property(x => x.ActivityDateStart, "Дата начала деятельности").Column("ACTIVITY_DATE_START");
            this.Property(x => x.ActivityDateEnd, "Дата окончания деятельности").Column("ACTIVITY_DATE_END");
            this.Property(x => x.ActivityDescription, "Описание").Column("ACTIVITY_DESCRIPTION");
            this.Property(x => x.TypeEntrepreneurship, "Тип предпринимательства").Column("TYPE_ENTREPRENEUR").NotNull();
            this.Property(x => x.ActivityGroundsTermination, "Основание прекращения деятельности").Column("ACTIVITY_TERMINATION").NotNull();
            this.Property(x => x.Okpo, "ОКПО").Column("OKPO");
            this.Property(x => x.Okved, "ОКВЭД").Column("OKVED").Length(50);
            this.Property(x => x.NameGenitive, "Наименование Родительный падеж").Column("NAME_GENITIVE").Length(300);
            this.Property(x => x.NameDative, "Наименование Дательный падеж").Column("NAME_DATIVE").Length(300);
            this.Property(x => x.NameAccusative, "Наименование Винительный падеж").Column("NAME_ACCUSATIVE").Length(300);
            this.Property(x => x.NameAblative, "Наименование Творительный падеж").Column("NAME_ABLATIVE").Length(300);
            this.Property(x => x.NamePrepositional, "Наименование Предложный падеж").Column("NAME_PREPOSITIONAL").Length(300);
            this.Property(x => x.Okato, "ОКАТО").Column("OKATO");
            this.Property(x => x.Oktmo, "ОКТМО").Column("OKTMO");
            this.Property(x => x.TaxRegistrationSeries, "Серия").Column("TAX_REGISTRATION_SERIES").Length(100);
            this.Property(x => x.TaxRegistrationNumber, "Номер").Column("TAX_REGISTRATION_NUMBER").Length(300);
            this.Property(x => x.TaxRegistrationIssuedBy, "Кем выдан").Column("TAX_REGISTRATION_ISSUED_BY").Length(300);
            this.Property(x => x.TaxRegistrationDate, "Дата выдачи").Column("TAX_REGISTRATION_DATE");
            this.Property(x => x.RegDateInSocialUse, "Дата постановки на учёт в реестре домов социального использования").Column("REG_DATE_SOC_USE");
            this.Property(x => x.LicenseDateReceipt, "Дата получения лицензии").Column("LICENSE_DATE_RECEIPT");
            this.Property(x => x.Fax, "Факс").Column("FAX").Length(100);
            this.Property(x => x.ProviderCode, "Код поставщика").Column("PROVIDER_CODE");
            this.Property(x => x.TimeZoneType, "Часовой пояс").Column("TIMEZONE_TYPE");
            this.Property(x => x.Okogu, "Часовой пояс").Column("OKOGU");
            this.Property(x => x.Okfs, "Часовой пояс").Column("OKFS");
            this.Property(x => x.GisGkhGUID, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID");
            this.Property(x => x.GisGkhVersionGUID, "ГИС ЖКХ orgVersionGUID").Column("GIS_GKH_VERSION_GUID");
            this.Property(x => x.GisGkhOrgPPAGUID, "ГИС ЖКХ orgPPAGUID").Column("GIS_GKH_ORGPPA_GUID");
            this.Property(x => x.IsEDSE, "Обмен данными в электронном виде").Column("IS_EDSE").NotNull();
            this.Property(x => x.IsSOPR, "Обмен данными в электронном виде").Column("IS_SOPR").NotNull();
            //this.Property(x => x.TorId, "Идентификатор в ТОР").Column("TOR_ID");
            this.Property(x => x.ReceiveNotifications, "Получать оповещения на E-mail").Column("RECEIVE_NOTIFICATIONS");
            this.Property(x => x.IncludeInSopr, "Включен в СОПР").Column("INCLUDE_IN_SOPR");

            this.Reference(x => x.Parent, "Родительский контрагент").Column("PARENT_ID").Fetch();
            this.Reference(x => x.OrganizationForm, "Организационно-правовая форма").Column("ORG_LEGAL_FORM_ID").Fetch();
            this.Reference(x => x.Municipality, "Муниципальный район").Column("MUNICIPALITY_ID").Fetch();
            this.Reference(x => x.MoSettlement, "Муниципальное образование").Column("MOSETTLEMENT_ID").Fetch();
            this.Reference(x => x.FiasFactAddress, "Фактический адрес ФИАС").Column("FIAS_FACT_ADDRESS_ID").Fetch();
            this.Reference(x => x.FiasJuridicalAddress, "Юридический адрес ФИАС").Column("FIAS_JUR_ADDRESS_ID").Fetch();
            this.Reference(x => x.FiasMailingAddress, "Почтовый адрес ФИАС").Column("FIAS_MAIL_ADDRESS_ID").Fetch();
            this.Reference(x => x.FiasOutsideSubjectAddress, "адрес за пределами субъекта ФИАС").Column("FIAS_OUT_ADDRESS_ID").Fetch();
            this.Reference(x => x.MainRole, "Основная роль").Column("MAIN_ROLE").Fetch();
        }
    }

    /// <summary>ReadOnly ExportId</summary>
    public class ContragentNhMapping : BaseHaveExportIdMapping<Contragent>
    {
    }
}