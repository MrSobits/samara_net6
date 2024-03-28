namespace Bars.Gkh.SystemDataTransfer.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид интеграции
    /// </summary>
    public enum DataTransferOperationType
    {
        /// <summary>
        /// Импорт
        /// </summary>
        [Display("Импорт")]
        Import,

        /// <summary>
        /// Экспорт
        /// </summary>
        [Display("Экспорт")]
        Export
    }
}