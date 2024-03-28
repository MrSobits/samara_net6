namespace Bars.Gkh.Modules.Gkh1468.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Перечисление для установки периода ввода показаний приборов учёта
    /// </summary>
    public enum MonthType
    {
        /// <summary>
        /// Текущего месяца
        /// </summary>
        [Display("Текущего месяца")]
        CurrentMonth,

        /// <summary>
        /// Следующего месяца
        /// </summary>
        [Display("Следующего месяца")]
        NextMonth
    }
}
