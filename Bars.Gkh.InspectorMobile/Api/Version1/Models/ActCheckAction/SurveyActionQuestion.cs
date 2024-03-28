namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheckAction
{
    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;

    /// <summary>
    /// Модель получения Вопроса действия акта проверки с типом "Опрос"
    /// </summary>
    public class SurveyActionQuestionGet : BaseSurveyActionQuestion
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
    }
    
    /// <summary>
    /// Модель создания Вопроса действия акта проверки с типом "Опрос"
    /// </summary>
    public class SurveyActionQuestionCreate : BaseSurveyActionQuestion
    {
    }
    
    /// <summary>
    /// Модель обновления Вопроса действия акта проверки с типом "Опрос"
    /// </summary>
    public class SurveyActionQuestionUpdate : BaseSurveyActionQuestion, INestedEntityId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [OnlyExistsEntity(typeof(SurveyActionQuestion))]
        public long? Id { get; set; }
    }
    
    /// <summary>
    /// Модель Вопроса действия акта проверки с типом "Опрос"
    /// </summary>
    public class BaseSurveyActionQuestion
    {
        /// <summary>
        /// Вопрос
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public string Answer { get; set; }
    }
}