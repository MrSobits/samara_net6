using GisGkhLibrary.Utils;
using GisGkhLibrary.Xades.Impl;
using System;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GisGkhLibrary.Crypto
{
    public class SignatureMessageInspector : IClientMessageInspector
    {
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // Это для подписи на бэке

            ////remove extra tags VsDebuggerCausalityData
            //int limit = request.Headers.Count;
            //for (int i = 0; i < limit; ++i)
            //{
            //    if (request.Headers[i].Name.Equals("VsDebuggerCausalityData"))
            //    {
            //        request.Headers.RemoveAt(i);
            //        break;
            //    }
            //}

            //string st = GetSignElement(MessageString(ref request));
                        
            ////place for log request

            //request = CreateMessageFromString(st, request.Version);

            var message = MessageToString(ref request);

            var document = new XmlDocument();
            document.LoadXml(message);

            var prefixer = new XmlNsPrefixer();
            prefixer.Process(document);

            request = CreateMessageFromString(document.OuterXml, request.Version);

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            string st = MessageString(ref reply);

            //place for log response

            reply = CreateMessageFromString(st, reply.Version);
        }

        static string GetSignElement(string messageString)
        {
            var originalDoc = new XmlDocument { PreserveWhitespace = true };
            originalDoc.LoadXml(messageString);

            var nodes = originalDoc.SelectNodes($"//node()[@Id='{Params.ContainerId}']");
            if (nodes == null || nodes.Count == 0)
            {
                return originalDoc.OuterXml;
            }

            var gostXadesBesService = new GostXadesBesService();

            string st = gostXadesBesService.Sign(messageString, Params.ContainerId, Params.CertificateThumbprint, "");

            return st;
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
            return Message.CreateMessage(XmlReaderFromString(xml), int.MaxValue, ver);
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

        String MessageString(ref Message m)
        {
            // copy the message into a working buffer.
            MessageBuffer mb = m.CreateBufferedCopy(int.MaxValue);

            // re-create the original message, because "copy" changes its state.
            m = mb.CreateMessage();

            Stream s = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(s);
            mb.CreateMessage().WriteMessage(xw);
            xw.Flush();
            s.Position = 0;

            byte[] bXml = new byte[s.Length];
            s.Read(bXml, 0, (int)s.Length);

            // sometimes bXML[] starts with a BOM
            if (bXml[0] != (byte)'<')
            {
                return Encoding.UTF8.GetString(bXml, 3, bXml.Length - 3);
            }
            return Encoding.UTF8.GetString(bXml, 0, bXml.Length);
        }
    }
}