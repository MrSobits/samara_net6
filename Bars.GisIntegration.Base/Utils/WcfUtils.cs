namespace Bars.GisIntegration.Base.Utils
{
    using System.IO;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Вспомогательные методы для работы с сообщениями WCF
    /// </summary>
    public static class WcfUtils
    {
        /// <summary>
        /// Создает текстовое представление сообщения. В отличии от стандартного
        /// ToString() корректно обрабатывает тело StreamedMessage.
        /// Кроме того, не портит сообщение, автоматически восстанавливая его состояние
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>Текстовое представление</returns>
        public static string MessageToString(ref Message message)
        {
            var messageBuffer = message.CreateBufferedCopy(int.MaxValue);

            message = messageBuffer.CreateMessage();

            var s = new MemoryStream();
            var xw = XmlWriter.Create(s, new XmlWriterSettings { Indent = true });
            messageBuffer.CreateMessage().WriteMessage(xw);
            xw.Flush();
            s.Position = 0;

            var xmlBytes = new byte[s.Length];
            s.Read(xmlBytes, 0, (int)s.Length);

            // масси xmlBytes[] может начинаться с UTF8 BOM маркера
            if (xmlBytes[0] != (byte)'<')
            {
                return Encoding.UTF8.GetString(xmlBytes, 3, xmlBytes.Length - 3);
            }

            return Encoding.UTF8.GetString(xmlBytes, 0, xmlBytes.Length);
        }

        /// <summary>
        /// Создает XmlReader для строки
        /// </summary>
        /// <param name="xml">Строка</param>
        /// <returns>XmlReader</returns>
        public static XmlReader XmlReaderFromString(string xml)
        {
            var stream = new MemoryStream();

            //  конструкция using(var writer ...){...} не используется,
            //  потому как закрытие StreamWriter приводит к закрытию stream
            //  и сообщение не получается
            var writer = new StreamWriter(stream);
            writer.Write(xml);
            writer.Flush();
            stream.Position = 0;
            return XmlReader.Create(stream);
        }

        /// <summary>
        /// Создает сообщение из строки и версии.
        /// По сути, обертка над Message.CreateMessage(XmlReader, int, MessageVersion),
        /// но гарантирует правильное создание XmlReader'а
        /// </summary>
        /// <param name="xml">Строка</param>
        /// <param name="ver">Версия</param>
        /// <returns>Сообщение</returns>
        public static Message CreateMessageFromString(string xml, MessageVersion ver)
        {
            return Message.CreateMessage(WcfUtils.XmlReaderFromString(xml), int.MaxValue, ver);
        }
    }
}