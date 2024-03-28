namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип поставщика услуги
    /// </summary>
    public enum TypeServiceOrg
    {
        [Display("Поставщик коммунальной услуги")]
        SupplyResourceOrg = 10,

        [Display("Поставщик жилищной услуги")]
        ServiceOrganization = 20
    }
}