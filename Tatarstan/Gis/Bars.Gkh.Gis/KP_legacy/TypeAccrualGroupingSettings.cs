namespace Bars.Gkh.Gis.KP_legacy
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип группировки
    /// </summary>
    public enum TypeAccrualGroupingSettings
    {
        [Display("Группировать по месяцу")]
        ActGroupByMonth = 520,

        [Display("Группировать по услуге")]
        ActGroupByService = 521,

        [Display("Группировать по поставщику")]
        ActGroupBySupplier = 522,

        [Display("Группировать по формуле")]
        ActGroupByFormula = 523,

        [Display("Группировать по управляющей организации")]
        ActGroupByManagementOrganization = 524,

        [Display("Показывать сальдовые показатели")]
        ActShowSaldo = 528,

        [Display("Группировать по получателю")]
        ActGroupByRecipient = 543
    }
}