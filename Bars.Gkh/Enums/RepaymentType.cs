namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип погашения
    /// </summary>
    public enum RepaymentType
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        None = 0,

        /// <summary>
        /// Погашено полностью
        /// </summary>
        [Display("Погашено полностью")]
        PaidInFull = 10,

        /// <summary>
        /// Не погашено
        /// </summary>
        [Display("Не погашено")]
        DoNotRepaid = 20,

    }
}