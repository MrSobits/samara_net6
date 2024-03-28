namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskPreventiveAction
{
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Вопрос для консультации. Модель выборки
    /// </summary>
    public class QuestionConsultationGet : BaseQuestionConsultation
    {
        /// <summary>
        /// Уникальный идентификатор записи в блоке "Вопросы для консультации"
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// Вопрос для консультации. Модель обновления
    /// </summary>
    public class QuestionConsultationUpdate : BaseQuestionConsultation, INestedEntityId
    {
        /// <inheritdoc />
        [OnlyExistsEntity(typeof(PreventiveActionTaskConsultingQuestion))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Вопрос для консультации. Базовая модель
    /// </summary>
    public class BaseQuestionConsultation
    {
        /// <summary>
        /// Вопрос
        /// </summary>
        [Required]
        public string Question { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        [Required]
        public string Answer { get; set; }

        /// <summary>
        /// Подконтрольное лицо
        /// </summary>
        [Required]
        public string ControlPerson { get; set; }
    }
}