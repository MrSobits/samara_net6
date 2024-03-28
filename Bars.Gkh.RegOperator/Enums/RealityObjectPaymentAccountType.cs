namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип счета оплат
    /// </summary>
    public enum RealityObjectPaymentAccountType
    {
        /// <summary>
        /// Специальный
        /// </summary>
        [Display("Специальный")]
        Special = 0x0,

        /// <summary>
        /// Общий счет (рег. оператора)
        /// </summary>
        [Display("Общий счет (рег. оператора)")]
        Common = 0x1
    }
}