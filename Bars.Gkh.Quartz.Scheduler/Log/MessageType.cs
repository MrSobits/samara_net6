namespace Bars.Gkh.Quartz.Scheduler.Log
{
    using Bars.B4.Utils;

    /// <summary>
    /// Перечисление типов сообщений
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Информационное сообщение
        /// </summary>
        [Display("Информация")]
        Info = 10,

        /// <summary>
        /// Сообщение Предупреждение
        /// </summary>
        [Display("Предупреждение")]
        Warning = 20,

        /// <summary>
        /// Сообщение Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error = 30
    }
}
