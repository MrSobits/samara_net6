namespace Bars.GkhGji.Entities.Dict
{
    using B4.DataAccess;
	using Bars.GkhGji.Enums;

    /// <summary>
    /// Справочники - ГЖИ - Предметы проверки
    /// </summary>
    public class SurveySubject : BaseEntity
    {
		/// <summary>
		/// Код
		/// </summary>
        public virtual string Code { get; set; }

		/// <summary>
		/// Наименование
		/// </summary>
        public virtual string Name { get; set; }

		/// <summary>
		/// Формулировка для плана проверок
		/// </summary>
		public virtual string Formulation { get; set; }

		/// <summary>
		/// Актуальность формулировки
		/// </summary>
		public virtual SurveySubjectRelevance Relevance { get; set; }

        /// <summary>
        /// Код ГИС ЖКХ
        /// </summary>
        public virtual string GisGkhCode { get; set; }

        /// <summary>
        /// ГИС ЖКХ Guid
        /// </summary>
        public virtual string GisGkhGuid { get; set; }
    }
}