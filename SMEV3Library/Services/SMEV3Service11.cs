using SMEV3Library.Entities;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Entities.SendRequestResponse;
using SMEV3Library.Helpers;
using SMEV3Library.Namespaces;
using SMEV3Library.Providers;
using SMEV3Library.SoapHttpClient;
using SMEV3Library.SoapHttpClient.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SMEV3Library.Services
{
    public class SMEV3Service11 : ISMEV3Service
    {
        private SMEVOptions _options;

        #region Constructors

        public SMEV3Service11(IOptionsProvider optionsProvider)
        {
            _options = optionsProvider.GetOptions();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Подтверждает получение запроса
        /// </summary>
        /// <param name="messageId">UUID сообщения</param>
        /// <param name="accepted">принято?</param>
        public async Task GetAckAsyncSGIO(string messageId, bool accepted, bool saveLog = false)
        {
            //-----------AckTargetMessage-----------
            XElement ackTargetMessageXElement = new XElement(SMEVNamespaces12.BasicNamespace + "AckTargetMessage",
                    new XAttribute("Id", "SIGNED_BY_CALLER")
                    , new XAttribute("accepted", accepted)
                    , messageId
                );
            //-----------AckRequest-----------
            XElement ackRequestXElement = new XElement(SMEVNamespaces12.TypesNamespace + "AckRequest",
                    ackTargetMessageXElement,
                    new XElement(SMEVNamespaces12.TypesNamespace + "CallerInformationSystemSignature", SignXElement(ackTargetMessageXElement, @"#SIGNED_BY_CALLER"))
                );

            //-----------Применяем namespace-----------
            ackRequestXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces12.TypesNamespace);
            ackRequestXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces12.BasicNamespace);

            await SendSoapData(ackRequestXElement, null, saveLog).ConfigureAwait(false);
        }

        /// <summary>
        /// Отправляет запрос в СМЭВ
        /// </summary>
        /// <param name="request">XML, вставляемая в MessagePrimaryContent</param>
        /// <param name="attachments">Прикладываемые файлы</param>
        /// <returns>SendRequestRequest, распарсеный в класс</returns>
        public async Task<SendRequestResponse> SendRequestAsyncPersonalSig(XElement request, List<FileAttachment> attachments = null, bool saveLog = false)
        {
            //-----------AttachmentContentList, AttachmentHeaderList-----------
            XElement AttachmentContentListXElement = null;
            XElement AttachmentHeaderListXElement = null;

            if (attachments != null && attachments.Count > 0)
            {
                AttachmentContentListXElement = new XElement(SMEVNamespaces11.BasicNamespace + "AttachmentContentList");
                AttachmentHeaderListXElement = new XElement(SMEVNamespaces11.BasicNamespace + "AttachmentHeaderList");

                foreach (FileAttachment file in attachments)
                {
                    //AttachmentHeader
                    AttachmentHeaderListXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "AttachmentHeader"
                        , new XElement(SMEVNamespaces11.BasicNamespace + "contentId", file.FileName)
                        , new XElement(SMEVNamespaces11.BasicNamespace + "MimeType", MimeTypeMap.GetMimeType(file.GetExtension()))));

                    //AttachmentContent
                    AttachmentContentListXElement.Add(new XElement("AttachmentContent"
                        , new XElement(SMEVNamespaces11.BasicNamespace + "Id", file.FileName)
                        , new XElement(SMEVNamespaces11.BasicNamespace + "Content", "cid:{file.FileName}")
                    ));
                }
            }

            //-----------SenderProvidedRequestData-----------
            var senderProvidedRequestDataXElement = new XElement(SMEVNamespaces11.TypesNamespace + "SenderProvidedRequestData",
                    new XAttribute("Id", "SIGNED_BY_CONSUMER")
                    , new XElement(SMEVNamespaces11.TypesNamespace + "MessageID", GetTimeStampUuid())
                    , new XElement(SMEVNamespaces11.BasicNamespace + "MessagePrimaryContent", request)
                );

            if (AttachmentHeaderListXElement != null)
                senderProvidedRequestDataXElement.Add(AttachmentHeaderListXElement);

            if (_options.TestMode)
                senderProvidedRequestDataXElement.Add(new XElement(SMEVNamespaces11.TypesNamespace + "TestMessage"));

            //-----------SendRequestRequest-----------
            XElement sendRequestRequestXElement = new XElement(
                    SMEVNamespaces11.TypesNamespace + "SendRequestRequest"
                    , senderProvidedRequestDataXElement);

            if (AttachmentContentListXElement != null)
                sendRequestRequestXElement.Add(AttachmentContentListXElement);

            sendRequestRequestXElement.Add(new XElement(SMEVNamespaces11.TypesNamespace + "CallerInformationSystemSignature", SignXElement(senderProvidedRequestDataXElement, @"#SIGNED_BY_CONSUMER")));

            //-----------Применяем namespace-----------
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces11.TypesNamespace);
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces11.BasicNamespace);

            var response = await SendSoapData(sendRequestRequestXElement, attachments, saveLog).ConfigureAwait(false);

            return new SendRequestResponse11(response);
        }

        /// <summary>
        /// Отправляет запрос в СМЭВ
        /// </summary>
        /// <param name="request">XML, вставляемая в MessagePrimaryContent</param>
        /// <param name="attachments">Прикладываемые файлы</param>
        /// <returns>SendRequestRequest, распарсеный в класс</returns>
        public async Task<SendRequestResponse> SendRequestAsync(XElement request, List<FileAttachment> attachments = null, bool saveLog = false)
        {
            //-----------AttachmentContentList, AttachmentHeaderList-----------
            XElement AttachmentContentListXElement = null;
            XElement AttachmentHeaderListXElement = null;

            if (attachments != null && attachments.Count > 0)
            {
                AttachmentContentListXElement = new XElement(SMEVNamespaces11.BasicNamespace + "AttachmentContentList");
                AttachmentHeaderListXElement = new XElement(SMEVNamespaces11.BasicNamespace + "AttachmentHeaderList");

                foreach (FileAttachment file in attachments)
                {
                    //AttachmentHeader
                    AttachmentHeaderListXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "AttachmentHeader"
                        , new XElement(SMEVNamespaces11.BasicNamespace + "contentId", file.FileName)
                        , new XElement(SMEVNamespaces11.BasicNamespace + "MimeType", MimeTypeMap.GetMimeType(file.GetExtension()))));

                    //AttachmentContent
                    AttachmentContentListXElement.Add(new XElement("AttachmentContent"
                        , new XElement(SMEVNamespaces11.BasicNamespace + "Id", file.FileName)
                        , new XElement(SMEVNamespaces11.BasicNamespace + "Content", "cid:{file.FileName}")
                    ));
                }
            }

            //-----------SenderProvidedRequestData-----------
            var senderProvidedRequestDataXElement = new XElement(SMEVNamespaces11.TypesNamespace + "SenderProvidedRequestData",
                    new XAttribute("Id", "SIGNED_BY_CONSUMER")
                    , new XElement(SMEVNamespaces11.TypesNamespace + "MessageID", GetTimeStampUuid())
                    , new XElement(SMEVNamespaces11.BasicNamespace + "MessagePrimaryContent", request)
                );

            if (AttachmentHeaderListXElement != null)
                senderProvidedRequestDataXElement.Add(AttachmentHeaderListXElement);

            if (_options.TestMode)
                senderProvidedRequestDataXElement.Add(new XElement(SMEVNamespaces11.TypesNamespace + "TestMessage"));

            //-----------SendRequestRequest-----------
            XElement sendRequestRequestXElement = new XElement(
                    SMEVNamespaces11.TypesNamespace + "SendRequestRequest"
                    , senderProvidedRequestDataXElement);

            if (AttachmentContentListXElement != null)
                sendRequestRequestXElement.Add(AttachmentContentListXElement);

            sendRequestRequestXElement.Add(new XElement(SMEVNamespaces11.TypesNamespace + "CallerInformationSystemSignature", SignXElement(senderProvidedRequestDataXElement, @"#SIGNED_BY_CONSUMER")));

            //-----------Применяем namespace-----------
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces11.TypesNamespace);
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces11.BasicNamespace);

            var response = await SendSoapData(sendRequestRequestXElement, attachments, saveLog).ConfigureAwait(false);

            return new SendRequestResponse11(response);
        }

        /// <summary>
        /// Отправляет запрос в СМЭВ
        /// </summary>
        /// <param name="request">XML, вставляемая в MessagePrimaryContent</param>
        /// <param name="attachments">Прикладываемые файлы</param>
        /// <returns>SendRequestRequest, распарсеный в класс</returns>
        public async Task<SendRequestResponse> SendRequestAsyncSGIO(XElement request, List<FileAttachment> attachments = null, bool saveLog = false)
        {
            //-----------AttachmentContentList, AttachmentHeaderList-----------
            XElement AttachmentContentListXElement = null;
            XElement AttachmentHeaderListXElement = null;

            if (attachments != null && attachments.Count > 0)
            {
                AttachmentContentListXElement = new XElement(SMEVNamespaces11.BasicNamespace + "AttachmentContentList");
                AttachmentHeaderListXElement = new XElement(SMEVNamespaces11.BasicNamespace + "AttachmentHeaderList");

                foreach (FileAttachment file in attachments)
                {
                    //AttachmentHeader
                    AttachmentHeaderListXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "AttachmentHeader"
                        , new XElement(SMEVNamespaces11.BasicNamespace + "contentId", file.FileName)
                        , new XElement(SMEVNamespaces11.BasicNamespace + "MimeType", MimeTypeMap.GetMimeType(file.GetExtension()))));

                    //AttachmentContent
                    AttachmentContentListXElement.Add(new XElement("AttachmentContent"
                        , new XElement(SMEVNamespaces11.BasicNamespace + "Id", file.FileName)
                        , new XElement(SMEVNamespaces11.BasicNamespace + "Content", "cid:{file.FileName}")
                    ));
                }
            }

            //-----------SenderProvidedRequestData-----------
            var senderProvidedRequestDataXElement = new XElement(SMEVNamespaces11.TypesNamespace + "SenderProvidedRequestData",
                    new XAttribute("Id", "SIGNED_BY_CONSUMER")
                    , new XElement(SMEVNamespaces11.TypesNamespace + "MessageID", GetTimeStampUuid())
                    , new XElement(SMEVNamespaces11.BasicNamespace + "MessagePrimaryContent", request)
                );

            if (AttachmentHeaderListXElement != null)
                senderProvidedRequestDataXElement.Add(AttachmentHeaderListXElement);

            if (_options.TestMode)
                senderProvidedRequestDataXElement.Add(new XElement(SMEVNamespaces11.TypesNamespace + "TestMessage"));

            //-----------SendRequestRequest-----------
            XElement sendRequestRequestXElement = new XElement(
                    SMEVNamespaces11.TypesNamespace + "SendRequestRequest"
                    , senderProvidedRequestDataXElement);

            if (AttachmentContentListXElement != null)
                sendRequestRequestXElement.Add(AttachmentContentListXElement);

            sendRequestRequestXElement.Add(new XElement(SMEVNamespaces11.TypesNamespace + "CallerInformationSystemSignature", SignXElement(senderProvidedRequestDataXElement, @"#SIGNED_BY_CONSUMER")));

            //-----------Применяем namespace-----------
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces11.TypesNamespace);
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces11.BasicNamespace);

            var response = await SendSoapData(sendRequestRequestXElement, attachments, saveLog).ConfigureAwait(false);

            return new SendRequestResponse11(response);
        }

        /// Подтверждает получение запроса
        /// </summary>
        /// <param name="messageId">UUID сообщения</param>
        /// <param name="accepted">принято?</param>
        public async Task SendResponceAsync(XElement responce, string messageId, string to, bool saveLog = false)
        {
         
        }

        /// <summary>
        /// Получает результат запроса. Если любой из параметров null, то без фильтра
        /// </summary>
        /// <param name="namespaceUri">target namespace схемы</param>
        /// <param name="rootElementLocalName">имя (local name) элемента</param>
        /// <returns>GetResponseResponse, распарсеный в класс</returns>
        public async Task<GetResponseResponse> GetResponseAsync(string namespaceUri = null, string rootElementLocalName = null, bool saveLog = false)
        {
            //-----------MessageTypeSelector-----------
            var messageTypeSelectorXElement = new XElement(SMEVNamespaces11.BasicNamespace + "MessageTypeSelector",
                    new XAttribute("Id", "SIGNED_BY_CALLER")
                );

            if (namespaceUri != null && rootElementLocalName != null)
            {
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "NamespaceURI", namespaceUri));
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "RootElementLocalName", rootElementLocalName));
            }

            messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "Timestamp", GetTimeStamp()));

            //-----------GetResponseRequest-----------
            XElement GetResponseRequest = new XElement(SMEVNamespaces11.TypesNamespace + "GetResponseRequest",
                    messageTypeSelectorXElement,
                    new XElement(SMEVNamespaces11.TypesNamespace + "CallerInformationSystemSignature", SignXElement(messageTypeSelectorXElement, @"#SIGNED_BY_CALLER"))
                );

            //-----------Применяем namespace-----------
            GetResponseRequest.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces11.TypesNamespace);
            GetResponseRequest.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces11.BasicNamespace);

            var getResponseResponseElement = await SendSoapData(GetResponseRequest, null, saveLog).ConfigureAwait(false);

            return new GetResponseResponse11(getResponseResponseElement);
        }

        /// <summary>
        /// Получает результат запроса. Если любой из параметров null, то без фильтра
        /// </summary>
        /// <param name="namespaceUri">target namespace схемы</param>
        /// <param name="rootElementLocalName">имя (local name) элемента</param>
        /// <returns>GetResponseResponse, распарсеный в класс</returns>
        public async Task<GetResponseResponse> GetResponseAsyncSGIO(string soapAction, string namespaceUri = null, string rootElementLocalName = null, bool saveLog = false)
        {
            //-----------MessageTypeSelector-----------
            var messageTypeSelectorXElement = new XElement(SMEVNamespaces11.BasicNamespace + "MessageTypeSelector",
                    new XAttribute("Id", "SIGNED_BY_CALLER")
                );

            if (namespaceUri != null && rootElementLocalName != null)
            {
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "NamespaceURI", namespaceUri));
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "RootElementLocalName", rootElementLocalName));
            }

            messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "Timestamp", GetTimeStamp()));

            //-----------GetResponseRequest-----------
            XElement GetResponseRequest = new XElement(SMEVNamespaces11.TypesNamespace + "GetResponseRequest",
                    messageTypeSelectorXElement,
                    new XElement(SMEVNamespaces11.TypesNamespace + "CallerInformationSystemSignature", SignXElement(messageTypeSelectorXElement, @"#SIGNED_BY_CALLER"))
                );

            //-----------Применяем namespace-----------
            GetResponseRequest.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces11.TypesNamespace);
            GetResponseRequest.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces11.BasicNamespace);

            var getResponseResponseElement = await SendSoapData(GetResponseRequest, null, saveLog).ConfigureAwait(false);

            return new GetResponseResponse11(getResponseResponseElement);
        }

        /// <summary>
        /// Получает результат запроса. Если любой из параметров null, то без фильтра
        /// </summary>
        /// <param name="namespaceUri">target namespace схемы</param>
        /// <param name="rootElementLocalName">имя (local name) элемента</param>
        /// <returns>GetResponseResponse, распарсеный в класс</returns>
        public async Task<GetRequestRequest> GetRequestAsyncSGIO(string soapAction, string namespaceUri = null, string rootElementLocalName = null, bool saveLog = false)
        {
            //-----------MessageTypeSelector-----------
            var messageTypeSelectorXElement = new XElement(SMEVNamespaces11.BasicNamespace + "MessageTypeSelector",
                    new XAttribute("Id", "SIGNED_BY_CALLER")
                );

            if (namespaceUri != null && rootElementLocalName != null)
            {
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "NamespaceURI", namespaceUri));
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "RootElementLocalName", rootElementLocalName));
            }

            messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces11.BasicNamespace + "Timestamp", GetTimeStamp()));

            //-----------GetResponseRequest-----------
            XElement GetResponseRequest = new XElement(SMEVNamespaces11.TypesNamespace + "GetResponseRequest",
                    messageTypeSelectorXElement,
                    new XElement(SMEVNamespaces11.TypesNamespace + "CallerInformationSystemSignature", SignXElement(messageTypeSelectorXElement, @"#SIGNED_BY_CALLER"))
                );

            //-----------Применяем namespace-----------
            GetResponseRequest.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces11.TypesNamespace);
            GetResponseRequest.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces11.BasicNamespace);

            var getResponseResponseElement = await SendSoapData(GetResponseRequest, null, saveLog).ConfigureAwait(false);

            return new GetRequestRequest(getResponseResponseElement);
        }

        /// <summary>
        /// Подтверждает получение запроса
        /// </summary>
        /// <param name="messageId">UUID сообщения</param>
        /// <param name="accepted">принято?</param>
        public async Task GetAckAsync(string messageId, bool accepted, bool saveLog = false)
        {
            //-----------AckTargetMessage-----------
            XElement ackTargetMessageXElement = new XElement(SMEVNamespaces11.BasicNamespace + "AckTargetMessage",
                    new XAttribute("Id", "SIGNED_BY_CALLER")
                    , new XAttribute("accepted", accepted)
                    , messageId
                );

            //-----------AckRequest-----------
            XElement ackRequestXElement = new XElement(SMEVNamespaces11.TypesNamespace + "AckRequest",
                    ackTargetMessageXElement,
                    new XElement(SMEVNamespaces11.TypesNamespace + "CallerInformationSystemSignature", SignXElement(ackTargetMessageXElement, @"#SIGNED_BY_CALLER"))
                );

            //-----------Применяем namespace-----------
            ackRequestXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces11.TypesNamespace);
            ackRequestXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces11.BasicNamespace);

            await SendSoapData(ackRequestXElement, null, saveLog).ConfigureAwait(false);
        }

        public byte[] SignFileDetached2012256(string filePath)
        {
            return CryptoProHelper.SignFileDetached2012256(filePath);
        }

        #endregion

        #region Private methods

        private string GetTimeStamp()
        {
            //"2014-02-11T17:10:03.616+04:00"
            return DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
        }

        private XElement SignXElement(XElement element, string signedNode)
        {
            return XElement.Parse(CryptoProHelper.Sign(XMLToLinqHelper.ToXmlDocument(element), signedNode).OuterXml);
        }

        private string GetTimeStampUuid()
        {
            return GUIDHelper.GenerateTimeBasedGuid().ToString();
        }

        private async Task<HTTPResponse> SendSoapData(XElement body, IEnumerable<FileAttachment> attachments = null, bool saveLog = false)
        {
            using (var soapClient = new SoapClient())
            {
                return
                  await soapClient.PostAsync(
                          endpoint: new Uri(_options.Endpoint),
                          soapVersion: SoapVersion.Soap11,
                          bodies: new List<XElement>() { body },
                          attachments: attachments,
                          storeLog: saveLog
                          ).ConfigureAwait(false);
            }
        }

        #endregion
    }
}
