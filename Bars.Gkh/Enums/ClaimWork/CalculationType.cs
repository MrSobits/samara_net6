namespace Bars.Gkh.Enums.ClaimWork
{
    using Bars.B4.Utils;

    /// <summary>
    ///     Способ расчета значения
    /// </summary>
    [Display("Способ расчета значения")]
    public enum CalculationType
    {
        /// <summary>
        ///     Заполнять в форме
        /// </summary>
        [Display("Заполнять в форме")]
        Manual,

        /// <summary>
        ///     Расчитывать автоматически
        /// </summary>
        [Display("Рассчитывать автоматически")]
        Automatic = 10
    }
}