namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид действия при установке/смене сальдо
    /// </summary>
    public enum SaldoChangeOperationType
    {
        /// <summary>
        /// Изменение сальдо вручную
        /// </summary>
        [Display("Изменение сальдо вручную")]
        Manual = 10,

        /// <summary>
        /// Перенос долга и переплаты по сальдо
        /// </summary>
        [Display("Перенос долга и переплаты по сальдо")]
        DebtAndOverpayment = 20,

        /// <summary>
        /// Перенос переплаты по сальдо
        /// </summary>
        [Display("Перенос переплаты по сальдо")]
        Overpayment = 30,

        /// <summary>
        /// Обнуление сальдо
        /// </summary>
        [Display("Обнуление сальдо")]
        SetToZero = 40
    }
}