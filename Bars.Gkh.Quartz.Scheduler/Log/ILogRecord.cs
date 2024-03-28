namespace Bars.Gkh.Quartz.Scheduler.Log
{
    using System;

    /// <summary>
    /// Интерфейс записи лога фонового процесса
    /// </summary>    
    public interface ILogRecord
    {
        /// <summary>
        /// Текст сообщения
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Тип сообщения
        /// </summary>
        MessageType Type { get; set; }

        /// <summary>
        /// Дата время сообщения
        /// </summary>
        DateTime DateTime { get; set; }
    }
}
