namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Задачи проверки
    /// </summary>
    public class TypeSurveyTaskInspGji : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Тип обследования
        /// </summary>
        public virtual TypeSurveyGji TypeSurvey { get; set; }

        /// <summary>
        /// Задача проверки
        /// </summary>
        public virtual SurveyObjective SurveyObjective { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Obsolete("Заменено на ссылку на справочник SurveyObjective")]
        public virtual string Name { get; set; }
    }
}