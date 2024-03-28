namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Видимость программы капитального ремонта
    /// </summary>
    public enum TypeVisibilityProgramCr
    {
        [Display("Полная")]
        Full = 10,

        [Display("При печати")]
        Print = 20,

        [Display("Скрытая")]
        Hidden = 30
    }
}
