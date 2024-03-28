namespace Bars.Gkh.Enums.ClaimWork
{
    using Bars.B4.Utils;

    /// <summary>
    ///     Способ заполнения значения
    /// </summary>
    [Display("Способ заполнения значения")]
    public enum FillType
    {
        /// <summary>
        ///     Заполнять в форме
        /// </summary>
        [Display("Заполнять в форме")]
        Manual,

        /// <summary>
        ///     Заполнять автоматически
        /// </summary>
        [Display("Заполнять автоматически")]
        Automatic = 10
    }
}