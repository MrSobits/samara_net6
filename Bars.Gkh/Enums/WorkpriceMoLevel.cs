namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Уровень муниципального образования
    /// </summary>
    public enum WorkpriceMoLevel
    {
        [Display("Муниципальный район")]
        MunicipalUnion = 0,

        [Display("Муниципальное образование")]
        Settlement = 10,

        [Display("Оба уровня")]
        Both = 20
    }
}