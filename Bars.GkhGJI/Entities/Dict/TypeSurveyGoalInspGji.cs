namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Цели проверки
    /// </summary>
    public class TypeSurveyGoalInspGji : BaseEntity
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
        /// Цели проверки
        /// </summary>
        public virtual SurveyPurpose SurveyPurpose { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Obsolete("Заменено на ссылку на справочник SurveyPurpose")]
        public virtual string Name { get; set; }
    }
}