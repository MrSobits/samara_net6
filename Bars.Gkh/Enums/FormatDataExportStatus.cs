namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус экспорта по формату
    /// </summary>
    public enum FormatDataExportStatus
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        Default,

        /// <summary>
        /// Ожидает выполнения
        /// </summary>
        [Display("Ожидает выполнения")]
        Pending,

        /// <summary>
        /// Выполняется
        /// </summary>
        [Display("Выполняется")]
        Running,

        /// <summary>
        /// Выполнена успешно
        /// </summary>
        [Display("Выполнена успешно")]
        Successfull,

        /// <summary>
        /// Выполнена с ошибкой
        /// </summary>
        [Display("Выполнена с ошибкой")]
        Error,

        /// <summary>
        /// Ошибка при выполнении
        /// </summary>
        [Display("Ошибка при выполнении")]
        RuntimeError,

        /// <summary>
        /// Прервано пользователем
        /// </summary>
        [Display("Прервано пользователем")]
        Aborted
    }
}