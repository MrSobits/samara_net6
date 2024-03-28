namespace Bars.GkhGji.Entities.Dict
{
	using Bars.B4.DataAccess;

	/// <summary>
    /// Перечень требований к субъектам проверки
    /// </summary>
    public class SurveySubjectRequirement : BaseEntity
    {
        /// <summary>
        /// Код.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public virtual string Name { get; set; }
    }
}