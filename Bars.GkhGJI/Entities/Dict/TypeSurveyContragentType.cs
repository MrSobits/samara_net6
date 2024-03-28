namespace Bars.GkhGji.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Справочники - ГЖИ - Типы обследования.
    /// Вкладка Типы контрагента
    /// </summary>
    public class TypeSurveyContragentType : BaseEntity
    {
		/// <summary>
		/// Тип обследования
		/// </summary>
        public virtual TypeSurveyGji TypeSurveyGji { get; set; }

		/// <summary>
		/// Тип юридического лица
		/// </summary>
        public virtual TypeJurPerson TypeJurPerson { get; set; }
    }
}