namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Результат обследования
    /// </summary>
    public enum SurveyResult
    {
        [Display("Не обследовано")]
        NotSurveyed = 10,

        [Display("Обследовано")]
        Surveyed = 20
    }
}