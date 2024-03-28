namespace Bars.GisIntegration.UI.ViewModel.Protocol
{
    using System;

    using Bars.Gkh.Quartz.Scheduler.Log;

    /// <summary>
    /// Представление записи протокола подготовки данных
    /// </summary>
    public class PreparingDataProtocolRecordView
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
    }
}
