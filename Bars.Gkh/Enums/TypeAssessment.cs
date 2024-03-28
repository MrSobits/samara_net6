namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Оценка
    /// </summary>
    public enum TypeAssessment
    {
        [Display("Положительный")]
        Positive = 10,

        [Display("Отрицательный")]
        Negative = 20,

        [Display("Не задано")]
        NotSet = 30
    }
}
