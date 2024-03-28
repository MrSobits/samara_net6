namespace Bars.Gkh.Quartz.Scheduler.Log
{
    using System;

    /// <summary>
    /// Класс записи лога фонового процесса
    /// </summary>
    [Serializable]
    public class BaseLogRecord : ILogRecord
    {
        /// <summary>
        /// Конструктор базовой записи лога
        /// </summary>
        /// <param name="dateTime">Дата время сообщения</param>
        /// <param name="text">Текст сообщения</param>
        /// <param name="messageType">Тип сообщения</param>
        public BaseLogRecord(DateTime dateTime, MessageType messageType, string text)
        {
            this.DateTime = dateTime;
            this.Text = text;
            this.Type = messageType;
        }

        /// <summary>
        /// Конструктор базовой записи лога
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="messageType">Тип сообщения</param>
        public BaseLogRecord(MessageType messageType, string text) :
            this(DateTime.Now, messageType, text)
        {
        }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Тип сообщения
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// Дата время сообщения
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
