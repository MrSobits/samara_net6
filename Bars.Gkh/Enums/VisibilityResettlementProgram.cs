namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Видимость программы переселения
    /// </summary>
    public enum VisibilityResettlementProgram
    {
        [Display("Полная")]
        Full = 10,

        [Display("Только при печати")]
        OnlyPrint = 20,

        [Display("Скрыта")]
        Hide = 30
    }
}
