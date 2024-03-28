namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус операции 
    /// </summary>
    public enum OperationStatus
    {
        /// <summary>
        /// Проверено
        /// </summary>
        [Display("Проверено")]
        Default = 0x0,

        /// <summary>
        /// Проверено
        /// </summary>
        [Display("Проверено")]
        Approved = 0x1,

        /// <summary>
        /// Ожидание
        /// </summary>
        [Display("Ожидание")]
        Pending = 0x2
    }
}