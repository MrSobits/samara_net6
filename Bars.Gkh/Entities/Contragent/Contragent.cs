namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities.Base;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Контрагент
    /// </summary>
    public class Contragent : BaseGkhEntity, IHaveExportId, IUsedInTorIntegration, IEntityUsedInErp
    {
        private string shortName;

        /// <summary>
        /// Идентификатор для экспорта
        /// </summary>
        public virtual long ExportId { get; set; }

        /// <summary>
        /// Родительский контрагент
        /// </summary>
        public virtual Contragent Parent { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality MoSettlement { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        public virtual string ShortName 
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.shortName) ? this.Name : this.shortName;
            }

            set
            {
                this.shortName = value;
            }
        }

        /// <summary>
        /// Фактический адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasFactAddress { get; set; }

        /// <summary>
        /// Юридический адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasJuridicalAddress { get; set; }

        /// <summary>
        /// Почтовый адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasMailingAddress { get; set; }

        /// <summary>
        /// адрес за пределами субъекта ФИАС
        /// </summary>
        public virtual FiasAddress FiasOutsideSubjectAddress { get; set; }

        /// <summary>
        /// Адрес за пределами субъекта
        /// </summary>
        public virtual string AddressOutsideSubject { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public virtual string FactAddress { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        public virtual string JuridicalAddress { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public virtual string MailingAddress { get; set; }

        /// <summary>
        /// Дата прекращения деятельности
        /// </summary>
        public virtual DateTime? DateTermination { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Электронный адрес
        /// </summary>
        public virtual string Email { get; set; }
        
        /// <summary>
        /// Получать оповещения на E-mail
        /// </summary>
        public virtual YesNo ReceiveNotifications { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public virtual string Kpp { get; set; }

        /// <summary>
        /// Официальный сайт Да/Нет
        /// </summary>
        public virtual bool IsSite { get; set; }

        /// <summary>
        /// Официальный сайт
        /// </summary>
        public virtual string OfficialWebsite { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual string Ogrn { get; set; }

        /// <summary>
        /// ОГРН, принявший решение о регистрации
        /// </summary>
        public virtual string OgrnRegistration { get; set; }

        /// <summary>
        /// Номер последней выписки из ЕГРЮЛ
        /// </summary>
        public virtual string EgrulExcNumber { get; set; }

        /// <summary>
        /// Дата последней выписки из ЕГРЮЛ
        /// </summary>
        public virtual DateTime? EgrulExcDate { get; set; }

        /// <summary>
        /// Организационно-правовая форма
        /// </summary>
        public virtual OrganizationForm OrganizationForm { get; set; }

        /// <summary>
        /// Основная роль
        /// </summary>
        public virtual ContragentRole MainRole { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// Факс
        /// </summary>
        public virtual string Fax { get; set; }

        /// <summary>
        /// Телефон дисп. службы
        /// </summary>
        public virtual string PhoneDispatchService { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual ContragentState ContragentState { get; set; }

        /// <summary>
        /// Абонентский ящик
        /// </summary>
        public virtual string SubscriberBox { get; set; }

        /// <summary>
        /// Твитер акаунт
        /// </summary>
        public virtual string TweeterAccount { get; set; }

        /// <summary>
        /// Реестровый номер функции в ФРГУ
        /// </summary>
        public virtual string FrguRegNumber { get; set; }

        /// <summary>
        /// Номер организации в ФРГУ
        /// </summary>
        public virtual string FrguOrgNumber { get; set; }

        /// <summary>
        /// Номер услуги в ФРГУ
        /// </summary>
        public virtual string FrguServiceNumber { get; set; }

        /// <summary>
        /// Год регистрации
        /// </summary>
        public virtual int? YearRegistration { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public virtual DateTime? DateRegistration { get; set; }

        // Деятельность

        /// <summary>
        /// Дата начала деятельности
        /// </summary>
        public virtual DateTime? ActivityDateStart { get; set; }

        /// <summary>
        /// Дата окончания деятельности
        /// </summary>
        public virtual DateTime? ActivityDateEnd { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string ActivityDescription { get; set; }

        /// <summary>
        /// Основание прекращения деятельности
        /// </summary>
        public virtual GroundsTermination ActivityGroundsTermination { get; set; }

        /// <summary>
        /// ОКПО
        /// </summary>
        public virtual int Okpo { get; set; }

        /// <summary>
        /// ОКВЭД
        /// </summary>
        public virtual string Okved { get; set; }

        /// <summary>
        /// Тип предпринимательства
        /// </summary>
        public virtual TypeEntrepreneurship TypeEntrepreneurship { get; set; }

        /// <summary>
        /// Наименование  Родительный падеж
        /// </summary>
        public virtual string NameGenitive { get; set; }

        /// <summary>
        /// Наименование Дательный падеж
        /// </summary>
        public virtual string NameDative { get; set; }

        /// <summary>
        /// Наименование Винительный падеж
        /// </summary>
        public virtual string NameAccusative { get; set; }

        /// <summary>
        /// Наименование Творительный падеж
        /// </summary>
        public virtual string NameAblative { get; set; }

        /// <summary>
        /// Наименование Предложный падеж
        /// </summary>
        public virtual string NamePrepositional { get; set; }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public virtual long? Okato { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public virtual long? Oktmo { get; set; }

        /// <summary>
        /// Серия
        /// </summary>
        public virtual string TaxRegistrationSeries { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string TaxRegistrationNumber { get; set; }
        
        /// <summary>
        /// Кем выдан
        /// </summary>
        public virtual string TaxRegistrationIssuedBy { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? TaxRegistrationDate { get; set; }

        /// <summary>
        /// Дата постановки на учёт в реестре домов социального использования
        /// </summary>
        public virtual DateTime? RegDateInSocialUse { get; set; }

        /// <summary>
        /// Дата получения лицензии
        /// </summary>
        public virtual DateTime? LicenseDateReceipt { get; set; }

        /// <summary>
        /// Код поставщика. Генерируется через <see cref="CrcGenerator"/>
        /// </summary>
        public virtual string ProviderCode { get; set; }

        /// <summary>
        /// Часовой пояс
        /// </summary>
        public virtual TimeZoneType? TimeZoneType { get; set; }

        /// <summary>
        /// ОКОГУ
        /// </summary>
        public virtual string Okogu { get; set; }

        /// <summary>
        /// ОКФС
        /// </summary>
        public virtual string Okfs { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGUID { get; set; }

        /// <summary>
        /// ГИС ЖКХ orgVersionGUID
        /// </summary>
        public virtual string GisGkhVersionGUID { get; set; }

        /// <summary>
        /// ГИС ЖКХ orgPPAGUID
        /// </summary>
        public virtual string GisGkhOrgPPAGUID { get; set; }

        /// <summary>
        /// Обмен данными в электронном виде
        /// </summary>
        public virtual bool IsEDSE { get; set; }

        /// <summary>
        /// Обмен данными в электронном виде
        /// </summary>
        public virtual bool IsSOPR { get; set; }
        /// Уникальный идентификатор ТОР.
        /// </summary>
        public virtual Guid? TorId { get; set; }

        /// <summary>
        /// Гуид ЕРП
        /// </summary>
        public virtual string ErpGuid { get; set; }

        /// <summary>
        /// Включен в СОПР
        /// </summary>
        public virtual bool IncludeInSopr { get; set; }
    }
}