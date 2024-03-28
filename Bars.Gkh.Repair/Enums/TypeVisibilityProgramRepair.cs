namespace Bars.Gkh.Repair.Enums
{
    using B4.Utils;

    /// <summary>
    /// Видимость программы текущего ремонта
    /// </summary>
    public enum TypeVisibilityProgramRepair
    {
        [Display("Полная")]
        Full = 10,

        [Display("При печати")]
        Print = 20,

        [Display("Скрытая")]
        Hidden = 30
    }
}
