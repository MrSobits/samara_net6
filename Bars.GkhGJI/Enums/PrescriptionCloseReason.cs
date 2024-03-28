namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    public enum PrescriptionCloseReason
    {
        [Display("Предписание исполнено")]
        Done = 10,
        [Display("Организация ликвидирована")]
        OrganizationLiquidated = 20,
        [Display("Сменилась управляющая организация")]
        ManOrgChanged = 30
    }
}
