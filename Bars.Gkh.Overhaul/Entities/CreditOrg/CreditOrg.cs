namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    public class CreditOrg : BaseImportableEntity, IHaveExportId
    {
        /// <summary>
        /// Идентификатор для экспорта
        /// </summary>
        public virtual long ExportId { get; set; }

        /// <summary>
        /// Родительская кредитная организация
        /// </summary>
        public virtual CreditOrg Parent { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Наименование для печати в квитанциях
        /// </summary>
        public virtual string PrintName { get; set; }

        /// <summary>
        /// Является филиалом
        /// </summary>
        public virtual bool IsFilial { get; set; }

        /// <summary>
        /// Фактический адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasAddress { get; set; }

        /// <summary>
        /// Строковый адрес фиас (Юридический адрес)
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Адрес за пределами субъекта (Юридический адрес)
        /// </summary>
        public virtual string AddressOutSubject { get; set; }

        /// <summary>
        /// Адрес за пределами субъекта (Юридический адрес)
        /// </summary>
        public virtual bool IsAddressOut { get; set; }

        /// <summary>
        /// Почтовый адрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasMailingAddress { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public virtual string MailingAddress { get; set; }

        /// <summary>
        /// Адрес за пределами субъекта (Почтовый адрес)
        /// </summary>
        public virtual string MailingAddressOutSubject { get; set; }

        /// <summary>
        /// Адрес за пределами субъекта (Почтовый адрес)
        /// </summary>
        public virtual bool IsMailingAddressOut { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public virtual string Kpp { get; set; }

        /// <summary>
        /// БИК
        /// </summary>
        public virtual string Bik { get; set; }

        /// <summary>
        /// ОКПО
        /// </summary>
        public virtual string Okpo { get; set; }

        /// <summary>
        /// Корреспондентский счет
        /// </summary>
        public virtual string CorrAccount { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual string Ogrn { get; set; }

        /// <summary>
        /// OKTMO
        /// </summary>
        public virtual string Oktmo { get; set; }

        /// <summary>
        /// Идентификатор корневой сущности организации в реестре организаций ГИС ЖКХ
        /// </summary>
        public virtual string GisGkhOrgRootEntityGUID { get; set; }
    }
}
