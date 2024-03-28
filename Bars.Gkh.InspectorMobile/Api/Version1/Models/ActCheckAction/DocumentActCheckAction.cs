namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheckAction
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Модель получения "Протокол действия акта"
    /// </summary>
    public class DocumentActCheckActionGet : BaseDocumentActCheckAction<ActCheckActionViolationGet, ActCheckActionRemarkGet,
        ActCheckActionFileInfoGet, SurveyActionQuestionGet, DocRequestActionRequestInfoGet>
    {
        /// <summary>
        /// Уникальный идентификатор документа
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор родительского документа
        /// </summary>
        public long ParentDocumentId { get; set; }

        /// <summary>
        /// Тип протокола действия
        /// </summary>
        public ActCheckActionType ActionType { get; set; }
    }

    /// <summary>
    /// Модель создания документа "Протокол действия акта"
    /// </summary>
    public class DocumentActCheckActionCreate : BaseDocumentActCheckAction<ActCheckActionViolationCreate, ActCheckActionRemarkCreate,
        ActCheckActionFileInfoCreate, SurveyActionQuestionCreate, DocRequestActionRequestInfoCreate>
    {
        /// <summary>
        /// Уникальный идентификатор родительского документа
        /// </summary>
        [RequiredExistsEntity(typeof(ActCheck))]
        public long? ParentDocumentId { get; set; }

        /// <summary>
        /// Тип протокола действия
        /// </summary>
        [Required]
        public ActCheckActionType? ActionType { get; set; }
    }

    /// <summary>
    /// Модель обновления "Протокол действия акта"
    /// </summary>
    public class DocumentActCheckActionUpdate : BaseDocumentActCheckAction<ActCheckActionViolationUpdate, ActCheckActionRemarkUpdate,
        ActCheckActionFileInfoUpdate, SurveyActionQuestionUpdate, DocRequestActionRequestInfoUpdate>
    {
    }

    /// <summary>
    /// Базовая модель "Протокол действия акта"
    /// </summary>
    public class BaseDocumentActCheckAction<TActCheckActionViolation, TActCheckActionRemark,
        TActCheckActionFile, TSurveyActionQuestion, TDocRequestActionRequestInfo>
    {
        /// <summary>
        /// Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        [OnlyExistsEntity(typeof(Inspector))]
        public IEnumerable<long> InspectorIds { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        [Required]
        public FiasAddress DocumentPlace { get; set; }

        /// <summary>
        /// Период проведения действия с
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Период проведения действия по
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Время проведения действия с
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Время проведения действия по
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Турритория
        /// </summary>
        public string Territory { get; set; }

        /// <summary>
        /// Помещение
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Место проведения действия
        /// </summary>
        public FiasAddress Location { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string NameControlledPerson { get; set; }

        /// <summary>
        /// Место работы
        /// </summary>
        public string WorkControlledPerson { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string PositionControlledPerson { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string FullNameRepresentative { get; set; }

        /// <summary>
        /// Номер доверенности
        /// </summary>
        public string PowerAttorneyNumber { get; set; }

        /// <summary>
        /// Дата доверенности
        /// </summary>
        public DateTime? PowerAttorneyDate { get; set; }

        /// <summary>
        /// Отказ в доступе на территорию
        /// </summary>
        public bool? DenialAccess { get; set; }

        /// <summary>
        /// Нормативно-правовые акты
        /// </summary>
        [OnlyExistsEntity(typeof(NormativeDoc))]
        public IEnumerable<long> Regulations { get; set; }

        /// <summary>
        /// Список действий
        /// </summary>
        public IEnumerable<ActCheckActionCarriedOutEventType> EventType { get; set; }

        /// <summary>
        /// Нарушения
        /// </summary>
        public IEnumerable<TActCheckActionViolation> Violations { get; set; }

        /// <summary>
        /// Замечания
        /// </summary>
        public IEnumerable<TActCheckActionRemark> Remarks { get; set; }

        /// <summary>
        /// Файлы
        /// </summary>
        public IEnumerable<TActCheckActionFile> Files { get; set; }

        /// <summary>
        /// Вопросы
        /// </summary>
        public IEnumerable<TSurveyActionQuestion> Questions { get; set; }

        /// <summary>
        /// Вопросы
        /// </summary>
        public IEnumerable<TDocRequestActionRequestInfo> InformationRequested { get; set; }

        /// <summary>
        /// Адрес предоставления документов
        /// </summary>
        public FiasAddress AddressSubmissionDocuments { get; set; }

        /// <summary>
        /// Срок предоставления (сутки)
        /// </summary>
        public long? Deadline { get; set; }

        /// <summary>
        /// Адрес эл. почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Объяснение
        /// </summary>
        public string Explanation { get; set; }
    }
}