namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Уровень муниципального образования
    /// </summary>
    public enum MoLevel
    {
        [Display("Муниципальный район")]
        MunicipalUnion = 0,

        [Display("Муниципальное образование")]
        Settlement = 10
    }
}