namespace Bars.GkhGji.Entities
{
	using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

	/// <summary>
	/// НПА проверки
	/// </summary>
	public class TypeSurveyInspFoundationCheckGji : BaseEntity
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
    }
}