namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.GkhGji.Enums;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Постановление
    /// </summary>
    public class Resolution : DocumentGji
    {
        /// <summary>
        /// тип исполнителя документа
        /// </summary>
        public virtual ExecutantDocGji Executant { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Вид санкции
        /// </summary>
        public virtual SanctionGji Sanction { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual Inspector Official { get; set; }

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
        /// Должность
        /// </summary>
        public virtual string Position { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

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
        /// Дата вручения
        /// </summary>
        public virtual DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Тип инициативного органа
        /// </summary>
        public virtual TypeInitiativeOrgGji TypeInitiativeOrg { get; set; }

        /// <summary>
        /// Номер участка
        /// </summary>
        public virtual string SectorNumber { get; set; }

        /// <summary>
        /// Сумма штрафов
        /// </summary>
        public virtual decimal? PenaltyAmount { get; set; }

        /// <summary>
        /// Штраф оплачен
        /// </summary>
        public virtual YesNoNotSet Paided { get; set; }

        /// <summary>
        /// Штраф оплачен (подробно)
        /// </summary>
        public virtual ResolutionPaymentStatus PayStatus { get; set; }

        /// <summary>
        /// Дата передачи в ССП
        /// </summary>
        public virtual DateTime? DateTransferSsp { get; set; }

        /// <summary>
        /// Номер документа, передача в ССП
        /// </summary>
        public virtual string DocumentNumSsp { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Время составления документа
        /// </summary>
        public virtual DateTime? DocumentTime { get; set; }

        /// <summary>
        /// Выписка из ЕГРЮЛ
        /// </summary>
        public virtual DateTime? DateWriteOut { get; set; }

        /// <summary>
        /// Вступило в законную силу
        /// </summary>
        public virtual bool BecameLegal { get; set; }

        /// <summary>
        /// МО получателя штрафа
        /// </summary>
        public virtual Municipality FineMunicipality { get; set; }

        //ToDo ГЖИ после перехода направила, выпилить поля которые нехрранимые 

        /// <summary>
        /// Список родительских документов (Используется при создании объекта)
        /// </summary>
        public virtual List<long> ParentDocumentsList { get; set; }

        /// <summary>
        /// Основание прекращения
        /// </summary>
        public virtual TypeTerminationBasement? TypeTerminationBasement { get; set; }

        /// <summary>
        /// Основание аннулирования
        /// </summary>
        //Используется в интеграции с ГИС ГМП (пока только татарстан)
        public virtual string AbandonReason { get; set; }

        /// <summary>
        /// Нарушитель явился на рассмотрение
        /// </summary>
        public virtual YesNoNotSet OffenderWas { get; set; }

        /// <summary>
        /// Номер постановления
        /// </summary>
        public virtual long? RulingNumber { get; set; }

        /// <summary>
        /// Фио в постановлении
        /// </summary>
        public virtual string RulinFio { get; set; }

        /// <summary>
        /// Дата постановления
        /// </summary>
        public virtual DateTime? RulingDate { get; set; }

        /// <summary>
        /// Почтовый идентификатор
        /// </summary>
        public virtual string PostGUID { get; set; }

        /// <summary>
        /// Отдел судебных приставов
        /// </summary>
        public virtual JurInstitution OSP { get; set; }

        /// <summary>
        /// Дата вручения исполнительного документа
        /// </summary>
        public virtual DateTime? DateOSPListArrive { get; set; }

        /// <summary>
        /// Дата исполнительного производства
        /// </summary>
        public virtual DateTime? DateExecuteSSP { get; set; }

        /// <summary>
        /// Дата окончания исполнительного производства
        /// </summary>
        public virtual DateTime? DateEndExecuteSSP { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Номер исполнительного дела
        /// </summary>
        public virtual string ExecuteSSPNumber { get; set; }

        /// <summary>
        /// Решение ОСП
        /// </summary>
        public virtual OSPDecisionType OSPDecisionType { get; set; }

        //решения судебного участка

        /// <summary>
        /// Номер решения
        /// </summary>
        public virtual string DecisionNumber { get; set; }

        /// <summary>
        /// Дата решения
        /// </summary>
        public virtual DateTime? DecisionDate { get; set; }

        /// <summary>
        /// Дата вступления в законную силу
        /// </summary>
        public virtual DateTime? DecisionEntryDate { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual string Violation { get; set; }

        /// <summary>
        /// Судебный участок
        /// </summary>
        public virtual JurInstitution JudicalOffice { get; set; }

        /// <summary>
        /// Дата вступления в законную силу
        /// </summary>
        public virtual DateTime? InLawDate { get; set; }

        /// <summary>
        /// Оплатить до
        /// </summary>
        public virtual DateTime? DueDate { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Результат рассмотрения
        /// </summary>
        public virtual ConcederationResult ConcederationResult { get; set; }

        /// <summary>
        /// Оплачено со скидкой
        /// </summary>
        public virtual bool Payded50Percent { get; set; }

        /// <summary>
        /// Оплачено со скидкой
        /// </summary>
        public virtual bool WrittenOff { get; set; }

        /// <summary>
        /// Оплачено со скидкой
        /// </summary>
        public virtual string WrittenOffComment { get; set; }

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