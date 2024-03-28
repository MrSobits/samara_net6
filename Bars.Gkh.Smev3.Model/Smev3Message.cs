namespace Bars.Gkh.Smev3
{
    using System;

    using Bars.Gkh.Smev3.Attachments;

    /// <summary>
    /// Базовая сущность обмена со СМЭВ 3.0
    /// </summary>
    [Serializable]
    public abstract class Smev3Message
    {
        public const string JsonType = "application/json";

        public const string XmlType = "application/xml";

        /// <summary>
        /// Идентификатор запроса к Шлюзу СМЭВ 3.0,
        /// отличается от MessageId СМЭВ
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Признак тестового сообщения
        /// </summary>
        public bool IsTestMessage { get; set; }

        /// <summary>
        /// Вложения
        /// </summary>
        public Smev3Attachment[] Attachments { get; set; }

        /// <summary>
        /// Тип содержимого
        /// </summary>
        public string DataContentType { get; set; } = Smev3Message.XmlType;
    }
}