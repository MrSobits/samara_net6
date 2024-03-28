namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
	/// Актуальность формулировки предмета проверки
    /// </summary>
	public enum SurveySubjectRelevance
    {
		[Display("Неактуальна")]
        NotActual = 10,

		[Display("Актуальна")]
        Actual = 20
    }
}