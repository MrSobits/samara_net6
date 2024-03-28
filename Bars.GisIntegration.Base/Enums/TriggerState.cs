namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Состояние выполнения триггера
    /// </summary>
    public enum TriggerState
    {
        /// <summary>
        /// Ожидает выполнения
        /// </summary>
        [Display("Ожидает выполнения")]
        Waiting = 10,

        /// <summary>
        /// Выполняется
        /// </summary>
        [Display("Выполняется")]
        Processing = 20,

        /// <summary>
        /// Выполнение приостановлено
        /// </summary>
        [Display("Выполнение приостановлено")]
        Paused = 30,

        /// <summary>
        /// Выполнено успешно
        /// </summary>
        [Display("Выполнено успешно")]
        CompleteSuccess = 40,

        /// <summary>
        /// Выполнено c ошибками
        /// </summary>
        [Display("Выполнено c ошибками")]
        CompleteWithErrors = 50,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error = 60,

        /// <summary>
        /// Не определен
        /// </summary>
        [Display("Не определен")]
        Undefined = 70
    }
}
