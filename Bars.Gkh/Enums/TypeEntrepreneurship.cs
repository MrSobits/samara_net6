namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип предпринимательства
    /// </summary>
    public enum TypeEntrepreneurship
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Среднее")]
        Average = 10,

        [Display("Малое")]
        Small = 20,

        [Display("Микро")]
        Micro = 30
    }
}
