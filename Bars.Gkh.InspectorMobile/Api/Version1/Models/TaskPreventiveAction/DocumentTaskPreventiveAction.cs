namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskPreventiveAction
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Документ "Задание профилактического мероприятия". Модель выборки
    /// </summary>
    public class DocumentTaskPreventiveActionGet : BaseDocumentTaskPreventiveAction<QuestionConsultationGet>
    {
        /// <summary>
        /// Уникальный идентификатор документа
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Данные по проверке
        /// </summary>
        public InspectionInfo Inspection { get; set; }

        /// <summary>
        /// Номер документа "Задание"
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// У документа "Задание" есть дочерние документы в НЕ конечном статусе
        /// </summary>
        public bool RelatedDocuments { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата начала мероприятия
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания мероприятия
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Время мероприятия
        /// </summary>
        public DateTime? Time { get; set; }

        /// <summary>
        /// Адрес дома - места проведения мероприятия
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Идентификатор Инспектора
        /// </summary>
        public long? InspectorId { get; set; }

        /// <summary>
        /// Вид мероприятия
        /// </summary>
        public PreventiveActionType TypeCheckId { get; set; }

        /// <summary>
        /// Тип визита
        /// </summary>
        public PreventiveActionVisitType? TypeVisit { get; set; }

        /// <summary>
        /// Тип подконтрольного лица
        /// </summary>
        public string ControlledPersonType { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string Individual { get; set; }

        /// <summary>
        /// Адрес контролируемого лица
        /// </summary>
        public string PersonAddress { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Запланированные действия
        /// </summary>
        public IEnumerable<PlannedActivity> PlannedActivities { get; set; }
    }

    public class DocumentTaskPreventiveActionUpdate: BaseDocumentTaskPreventiveAction<QuestionConsultationUpdate>
    {
    }

    /// <summary>
    /// Документ "Задание профилактического мероприятия". Базовая модель
    /// </summary>
    /// <typeparam name="TQuestionsConsultation">Тип модели "Вопрос для консультации"</typeparam>
    public class BaseDocumentTaskPreventiveAction<TQuestionsConsultation>
    {
        /// <summary>
        /// Вопросы для консультации
        /// </summary>
        public IEnumerable<TQuestionsConsultation> QuestionsConsultation { get; set; }
    }
}