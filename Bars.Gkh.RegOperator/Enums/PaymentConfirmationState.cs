namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Статус подтверждения оплат
    /// </summary>
    public enum PaymentConfirmationState
    {
        /// <summary>
        /// Не подтвержден
        /// </summary>
        [Display("Не подтвержден")]
        NotDistributed = 10,

        /// <summary>
        /// Частично подтвержден
        /// </summary>
        [Display("Частично подтвержден")]
        PartiallyDistributed = 20,

        /// <summary>
        /// Подтвержден
        /// </summary>
        [Display("Подтвержден")]
        Distributed = 30,

        /// <summary>
        /// Удален
        /// </summary>
        [Display("Удален")]
        Deleted = 40,

        /// <summary>
        /// Ожидание подтверждения
        /// </summary>
        [Display("Ожидание подтверждения")]
        WaitingConfirmation = 50,

        /// <summary>
        /// Ожидание отмены подтверждения
        /// </summary>
        [Display("Ожидание отмены подтверждения")]
        WaitingCancellation = 60,

        /// <summary>
        /// Невозможно подтвердить
        /// </summary>
        [Display("Невозможно подтвердить")]
        ConfirmationImpossible = 70
    }
}