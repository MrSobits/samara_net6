namespace Bars.Gkh.ConfigSections.Overhaul.Enums
{
    using B4.Utils;

    /// <summary>
    /// Способ корректировки
    /// </summary>
    public enum TypeCorrection
    {
        [Display("Без фиксации года")]
        WithoutFixYear = 0,

        [Display("С фиксацией года")]
        WithFixYear = 10
    }
}