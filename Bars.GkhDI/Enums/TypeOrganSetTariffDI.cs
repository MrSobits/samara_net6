namespace Bars.GkhDi.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип организации установившей тариф
    /// </summary>
    public enum TypeOrganSetTariffDi
    {
        [Display("Орган, ответственный за утверждение тарифа")]
        Organ = 10,

        [Display("Управляющая организация")]
        Uk = 20
    }
}
