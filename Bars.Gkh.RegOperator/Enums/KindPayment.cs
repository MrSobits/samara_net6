namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Вид платежа
    /// </summary>
    public enum KindPayment
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Электронно
        /// </summary>
        [Display("Электронно")]
        Electronic = 10,

        /// <summary>
        /// Почтой
        /// </summary>
        [Display("Почтой")]
        ByPost = 20
    }
}