namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Вопрос действия акта проверки с типом "Опрос"
    /// </summary>
    public class SurveyActionQuestion : BaseEntity
    {
        /// <summary>
        /// Действие "Опрос"
        /// </summary>
        public virtual SurveyAction SurveyAction { get; set; }

        /// <summary>
        /// Вопрос
        /// </summary>
        public virtual string Question { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }
    }
}