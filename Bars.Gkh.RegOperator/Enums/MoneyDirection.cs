namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Направление движения денег (приход/расход)
    /// </summary>
    public enum MoneyDirection
    {
        /// <summary>
        /// приход
        /// </summary>
        [Display("Приход")]
        Income = 0,

        /// <summary>
        /// расход
        /// </summary>
        [Display("Расход")]
        Outcome = 10
    }
}