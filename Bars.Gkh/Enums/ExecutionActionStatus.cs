namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус выполнения действия
    /// </summary>
    public enum ExecutionActionStatus
    {
        /// <summary>Создано</summary>
        [Display("Создано")]
        Initial,

        /// <summary>В очереди</summary>
        [Display("В очереди")]
        Queued,

        /// <summary>В работе</summary>
        [Display("В работе")]
        Running,

        /// <summary>Успешно выполнено</summary>
        [Display("Успешно выполнено")]
        Success,

        /// <summary>Завершено с ошибкой</summary>
        [Display("Завершено с ошибкой")]
        Error,

        /// <summary>Ошибка при выполнении</summary>
        [Display("Ошибка при выполнении")]
        RuntimeError,

        /// <summary>Непредвиденная ошибка</summary>
        [Display("Непредвиденная ошибка")]
        UnexpectedError,

        /// <summary>Отменена пользователем</summary>
        [Display("Отменена пользователем")]
        Cancelled,

        /// <summary>Прервано при перезапуске</summary>
        [Display("Прервано при перезапуске")]
        AbortedOnRestart
    }
}