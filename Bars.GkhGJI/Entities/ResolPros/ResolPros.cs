namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.GkhGji.Enums;
    using Gkh.Entities;

    /// <summary>
    /// Постановление прокуратуры
    /// </summary>
    public class ResolPros : DocumentGji
    {
        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        public virtual ExecutantDocGji Executant { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPersonPosition { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Муниципальное образование (Орган прокуратуры, вынесший постановление)
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Дата поступления в ГЖИ
        /// </summary>
        public virtual DateTime? DateSupply { get; set; }

        /// <summary>
        /// Не хранимое поле Акт проверки (подтягивается в методе Get)
        /// </summary>
        public virtual DocumentGji ActCheck { get; set; }

        /// <summary>
        /// Не хранимое поле InspectionId потомучто поле Inspection JSONIgnore ичтобы работат ьна клиенте нужен id инспекции
        /// </summary>
        public virtual long InspectionId { get; set; }

        /// <summary>
        /// Не хранимое поле, говорит о том можно или нет формировать Постановления из карточки Постановления прокуратуры
        /// в методе ResolProsGJIController/Get Идет логика получения правав можно или нет формировать постановление
        /// </summary>
        public virtual bool BlockResolution { get; set; }

        /// <summary>
        /// УИН
        /// </summary>
        public virtual string UIN { get; set; }

        /// <summary>
        /// Не хранимое поле Акт проверки (подтягивается в методе Get)
        /// </summary>
        public virtual ProsecutorOffice ProsecutorOffice { get; set; }

        /// <summary>
        /// должность прокурора
        /// </summary>
        public virtual string IssuedByPosition { get; set; }

        /// <summary>
        /// звание прокурора
        /// </summary>
        public virtual string IssuedByRank { get; set; }

        /// <summary>
        /// фио прокурора
        /// </summary>
        public virtual string IssuedByFio { get; set; }

        /// <summary>
        /// Часы рассмотрения постановление прокуратуры
        /// </summary>
        public virtual int? FormatHour { get; set; }

        /// <summary>
        /// Минуты рассмотрения постановление прокуратуры
        /// </summary>
        public virtual int? FormatMinute { get; set; }

        /// <summary>
        /// Дата рассмотрения постановления прокуратуры
        /// </summary>
        public virtual DateTime? DateResolPros { get; set; }

        /// <summary>
        /// Тип документа физ лица
        /// </summary>
        public virtual PhysicalPersonDocType PhysicalPersonDocType { get; set; }

        /// <summary>
        /// Номер документа физлица
        /// </summary>
        public virtual string PhysicalPersonDocumentNumber { get; set; }

        /// <summary>
        /// Серия документа физлица
        /// </summary>
        public virtual string PhysicalPersonDocumentSerial { get; set; }

        /// <summary>
        /// Не является гражданином РФ
        /// </summary>
        public virtual bool PhysicalPersonIsNotRF { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// Адрес регистрации (место жительства, телефон)
        /// </summary>
        public virtual string PersonRegistrationAddress { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public virtual string PersonFactAddress { get; set; }

        /// <summary>
        /// Протокол - Реквизиты - В присуствии/отсутствии
        /// </summary>
        public virtual TypeRepresentativePresence TypePresence { get; set; }

        /// <summary>
        /// Представитель
        /// </summary>
        public virtual string Representative { get; set; }

        /// <summary>
        /// Вид и реквизиты основания
        /// </summary>
        public virtual string ReasonTypeRequisites { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime? PersonBirthDate { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public virtual string PersonBirthPlace { get; set; }
    }
}