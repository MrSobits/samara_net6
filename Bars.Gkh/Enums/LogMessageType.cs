namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип сообщения в логе 
    /// </summary>
    public enum LogMessageType
    {
        /// <summary>
        /// Отладка
        /// </summary>
        [Display("Отладка")]
        Debug,

        /// <summary>
        /// Информация
        /// </summary>
        [Display("Информация")]
        Info,

        /// <summary>
        /// Предупреждение
        /// </summary>
        [Display("Предупреждение")]
        Warning,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error
    }
}