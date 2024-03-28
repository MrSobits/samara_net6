namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус отправки платежного документа на почту
    /// </summary>
    public enum PaymentDocumentSendingEmailState
    {
        /// <summary>
        /// Не отправлен
        /// </summary>
        [Display("Не отправлен")]
        NotSent = 0,

        /// <summary>
        /// Отправлен
        /// </summary>
        [Display("Отправлен")]
        Sent = 1,

        /// <summary>
        /// В очереди
        /// </summary>
        [Display("В очереди")]
        Queue = 2
    }
}