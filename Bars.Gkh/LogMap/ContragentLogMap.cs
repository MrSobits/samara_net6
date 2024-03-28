namespace Bars.Gkh.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Регистрация изменений полей сущности <see cref="Contragent"/>
    /// </summary>
    public class ContragentLogMap : AuditLogMap<Contragent>
    {
        public ContragentLogMap()
        {
            this.Name("Контрагенты");

            this.Description(x => x.Name);

            // bool
            this.MapProperty(x => x.IsSite, "IsSite", "Официальный сайт Да/Нет", x => x ? "Да" : "Нет");

            // Contragent
            this.MapProperty(x => x.Parent, "Parent", "Родительский контрагент", x => x?.Name);

            // ContragentState
            this.MapProperty(x => x.ContragentState, "ContragentState", "Статус", 
                x => x != default(ContragentState) ? x.GetDisplayName() : string.Empty);

            // DateTime?
            this.MapProperty(x => x.DateTermination, "DateTermination", "Дата прекращения деятельности");
            this.MapProperty(x => x.EgrulExcDate, "EgrulExcDate", "Дата выписки из ЕГРЮЛ");
            this.MapProperty(x => x.DateRegistration, "DateRegistration", "Дата регистрации");
            this.MapProperty(x => x.ActivityDateStart, "ActivityDateStart", "Дата начала деятельности");
            this.MapProperty(x => x.ActivityDateEnd, "ActivityDateEnd", "Дата окончания деятельности");
            this.MapProperty(x => x.TaxRegistrationDate, "TaxRegistrationDate", "Дата выдачи");
            this.MapProperty(x => x.RegDateInSocialUse, "RegDateInSocialUse", "Дата постановки на учёт в реестре домов социального использования");
            this.MapProperty(x => x.LicenseDateReceipt, "LicenseDateReceipt", "Дата получения лицензии");

            // FiasAddress
            this.MapProperty(x => x.FiasFactAddress, "FiasFactAddress", "Фактический адрес ФИАС", x => x?.AddressName);
            this.MapProperty(x => x.FiasJuridicalAddress, "FiasJuridicalAddress", "Юридический адрес ФИАС", x => x?.AddressName);
            this.MapProperty(x => x.FiasMailingAddress, "FiasMailingAddress", "Почтовый адрес ФИАС", x => x?.AddressName);
            this.MapProperty(x => x.FiasOutsideSubjectAddress, "FiasOutsideSubjectAddress", "адрес за пределами субъекта ФИАС", x => x?.AddressName);

            // GroundsTermination
            this.MapProperty(x => x.ActivityGroundsTermination, "ActivityGroundsTermination", "Основание прекращения деятельности", 
                x => x != default(GroundsTermination) ? x.GetDisplayName() : string.Empty);

            // int
            this.MapProperty(x => x.Okpo, "Okpo", "ОКПО");

            // int?
            this.MapProperty(x => x.YearRegistration, "YearRegistration", "Год регистрации");

            // long?
            this.MapProperty(x => x.Okato, "Okato", "ОКАТО");
            this.MapProperty(x => x.Oktmo, "Oktmo", "ОКТМО");

            // Municipality
            this.MapProperty(x => x.Municipality, "Municipality", "Муниципальный район", x => x?.Name);
            this.MapProperty(x => x.MoSettlement, "MoSettlement", "Муниципальное образование", x => x?.Name);

            // OrganizationForm
            this.MapProperty(x => x.OrganizationForm, "OrganizationForm", "Организационно-правовая форма", x => x?.Name);

            // string
            this.MapProperty(x => x.Name, "Name", "Наименование");
            this.MapProperty(x => x.ShortName, "ShortName", "Краткое наименование");
            this.MapProperty(x => x.AddressOutsideSubject, "AddressOutsideSubject", "Адрес за пределами субъекта");
            this.MapProperty(x => x.FactAddress, "FactAddress", "Фактический адрес");
            this.MapProperty(x => x.JuridicalAddress, "JuridicalAddress", "Юридический адрес");
            this.MapProperty(x => x.MailingAddress, "MailingAddress", "Почтовый адрес");
            this.MapProperty(x => x.Description, "Description", "Описание");
            this.MapProperty(x => x.Email, "Email", "Электронный адрес");
            this.MapProperty(x => x.Inn, "Inn", "ИНН");
            this.MapProperty(x => x.Kpp, "Kpp", "КПП");
            this.MapProperty(x => x.OfficialWebsite, "OfficialWebsite", "Официальный сайт");
            this.MapProperty(x => x.Ogrn, "Ogrn", "ОГРН");
            this.MapProperty(x => x.OgrnRegistration, "OgrnRegistration", "ОГРН, принявший решение о регистрации");
            this.MapProperty(x => x.EgrulExcNumber, "EgrulExcNumber", "Номер выписки из ЕГРЮЛ");
            this.MapProperty(x => x.Phone, "Phone", "Телефон");
            this.MapProperty(x => x.Fax, "Fax", "Факс");
            this.MapProperty(x => x.PhoneDispatchService, "PhoneDispatchService", "Телефон дисп");
            this.MapProperty(x => x.SubscriberBox, "SubscriberBox", "Абонентский ящик");
            this.MapProperty(x => x.TweeterAccount, "TweeterAccount", "Твитер акаунт");
            this.MapProperty(x => x.FrguRegNumber, "FrguRegNumber", "Реестровый номер функции в ФРГУ");
            this.MapProperty(x => x.FrguOrgNumber, "FrguOrgNumber", "Номер организации в ФРГУ");
            this.MapProperty(x => x.FrguServiceNumber, "FrguServiceNumber", "Номер услуги в ФРГУ");
            this.MapProperty(x => x.ActivityDescription, "ActivityDescription", "Описание");
            this.MapProperty(x => x.Okved, "Okved", "ОКВЭД");
            this.MapProperty(x => x.NameGenitive, "NameGenitive", "Наименование  Родительный падеж");
            this.MapProperty(x => x.NameDative, "NameDative", "Наименование Дательный падеж");
            this.MapProperty(x => x.NameAccusative, "NameAccusative", "Наименование Винительный падеж");
            this.MapProperty(x => x.NameAblative, "NameAblative", "Наименование Творительный падеж");
            this.MapProperty(x => x.NamePrepositional, "NamePrepositional", "Наименование Предложный падеж");
            this.MapProperty(x => x.TaxRegistrationSeries, "TaxRegistrationSeries", "Серия");
            this.MapProperty(x => x.TaxRegistrationNumber, "TaxRegistrationNumber", "Номер");
            this.MapProperty(x => x.TaxRegistrationIssuedBy, "TaxRegistrationIssuedBy", "Кем выдан");
            this.MapProperty(x => x.ProviderCode, "ProviderCode", "Код поставщика");

            // TypeEntrepreneurship
            this.MapProperty(x => x.TypeEntrepreneurship, "TypeEntrepreneurship", "Тип предпринимательства", 
                x => x != default(TypeEntrepreneurship) ? x.GetDisplayName() : string.Empty);
        }
    }
}
