namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Основание прекращения деятелньости лицензии
    /// </summary>
    public enum TypeManOrgTypeDocLicense
    {
        [Display("Протокол лицензионной комиссии")]
        Protocol = 10,

        [Display("Решение суда")]
        CourtDecision = 20,

        [Display("Заявление в суд")]
        StatementInCourt = 30,

        [Display("Заявка о прекращении деятельности")]
        RequestTerminationActivity = 40,

        [Display("Приказ о прекращении действия лицензии")]
        OrderToTerminateLicense = 50,

        [Display("Прочее")]
        Other = 100
    }
}
