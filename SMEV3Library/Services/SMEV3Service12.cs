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
    public class SMEV3Service12 : ISMEV3Service
    {
        private SMEVOptions _options;

        #region Constructors

        public SMEV3Service12(IOptionsProvider optionsProvider)
        {
            _options = optionsProvider.GetOptions();
        }

        #endregion

        #region Public methods

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
                AttachmentContentListXElement = new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentContentList");
                AttachmentHeaderListXElement = new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentHeaderList");

                foreach (FileAttachment file in attachments)
                {
                    //AttachmentHeader
                    AttachmentHeaderListXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentHeader"
                        , new XElement(SMEVNamespaces12.BasicNamespace + "contentId", file.FileName)
                        , new XElement(SMEVNamespaces12.BasicNamespace + "MimeType", MimeTypeMap.GetMimeType(file.GetExtension()))));

                    //AttachmentContent
                    AttachmentContentListXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentContent"
                        , new XElement(SMEVNamespaces12.BasicNamespace + "Id", file.FileName)
                        //, new XElement(SMEVNamespaces12.BasicNamespace + "Content", $"cid:{file.FileName}"))
                        , new XElement(SMEVNamespaces12.BasicNamespace + "Content",
                            new XElement(SMEVNamespaces12.XopNamespace + "Include",
                                new XAttribute("href", $"cid:{file.FileName}"),
                                new XAttribute(XNamespace.Xmlns + "xop", SMEVNamespaces12.XopNamespace)))));
                }
            }

            //-----------SenderProvidedRequestData-----------
            var senderProvidedRequestDataXElement = new XElement(SMEVNamespaces12.TypesNamespace + "SenderProvidedRequestData",
                    new XAttribute("Id", "SIGNED_BY_CONSUMER")
                    , new XElement(SMEVNamespaces12.TypesNamespace + "MessageID", GetTimeStampUuid())
                    , new XElement(SMEVNamespaces12.BasicNamespace + "MessagePrimaryContent", request)
                );

            if (AttachmentHeaderListXElement != null)
                senderProvidedRequestDataXElement.Add(AttachmentHeaderListXElement);

            if (_options.TestMode)
                senderProvidedRequestDataXElement.Add(new XElement(SMEVNamespaces12.TypesNamespace + "TestMessage"));

            senderProvidedRequestDataXElement.Add(new XElement(SMEVNamespaces12.TypesNamespace + "PersonalSignature", SignXElementPersonalSig(request, @"#PERSONAL_SIGNATURE")));

            //-----------SendRequestRequest-----------
            XElement sendRequestRequestXElement = new XElement(
                    SMEVNamespaces12.TypesNamespace + "SendRequestRequest"
                    , senderProvidedRequestDataXElement);

            if (AttachmentContentListXElement != null)
                sendRequestRequestXElement.Add(AttachmentContentListXElement);
            

            sendRequestRequestXElement.Add(new XElement(SMEVNamespaces12.TypesNamespace + "CallerInformationSystemSignature", SignXElement(senderProvidedRequestDataXElement, @"#SIGNED_BY_CONSUMER")));

            //-----------Применяем namespace-----------
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces12.TypesNamespace);
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces12.BasicNamespace);


            var response = await SendSoapData(sendRequestRequestXElement, attachments, saveLog).ConfigureAwait(false);

            return new SendRequestResponse12(response);
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
                AttachmentContentListXElement = new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentContentList");
                AttachmentHeaderListXElement = new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentHeaderList");

                foreach (FileAttachment file in attachments)
                {
                    var SignaturePKCS7 = CryptoProHelper.SignAttachment(file);
                    //AttachmentHeader
                    AttachmentHeaderListXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentHeader"
                        , new XElement(SMEVNamespaces12.BasicNamespace + "contentId", !string.IsNullOrEmpty(file.FileGuid)? file.FileGuid : file.FileName)
                        , new XElement(SMEVNamespaces12.BasicNamespace + "MimeType", MimeTypeMap.GetMimeType(file.GetExtension())),
                        new XElement(SMEVNamespaces12.BasicNamespace + "SignaturePKCS7", Convert.ToBase64String(SignaturePKCS7))));

                    //AttachmentContent
                    AttachmentContentListXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentContent"
                        , new XElement(SMEVNamespaces12.BasicNamespace + "Id", !string.IsNullOrEmpty(file.FileGuid)? file.FileGuid : file.FileName)
                        //, new XElement(SMEVNamespaces12.BasicNamespace + "Content", $"cid:{file.FileName}"))
                        , new XElement(SMEVNamespaces12.BasicNamespace + "Content", Convert.ToBase64String(file.FileData))));
                }
            }

            //-----------SenderProvidedRequestData-----------
            var senderProvidedRequestDataXElement = new XElement(SMEVNamespaces12.TypesNamespace + "SenderProvidedRequestData",
                    new XAttribute("Id", "SIGNED_BY_CONSUMER")
                    , new XElement(SMEVNamespaces12.TypesNamespace + "MessageID", GetTimeStampUuid())
                    , new XElement(SMEVNamespaces12.BasicNamespace + "MessagePrimaryContent", request)
                );

            if (AttachmentHeaderListXElement != null)
                senderProvidedRequestDataXElement.Add(AttachmentHeaderListXElement);

            if (_options.TestMode)
                senderProvidedRequestDataXElement.Add(new XElement(SMEVNamespaces12.TypesNamespace + "TestMessage"));

            //-----------SendRequestRequest-----------
            XElement sendRequestRequestXElement = new XElement(
                    SMEVNamespaces12.TypesNamespace + "SendRequestRequest"
                    , senderProvidedRequestDataXElement);

            if (AttachmentContentListXElement != null)
                sendRequestRequestXElement.Add(AttachmentContentListXElement);

            sendRequestRequestXElement.Add(new XElement(SMEVNamespaces12.TypesNamespace + "CallerInformationSystemSignature", SignXElement(senderProvidedRequestDataXElement, @"#SIGNED_BY_CONSUMER")));

            //-----------Применяем namespace-----------
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces12.TypesNamespace);
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces12.BasicNamespace);


            var response = await SendSoapData(sendRequestRequestXElement, attachments, saveLog).ConfigureAwait(false);

            return new SendRequestResponse12(response);
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
                AttachmentContentListXElement = new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentContentList");
                AttachmentHeaderListXElement = new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentHeaderList");

                foreach (FileAttachment file in attachments)
                {
                    //AttachmentHeader
                    AttachmentHeaderListXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentHeader"
                        , new XElement(SMEVNamespaces12.BasicNamespace + "contentId", file.FileName)
                        , new XElement(SMEVNamespaces12.BasicNamespace + "MimeType", MimeTypeMap.GetMimeType(file.GetExtension()))));

                    //AttachmentContent
                    AttachmentContentListXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "AttachmentContent"
                        , new XElement(SMEVNamespaces12.BasicNamespace + "Id", file.FileName)
                        //, new XElement(SMEVNamespaces12.BasicNamespace + "Content", $"cid:{file.FileName}"))
                        , new XElement(SMEVNamespaces12.BasicNamespace + "Content",
                            new XElement(SMEVNamespaces12.XopNamespace + "Include",
                                new XAttribute("href", $"cid:{file.FileName}"),
                                new XAttribute(XNamespace.Xmlns + "xop", SMEVNamespaces12.XopNamespace)))));
                }
            }

            //-----------SenderProvidedRequestData-----------
            var senderProvidedRequestDataXElement = new XElement(SMEVNamespaces12.TypesNamespace + "SenderProvidedRequestData",
                    new XAttribute("Id", "SIGNED_BY_CONSUMER")
                    , new XElement(SMEVNamespaces12.TypesNamespace + "MessageID", GetTimeStampUuid())
                    , new XElement(SMEVNamespaces12.BasicNamespace + "MessagePrimaryContent", request)
                );

            if (AttachmentHeaderListXElement != null)
                senderProvidedRequestDataXElement.Add(AttachmentHeaderListXElement);

            if (_options.TestMode)
                senderProvidedRequestDataXElement.Add(new XElement(SMEVNamespaces12.TypesNamespace + "TestMessage"));

            //-----------SendRequestRequest-----------
            XElement sendRequestRequestXElement = new XElement(
                    SMEVNamespaces12.TypesNamespace + "SendRequestRequest"
                    , senderProvidedRequestDataXElement);

            if (AttachmentContentListXElement != null)
                sendRequestRequestXElement.Add(AttachmentContentListXElement);

            sendRequestRequestXElement.Add(new XElement(SMEVNamespaces12.TypesNamespace + "CallerInformationSystemSignature", SignXElement(senderProvidedRequestDataXElement, @"#SIGNED_BY_CONSUMER")));

            //-----------Применяем namespace-----------
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces12.TypesNamespace);
            senderProvidedRequestDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces12.BasicNamespace);


            var response = await SendSoapDataSGIO(sendRequestRequestXElement, "urn:SendRequest", attachments, saveLog).ConfigureAwait(false);

            return new SendRequestResponse12(response);
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
            var messageTypeSelectorXElement = new XElement(SMEVNamespaces12.BasicNamespace + "MessageTypeSelector",
                    new XAttribute("Id", "SIGNED_BY_CALLER")
                );

            if (namespaceUri != null && rootElementLocalName != null)
            {
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "NamespaceURI", namespaceUri));
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "RootElementLocalName", rootElementLocalName));
            }

            messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "Timestamp", GetTimeStamp()));

            //-----------GetResponseRequest-----------
            XElement sendRequestRequestXElement = new XElement(SMEVNamespaces12.TypesNamespace + "GetResponseRequest",
                    messageTypeSelectorXElement,
                    new XElement(SMEVNamespaces12.TypesNamespace + "CallerInformationSystemSignature", SignXElement(messageTypeSelectorXElement, @"#SIGNED_BY_CALLER"))
                );

            //-----------Применяем namespace-----------
            sendRequestRequestXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces12.TypesNamespace);
            sendRequestRequestXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces12.BasicNamespace);

            var getResponseResponseElement = await SendSoapData(sendRequestRequestXElement, null, saveLog).ConfigureAwait(false);

            return new GetResponseResponse12(getResponseResponseElement);
        }

        /// <summary>
        /// Получает результат запроса. Если любой из параметров null, то без фильтра
        /// </summary>
        /// <param name="namespaceUri">target namespace схемы</param>
        /// <param name="rootElementLocalName">имя (local name) элемента</param>
        /// <returns>GetResponseResponse, распарсеный в класс</returns>
        public async Task<GetResponseResponse> GetResponseAsyncSGIO(string smevAction, string namespaceUri = null, string rootElementLocalName = null, bool saveLog = false)
        {
            //-----------MessageTypeSelector-----------
            var messageTypeSelectorXElement = new XElement(SMEVNamespaces12.BasicNamespace + "MessageTypeSelector",
                    new XAttribute("Id", "SIGNED_BY_CALLER")
                );

            if (namespaceUri != null && rootElementLocalName != null)
            {
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "NamespaceURI", namespaceUri));
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "RootElementLocalName", rootElementLocalName));
            }

            messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "Timestamp", GetTimeStamp()));

            //-----------GetResponseRequest-----------
            XElement sendRequestRequestXElement = new XElement(SMEVNamespaces12.SGIOTypesNamespace + "GetResponseRequest",
                    messageTypeSelectorXElement,
                    new XElement(SMEVNamespaces12.SGIOTypesNamespace + "CallerInformationSystemSignature", SignXElement(messageTypeSelectorXElement, @"#SIGNED_BY_CALLER"))
                );

            //-----------Применяем namespace-----------
            sendRequestRequestXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces12.SGIOTypesNamespace);
            sendRequestRequestXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces12.BasicNamespace);

            var getResponseResponseElement = await SendSoapDataSGIO(sendRequestRequestXElement, smevAction, null, saveLog).ConfigureAwait(false);

            return new GetResponseResponse12SGIO(getResponseResponseElement);
        }

        /// <summary>
        /// . Если любой из параметров null, то без фильтра
        /// </summary>
        /// <param name="namespaceUri">target namespace схемы</param>
        /// <param name="rootElementLocalName">имя (local name) элемента</param>
        /// <returns>GetResponseResponse, распарсеный в класс</returns>
        public async Task<GetRequestRequest> GetRequestAsyncSGIO(string smevAction, string namespaceUri = null, string rootElementLocalName = null, bool saveLog = false)
        {
            //-----------MessageTypeSelector-----------
            var messageTypeSelectorXElement = new XElement(SMEVNamespaces12.BasicNamespace + "MessageTypeSelector",
                    new XAttribute("Id", "SIGNED_BY_CALLER")
                );

            if (namespaceUri != null && rootElementLocalName != null)
            {
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "NamespaceURI", namespaceUri));
                messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "RootElementLocalName", rootElementLocalName));
            }

            messageTypeSelectorXElement.Add(new XElement(SMEVNamespaces12.BasicNamespace + "Timestamp", GetTimeStamp()));

            //-----------GetResponseRequest-----------
            XElement sendRequestRequestXElement = new XElement(SMEVNamespaces12.SGIOTypesNamespace + "GetRequestRequest",
                    messageTypeSelectorXElement,
                    new XElement(SMEVNamespaces12.SGIOTypesNamespace + "CallerInformationSystemSignature", SignXElement(messageTypeSelectorXElement, @"#SIGNED_BY_CALLER"))
                );

            //-----------Применяем namespace-----------
            sendRequestRequestXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces12.SGIOTypesNamespace);
            sendRequestRequestXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces12.BasicNamespace);

            var getResponseResponseElement = await SendSoapDataSGIO(sendRequestRequestXElement, smevAction, null, saveLog).ConfigureAwait(false);

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

        /// Подтверждает получение запроса
        /// </summary>
        /// <param name="messageId">UUID сообщения</param>
        /// <param name="accepted">принято?</param>
        public async Task SendResponceAsync(XElement responce, string messageId, string to, bool saveLog = false)
        {
            var senderProvidedResponceDataXElement = new XElement(SMEVNamespaces12.TypesNamespace + "SenderProvidedResponseData",
               new XAttribute("Id", "SIGNED_BY_CONSUMER"),
               new XElement(SMEVNamespaces12.TypesNamespace + "MessageID", messageId),
               new XElement(SMEVNamespaces12.TypesNamespace + "To", to),
               new XElement(SMEVNamespaces12.BasicNamespace + "MessagePrimaryContent", responce)
           );

            XElement sendResponseRequestXElement = new XElement(
         SMEVNamespaces12.TypesNamespace + "SendResponseRequest", senderProvidedResponceDataXElement);

            sendResponseRequestXElement.Add(new XElement(SMEVNamespaces12.TypesNamespace + "CallerInformationSystemSignature", SignXElement(senderProvidedResponceDataXElement, @"#SIGNED_BY_CONSUMER")));

            //-----------Применяем namespace-----------
            senderProvidedResponceDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns", SMEVNamespaces12.TypesNamespace);
            senderProvidedResponceDataXElement.SetAttributeValue(XNamespace.Xmlns + "ns1", SMEVNamespaces12.BasicNamespace);


            var response = await SendSoapDataSGIO(sendResponseRequestXElement, "urn:SendResponse", null, saveLog).ConfigureAwait(false);
        }

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

            await SendSoapDataSGIO(ackRequestXElement, "urn:Ack", null, saveLog).ConfigureAwait(false);
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
        private XElement SignXElementPersonalSig(XElement element, string signedNode)
        {
            return XElement.Parse(CryptoProHelper.SignPersonalSig(XMLToLinqHelper.ToXmlDocument(element), signedNode).OuterXml);
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
                          soapVersion: SoapVersion.Soap11, //это прикол смэва, не ошибка
                          bodies: new List<XElement>() { body },
                          attachments: attachments,
                          storeLog: saveLog
                          ).ConfigureAwait(false);
            }
        }

        private async Task<HTTPResponse> SendSoapDataSGIO(XElement body, string smevAction, IEnumerable<FileAttachment> attachments = null, bool saveLog = false)
        {
            using (var soapClient = new SoapClient())
            {
                return
                  await soapClient.PostAsyncSGIO(smevAction,
                          endpoint: new Uri(_options.Endpoint),
                          soapVersion: SoapVersion.Soap11, //это прикол смэва, не ошибка
                          bodies: new List<XElement>() { body },
                          attachments: attachments,
                          storeLog: saveLog
                          ).ConfigureAwait(false);
            }
        }

        #endregion
    }
}
