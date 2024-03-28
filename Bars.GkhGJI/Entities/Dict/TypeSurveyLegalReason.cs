namespace Bars.GkhGji.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Справочники - ГЖИ - Типы обследований.
    /// Вкладка Правовые основания
    /// </summary>
    public class TypeSurveyLegalReason : BaseEntity
    {
		/// <summary>
		/// Тип обследования
		/// </summary>
        public virtual TypeSurveyGji TypeSurveyGji { get; set; }

		/// <summary>
		/// Правовое основание
		/// </summary>
        public virtual LegalReason LegalReason { get; set; }
    }
}