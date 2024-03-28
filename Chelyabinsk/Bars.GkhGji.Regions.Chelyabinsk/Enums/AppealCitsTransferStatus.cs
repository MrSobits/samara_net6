namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус операции трансфера обращений
    /// </summary>
    public enum AppealCitsTransferStatus
    {
        /// <summary>
        /// Выполняется
        /// </summary>
        [Display("Выполняется")]
        Running,

        /// <summary>
        /// Успешно выполнено
        /// </summary>
        [Display("Успешно выполнено")]
        Success,

        /// <summary>
        /// Завершено с ошибкой
        /// </summary>
        [Display("Завершено с ошибкой")]
        Failure
    }
}
