namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Статус подтверждения оплат
    /// </summary>
    public enum ImportedPaymentPaymentConfirmState
    {
        /// <summary>
        /// Не подтверждена
        /// </summary>
        [Display("Не подтверждена")]
        NotDistributed = 10,

        /// <summary>
        /// Подтверждена
        /// </summary>
        [Display("Подтверждена")]
        Distributed = 20,

        /// <summary>
        /// Удалена
        /// </summary>
        [Display("Удалена")]
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
