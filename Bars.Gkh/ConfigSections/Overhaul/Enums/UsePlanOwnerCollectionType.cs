namespace Bars.Gkh.ConfigSections.Overhaul.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип расчета собираемости 
    /// </summary>
    public enum UsePlanOwnerCollectionType
    {
        [Display("За предыдущий год")]
        LastYear = 10,

        [Display("За текущий год")]
        CurrentYear = 20,

        [Display("Год  1 и 2 за первый год.")]
        NaoMethod = 30
    }
}