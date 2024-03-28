namespace Bars.GisIntegration.Base.Tasks.SendData
{
    using System;

    using Bars.Gkh.Quartz.Scheduler.Log;

    /// <summary>
    /// Запись лога отправки пакетов
    /// </summary>
    [Serializable]
    public class PackageSendingLogRecord: BaseLogRecord
    {
        /// <summary>
        /// Конструктор записи лога отправки пакетов
        /// </summary>
        /// <param name="dateTime">Дата время сообщения</param>
        /// <param name="packageName">Имя пакета</param>
        /// <param name="text">Текст сообщения</param>
        /// <param name="messageType">Тип сообщения</param>
        public PackageSendingLogRecord(DateTime dateTime, MessageType messageType, string packageName, string text):
            base(dateTime, messageType, text)
        {
            this.PackageName = packageName;
        }

        /// <summary>
        /// Конструктор записи лога отправки пакетов
        /// </summary>
        /// <param name="packageName">Имя пакета</param>
        /// <param name="text">Текст сообщения</param>
        /// <param name="messageType">Тип сообщения</param>
        public PackageSendingLogRecord(MessageType messageType, string packageName, string text) :
            base(messageType, text)
        {
            this.PackageName = packageName;
        }

        /// <summary>
        /// Имя пакета
        /// </summary>
        public string PackageName { get; set; }
    }
}
