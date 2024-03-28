namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус импортируемой оплаты
    /// </summary>
    public enum ImportedPaymentState
    {
        /// <summary>
        /// Реестр неподтвержденных оплат
        /// </summary>
        [Display("РНО")]
        Rno = 10,

        /// <summary>
        /// Реестр невыясненных сумм
        /// </summary>
        [Display("РНС")]
        Rns = 20
    }
}