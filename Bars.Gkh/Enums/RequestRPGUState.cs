namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус выполнения действия
    /// </summary>
    public enum RequestRPGUState
    {
        /// <summary>Создано</summary>
        [Display("Направлено")]
        Sended = 10,

        /// <summary>В очереди</summary>
        [Display("Получен ответ")]
        Queued = 20,

        /// <summary>В работе</summary>
        [Display("Ожидание отправки")]
        Waiting = 30,
        
    }
}