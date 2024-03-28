namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Расчет количества дней просрочки
    /// </summary>
    public enum NumberDaysDelay
    {
        /// <summary>
        /// С даты начала расчетного месяца
        /// </summary>
        [Display("С даты начала расчетного месяца")]
        StartDateMonth,

        /// <summary>
        ///  С даты возникновения просроченной задолженности
        /// </summary>
        [Display("С даты возникновения просроченной задолженности")]
        StartDateDebt
    }
}