namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус выполнения задачи
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// Ожидает выполнения
        /// </summary>
        [Display("Ожидает выполнения")]
        Waiting = 10,

        /// <summary>
        /// Выполняется
        /// для задач без детализации этапов выполнения 
        /// при подписывании данных на сервере - без участия пользователя
        /// </summary>
        [Display("Выполняется")]
        Processing = 20,

        /// <summary>
        /// Подготовка данных
        /// </summary>
        [Display("Подготовка данных")]
        PreparingData = 30,

        /// <summary>
        /// Ожидает подписывания данных
        /// </summary>
        [Display("Ожидает подписывания данных")]
        SignDataWaiting = 40,

        /// <summary>
        /// Ожидание отправки данных
        /// </summary>
        [Display("Ожидание отправки данных")]
        SendDataWaiting = 50,

        /// <summary>
        /// Отправка данных
        /// </summary>
        [Display("Отправка данных")]
        SendingData = 60,

        /// <summary>
        /// Выполнение приостановлено
        /// </summary>
        [Display("Выполнение приостановлено")]
        Paused = 70,

        /// <summary>
        /// Выполнена успешно
        /// </summary>
        [Display("Выполнена успешно")]
        CompleteSuccess = 80,

        /// <summary>
        /// Выполнена c ошибками
        /// </summary>
        [Display("Выполнена c ошибками")]
        CompleteWithErrors = 90,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error = 100,

        /// <summary>
        /// Не определен
        /// </summary>
        [Display("Не определен")]
        Undefined = 110
    }
}
