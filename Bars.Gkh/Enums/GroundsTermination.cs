namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Основание прекращения деятельности
    /// </summary>
    public enum GroundsTermination
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Банкротство")]
        Bankruptcy = 20,

        [Display("Ликвидация")]
        Liquidation = 30,

        [Display("Организация ошибочно заведена")]
        Erroneous = 40,

        [Display("Прекращение деятельности по обслуживанию МКД")]
        TerminationMkd = 50
    }
}
