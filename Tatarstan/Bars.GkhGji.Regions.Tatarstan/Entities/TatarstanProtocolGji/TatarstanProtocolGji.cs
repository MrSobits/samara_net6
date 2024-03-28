namespace Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Протокол ГЖИ РТ
    /// </summary>
    public class TatarstanProtocolGji: DocumentGji
    {
        /// <summary>
        /// Муниципальное образование 
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Орган, оформивший протокол
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        public virtual TypeDocObject Executant { get; set; }

        /// <summary>
        /// Дата поступления в ГЖИ
        /// </summary>
        public virtual DateTime? DateSupply { get; set; }

        /// <summary>
        /// Дата правонарушения
        /// </summary>
        public virtual DateTime? DateOffense { get; set; }

        /// <summary>
        /// Время правонарушения
        /// </summary>
        public virtual DateTime? TimeOffense { get; set; }

        /// <summary>
        /// Шаблон ГИС ГМП
        /// </summary>
        public virtual GisGmpPatternDict Pattern { get; set; }

        /// <summary>
        /// Причина аннулирования
        /// </summary>
        public virtual string AnnulReason { get; set; }

        /// <summary>
        /// Причина изменения
        /// </summary>
        public virtual string UpdateReason { get; set; }

        /// <summary>
        /// Вид санкции
        /// </summary>
        public virtual SanctionGji Sanction { get; set; }

        /// <summary>
        /// Штраф оплачен
        /// </summary>
        public virtual YesNoNotSet Paided { get; set; }

        /// <summary>
        /// Сумма штрафа
        /// </summary>
        public virtual decimal? PenaltyAmount { get; set; }

        /// <summary>
        /// Дата передачи в ССП
        /// </summary>
        public virtual DateTime? DateTransferSsp { get; set; }

        /// <summary>
        /// Основание прекращения
        /// </summary>
        public virtual TypeTerminationBasement? TerminationBasement { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string TerminationDocumentNum { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string SurName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// Фактический адрес проживания
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Семейное положение
        /// </summary>
        public virtual string MaritalStatus { get; set; }

        /// <summary>
        /// Количество иждивенцев
        /// </summary>
        public virtual int? DependentCount { get; set; }

        /// <summary>
        /// Тип гражданства
        /// </summary>
        public virtual CitizenshipType? CitizenshipType { get; set; }

        /// <summary>
        /// Код страны
        /// </summary>
        public virtual Citizenship Citizenship { get; set; }

        /// <summary>
        /// Тип документа, удостоверяющего личность
        /// </summary>
        public virtual IdentityDocumentType IdentityDocumentType { get; set; }

        /// <summary>
        /// Серия и номер документа
        /// </summary>
        public virtual string SerialAndNumberDocument { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        public virtual DateTime? IssueDate { get; set; }

        /// <summary>
        /// Кем выдан
        /// </summary>
        public virtual string IssuingAuthority { get; set; }

        /// <summary>
        /// Место работы, должность, адрес
        /// </summary>
        public virtual string Company { get; set; }

        /// <summary>
        /// Адрес регистрации
        /// </summary>
        public virtual string RegistrationAddress { get; set; }

        /// <summary>
        /// Размер зарплаты (пенсии, стипендии) в руб.
        /// </summary>
        public virtual decimal? Salary { get; set; }

        /// <summary>
        /// Привлекался ли ранее к административной ответственности по ч. 1 ст. 20.6 КоАП РФ
        /// </summary>
        public virtual bool ResponsibilityPunishment { get; set; }
        
        /// <summary>
        /// Объяснение и замечания по содержанию протокола
        /// </summary>
        public virtual string ProtocolExplanation { get; set; }

        /// <summary>
        /// Рассмотрения дела состоится в суде
        /// </summary>
        public virtual bool IsInTribunal { get; set; }

        /// <summary>
        /// Наименование суда
        /// </summary>
        public virtual string TribunalName { get; set; }

        /// <summary>
        /// Адрес правонарушения
        /// </summary>
        public virtual string OffenseAddress { get; set; }

        /// <summary>
        /// Объяснение лица, в отношении которого возбуждено дело
        /// </summary>
        public virtual string AccusedExplanation { get; set; }

        /// <summary>
        /// Отказ от подписания протокола
        /// </summary>
        public virtual bool RejectionSignature { get; set; }

        /// <summary>
        /// Ходатайство по месту жительства
        /// </summary>
        public virtual bool ResidencePetition { get; set; }

        /// <summary>
        /// Не хранимое поле InspectionId потомучто поле Inspection JSONIgnore и чтобы работать на клиенте нужен id инспекции
        /// Адрес регистрации
        /// </summary>
        public virtual long InspectionId { get; set; }
    }
}
