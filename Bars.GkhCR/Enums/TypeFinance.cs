namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип разреза
    /// </summary>
    public enum TypeFinance
    {
        [Display("Другие")]
        Other = 10,

        [Display("Лизинг")]
        Leasing = 20,

        [Display("Не указано")]
        NotDefined = 30,

        [Display("Федеральный закон")]
        FederalLaw = 40
    }
}
