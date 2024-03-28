using System;
using System.Xml.Linq;

namespace SMEV3Library.Entities
{
    public abstract class SMEVResponse
    {
        /// <summary>
        /// Произошла ли ошибка при отправке?
        /// </summary>
        public Error Error = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string MessageId = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string MessageType = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string Sender = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string SendingTimestamp = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string Recipient = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string Status = null;

        /// <summary>
        /// Если сервер вернул fault, это нода fault, иначе null
        /// </summary>
        public XElement FaultXML = null;

        /// <summary>
        /// Снапшот содержимого отправленного пакета для отладки
        /// </summary>
        public byte[] SendedData;

        /// <summary>
        /// Снапшот содержимого принятого пакета для отладки
        /// </summary>
        public byte[] ReceivedData;
    }
}