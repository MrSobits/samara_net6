using SMEV3Library.Entity;
using SMEV3Library.Namespaces;
using SMEV3Library.SoapHttpClient.Enums;
using System;
using System.Xml.Linq;

namespace SMEV3Library.Entities
{
    /// <summary>
    /// Ответ на SendRequestRequest
    /// </summary>
    public class SendRequestResponse
    {
        /// <summary>
        /// Произошла ли ошибка при отправке?
        /// </summary>
        public string Error = null;

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

        public SendRequestResponse(HTTPResponse response, SoapVersion version)
        {
            Error = response.Error;

            SendedData = response.sendedData;
            ReceivedData = response.receivedData;

            try
            {
                var sendRequestResponseXml = GetResponseXML(response, version);

                if (sendRequestResponseXml != null)
                {
                    switch (version)
                    {
                        case SoapVersion.Soap11:
                            {
                                XElement MessageMetadataElement = sendRequestResponseXml.Element(SMEVNamespaces11.Namespace + "MessageMetadata");
                                if (MessageMetadataElement == null)
                                    throw new Exception("MessageMetadataElement == null");

                                MessageId = MessageMetadataElement.Element(SMEVNamespaces11.Namespace + "MessageId")?.Value;
                                MessageType = MessageMetadataElement.Element(SMEVNamespaces11.Namespace + "MessageType")?.Value;
                                Sender = MessageMetadataElement.Element(SMEVNamespaces11.Namespace + "Sender")?.Element(SMEVNamespaces11.Namespace + "Mnemonic")?.Value;
                                SendingTimestamp = MessageMetadataElement.Element(SMEVNamespaces11.Namespace + "SendingTimestamp")?.Value;
                                Recipient = MessageMetadataElement.Element(SMEVNamespaces11.Namespace + "Recipient")?.Element(SMEVNamespaces11.Namespace + "Mnemonic")?.Value;
                                Status = MessageMetadataElement.Element(SMEVNamespaces11.Namespace + "Status")?.Value;
                            }
                            break;
                        case SoapVersion.Soap12:
                            {
                                XElement MessageMetadataElement = sendRequestResponseXml.Element(SMEVNamespaces12.Namespace + "MessageMetadata");
                                if (MessageMetadataElement == null)
                                    throw new Exception("MessageMetadataElement == null");

                                MessageId = MessageMetadataElement.Element(SMEVNamespaces12.Namespace + "MessageId")?.Value;
                                MessageType = MessageMetadataElement.Element(SMEVNamespaces12.Namespace + "MessageType")?.Value;
                                Sender = MessageMetadataElement.Element(SMEVNamespaces12.Namespace + "Sender")?.Element(SMEVNamespaces12.Namespace + "Mnemonic")?.Value;
                                SendingTimestamp = MessageMetadataElement.Element(SMEVNamespaces12.Namespace + "SendingTimestamp")?.Value;
                                Recipient = MessageMetadataElement.Element(SMEVNamespaces12.Namespace + "Recipient")?.Element(SMEVNamespaces12.Namespace + "Mnemonic")?.Value;
                                Status = MessageMetadataElement.Element(SMEVNamespaces12.Namespace + "Status")?.Value;
                            }
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                Error = $"Ошибка разбора ответа: {e.Message}";
            }
        }

        private XElement GetResponseXML(HTTPResponse response, SoapVersion version)
        {
            switch (version)
            {
                case SoapVersion.Soap11:
                    {
                        //Проверка на Fault
                        var fault = response.SoapXML.Element(SMEVNamespaces11.SoapNamespace + "Fault");
                        if (fault != null)
                        {
                            FaultXML = fault;
                            return null;
                        }

                        return response.SoapXML.Element(SMEVNamespaces11.Namespace + "SendRequestResponse");
                    }
                case SoapVersion.Soap12:
                    {
                        //Проверка на Fault
                        var fault = response.SoapXML.Element(SMEVNamespaces12.SoapNamespace + "Fault");
                        if (fault != null)
                        {
                            FaultXML = fault;
                            return null;
                        }

                        return response.SoapXML.Element(SMEVNamespaces12.Namespace2 + "SendRequestResponse");
                    }
                default:
                    throw new Exception($"Версия SOAP {version} не поддерживается");
            }
        }
    }
}
