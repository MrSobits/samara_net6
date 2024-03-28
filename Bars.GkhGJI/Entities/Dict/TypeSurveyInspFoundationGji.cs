namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Правовое основание проведения проверки
    /// </summary>
    public class TypeSurveyInspFoundationGji : BaseEntity
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
        /// Нормативный документ
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [Obsolete("Заменено на ссылку на справочник NormativeDoc")]
        public virtual string Name { get; set; }
    }
}