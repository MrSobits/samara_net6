using SMEV3Library.Entity;
using SMEV3Library.Namespaces;
using SMEV3Library.SoapHttpClient.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SMEV3Library.Entities
{
    /// <summary>
    /// Ответ на GetResponseRequest
    /// </summary>
    public class GetResponseResponse
    {
        #region Fields

        /// <summary>
        /// Произошла ли ошибка при отправке?
        /// </summary>
        public string Error = null;

        /// <summary>
        /// Содержит ли сообщение ответ?
        /// </summary>
        public bool isAnswerPresent = false;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string OriginalMessageId = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string MessageId = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public string To = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public XElement MessagePrimaryContent = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public XElement RequestRejected = null;

        /// <summary>
        /// См. описание обмена СМЭВ
        /// </summary>
        public XElement AsyncProcessingStatus = null;

        /// <summary>
        /// Приаттаченные файлы
        /// </summary>
        public List<FileAttachment> Attachments = null;

        /// <summary>
        /// Время получения
        /// </summary>
        public DateTime CreatedTime;

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

        #endregion

        public GetResponseResponse(HTTPResponse response, SoapVersion version)
        {
            Error = response.Error;
            SendedData = response.sendedData;
            ReceivedData = response.receivedData;
            Attachments = response.Attachments;
            CreatedTime = DateTime.Now;

            try
            {
                var GetResponseResponseElement = GetResponseXML(response.SoapXML, version);
                if (GetResponseResponseElement != null)
                {
                    switch (version)
                    {
                        case SoapVersion.Soap11:
                            {
                                XElement ResponseElement = GetResponseResponseElement.Element(SMEVNamespaces11.Namespace + "ResponseMessage")?.Element(SMEVNamespaces11.Namespace + "Response");
                                //OriginalMessageId
                                OriginalMessageId = ResponseElement.Element(SMEVNamespaces11.Namespace + "OriginalMessageId").Value;
                                //SenderProvidedResponseData
                                XElement SenderProvidedResponseDataElement = ResponseElement.Element(SMEVNamespaces11.Namespace + "SenderProvidedResponseData");

                                MessageId = SenderProvidedResponseDataElement.Element(SMEVNamespaces11.Namespace + "MessageID").Value;
                                To = SenderProvidedResponseDataElement.Element(SMEVNamespaces11.Namespace + "To").Value;
                                RequestRejected = SenderProvidedResponseDataElement.Element(SMEVNamespaces11.Namespace + "RequestRejected");
                                MessagePrimaryContent = SenderProvidedResponseDataElement.Element(SMEVNamespaces11.Namespace1 + "MessagePrimaryContent");
                                AsyncProcessingStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces11.Namespace + "AsyncProcessingStatus");
                            }
                            break;
                        case SoapVersion.Soap12:
                            {
                                var ResponseElement = GetResponseResponseElement.Element(SMEVNamespaces12.Namespace2 + "ResponseMessage")?.Element(SMEVNamespaces12.Namespace2 + "Response");
                                //OriginalMessageId
                                OriginalMessageId = ResponseElement.Element(SMEVNamespaces12.Namespace2 + "OriginalMessageId").Value;
                                //SenderProvidedResponseData
                                XElement SenderProvidedResponseDataElement = ResponseElement.Element(SMEVNamespaces12.Namespace2 + "SenderProvidedResponseData");

                                MessageId = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.Namespace2 + "MessageID").Value;
                                To = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.Namespace2 + "To").Value;
                                RequestRejected = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.Namespace2 + "RequestRejected");
                                MessagePrimaryContent = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.Namespace1 + "MessagePrimaryContent");
                                AsyncProcessingStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.Namespace2 + "AsyncProcessingStatus");
                            }
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                Error = "Ошибка разбора ответа: " + e.Message;
            }
        }

        private XElement GetResponseXML(XElement response, SoapVersion version)
        {
            switch (version)
            {
                case SoapVersion.Soap11:
                    {
                        //Проверка на Fault
                        var fault = response.Element(SMEVNamespaces11.SoapNamespace + "Fault");
                        if (fault != null)
                        {
                            FaultXML = fault;
                            return null;
                        }

                        //ничего не найдено?
                        if (response.Element(SMEVNamespaces11.Namespace + "GetResponseResponse")?.Element(SMEVNamespaces11.Namespace + "ResponseMessage") == null)
                            return null;

                        isAnswerPresent = true;
                        return response.Element(SMEVNamespaces11.Namespace + "GetResponseResponse");
                    }

                case SoapVersion.Soap12:
                    {
                        //Проверка на Fault
                        var fault = response.Element(SMEVNamespaces12.SoapNamespace + "Fault");
                        if (fault != null)
                        {
                            FaultXML = fault;
                            return null;
                        }

                        //ничего не найдено?
                        if (response.Element(SMEVNamespaces12.Namespace2 + "GetResponseResponse")?.Element(SMEVNamespaces12.Namespace2 + "ResponseMessage") == null)
                            return null;

                        isAnswerPresent = true;
                        return response.Element(SMEVNamespaces12.Namespace2 + "GetResponseResponse");
                    }
                default:
                    throw new Exception($"Версия SOAP {version} не поддерживается");
            }
        }
    }
}
