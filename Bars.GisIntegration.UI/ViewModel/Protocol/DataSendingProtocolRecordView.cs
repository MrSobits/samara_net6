namespace Bars.GisIntegration.UI.ViewModel.Protocol
{
    using System;

    using Bars.Gkh.Quartz.Scheduler.Log;

    /// <summary>
    /// Представление записи протокола отправки данных
    /// </summary>
    public class DataSendingProtocolRecordView
    {
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

        /// <summary>
        /// Имя пакета
        /// </summary>
        public string PackageName { get; set; }
    }
}
