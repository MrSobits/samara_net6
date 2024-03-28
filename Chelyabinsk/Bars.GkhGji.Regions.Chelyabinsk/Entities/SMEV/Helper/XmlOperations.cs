namespace Bars.GkhGji.Regions.Chelyabinsk.Entities.SMEV
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Cryptography.Xml;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using CryptoPro.Sharpei.Xml;
    using SmevRef;
    using System.Xml.Serialization;
    using System.IO;
    using Proxy;
    using System.Diagnostics;

    public static class XmlOperations
    {
        public static XmlDocument CreateXmlDocReqForSign(XmlElement element, out string messageId)
        {
            DateTime dateTime = DateTime.Now;
            messageId = GuidGenerator.GenerateTimeBasedGuid(dateTime).ToString();

            SenderProvidedRequestData provReqData = new SenderProvidedRequestData();
            provReqData.Id = "SIGNED_BY_CONSUMER";
            provReqData.MessageID = messageId;
            provReqData.MessagePrimaryContent = element;
            //provReqData.TestMessage = new SmevRef.Void();

            XmlSerializer sprdSerializer = new XmlSerializer(typeof(SenderProvidedRequestData));
            MemoryStream streamElementSend = new MemoryStream();

            XmlTextWriter xmltw = new XmlTextWriter(streamElementSend, new UTF8Encoding(false));
            sprdSerializer.Serialize(xmltw, provReqData);
            streamElementSend.Position = 0;

            XmlDocument docForSign = new XmlDocument();
            docForSign.PreserveWhitespace = true;
            docForSign.Load(streamElementSend);

            return docForSign;
        }

        public static XmlDocument CreateXmlDocReqForSign(XmlElement element, string messageId, bool withAtach)
        {
            SenderProvidedRequestData provReqData = new SenderProvidedRequestData();
            provReqData.Id = "SIGNED_BY_CONSUMER";
            provReqData.MessageID = messageId;
            provReqData.MessagePrimaryContent = element;
            provReqData.TestMessage = new SmevRef.Void();

            //Если есть вложения
            if (withAtach)
            {
                List<AttachmentHeaderType> atachments = new List<AttachmentHeaderType>();

                AttachmentHeaderType attachHeader = new AttachmentHeaderType();
                attachHeader.MimeType = "application/zip";
                attachHeader.contentId = "Id_Attach";

                atachments.Add(attachHeader);
                provReqData.AttachmentHeaderList = atachments.ToArray();
            }

            XmlSerializer sprdSerializer = new XmlSerializer(typeof(SenderProvidedRequestData));
            MemoryStream streamElementSend = new MemoryStream();

            XmlTextWriter xmltw = new XmlTextWriter(streamElementSend, new UTF8Encoding(false));
            sprdSerializer.Serialize(xmltw, provReqData);
            streamElementSend.Position = 0;

            XmlDocument docForSign = new XmlDocument();
            docForSign.PreserveWhitespace = true;
            docForSign.Load(streamElementSend);

            return docForSign;
        }

        public static XmlDocument CreateXmlDocRespForSign(String localName, String uri)
        {
            MessageTypeSelector messageTypeSelector = new MessageTypeSelector();
            messageTypeSelector.Id = "SIGNED_BY_CONSUMER";
            // messageTypeSelector.Timestamp = dateTime;
            messageTypeSelector.Timestamp = DateTime.Now;
            messageTypeSelector.RootElementLocalName = localName;
            messageTypeSelector.NamespaceURI = uri;

            XmlSerializer mtsSerializer = new XmlSerializer(typeof(MessageTypeSelector));
            var xns = new XmlSerializerNamespaces();
            MemoryStream streamResp = new MemoryStream();

            XmlTextWriter xmltw = new XmlTextWriter(streamResp, new UTF8Encoding(false));
            mtsSerializer.Serialize(xmltw, messageTypeSelector);
            streamResp.Position = 0;

            XmlDocument docForSign = new XmlDocument();
            docForSign.PreserveWhitespace = true;
            docForSign.Load(streamResp);

            return docForSign;
        }

        public static XmlDocument CreateXmlDocAckForSign(String messageId)
        {
            AckTargetMessage ackTargetMessage = new AckTargetMessage();
            ackTargetMessage.accepted = true;
            ackTargetMessage.Id = "SIGNED_BY_CONSUMER";
            ackTargetMessage.Value = messageId;

            XmlSerializer atmSerializer = new XmlSerializer(typeof(AckTargetMessage));
            MemoryStream streamAck = new MemoryStream();

            XmlTextWriter xmltw = new XmlTextWriter(streamAck, new UTF8Encoding(false));
            atmSerializer.Serialize(xmltw, ackTargetMessage);
            streamAck.Position = 0;

            XmlDocument docForSign = new XmlDocument();
            docForSign.PreserveWhitespace = true;
            docForSign.Load(streamAck);

            return docForSign;
        }

        public static XmlDocument CreateSendRequest(XmlDocument docForSign, XmlElement sign, Boolean withAttach = false)
        {
            XmlDocument requestRequest = new XmlDocument();
            requestRequest.PreserveWhitespace = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("<S:Envelope xmlns:S=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ns=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1\">");
            sb.Append("<S:Body>");
            sb.Append("<ns2:SendRequestRequest xmlns:ns3=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/faults/1.1\" xmlns:ns2=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1\" xmlns=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1\">");
            sb.Append("</ns2:SendRequestRequest>");
            sb.Append("</S:Body>");
            sb.Append("</S:Envelope>");
            requestRequest.LoadXml(sb.ToString());

            requestRequest.GetElementsByTagName("ns2:SendRequestRequest")[0].AppendChild(
                requestRequest.ImportNode(docForSign.GetElementsByTagName("SenderProvidedRequestData").Cast<XmlElement>().FirstOrDefault(), true));

            requestRequest.GetElementsByTagName("ns2:SendRequestRequest")[0].InnerXml =
                requestRequest.GetElementsByTagName("ns2:SendRequestRequest")[0].InnerXml +
                    "<ns:CallerInformationSystemSignature></ns:CallerInformationSystemSignature>";

            requestRequest.GetElementsByTagName("ns:CallerInformationSystemSignature")[0].AppendChild(
                requestRequest.ImportNode(sign, true));

            if (withAttach)
            {
                AttachmentContentType attachment = new AttachmentContentType();
                attachment.Id = "Id_Attach";
                byte[] dataAtachment = File.ReadAllBytes(@"C:\FileStore\Attach\Request.zip");
                attachment.Content = dataAtachment;

                XmlSerializer serializerattach = new XmlSerializer(typeof(AttachmentContentType));
                // XmlDocument nodeDoc = new XmlDocument();

                using (XmlTextWriter xmltw = new XmlTextWriter(@"C:\FileStore\atachment.xml",
                   new UTF8Encoding(false)))
                {
                    serializerattach.Serialize(xmltw, attachment);
                }
                XmlDocument attachmentDoc = new XmlDocument();
                attachmentDoc.PreserveWhitespace = true;
                attachmentDoc.Load(@"C:\FileStore\atachment.xml");

                XmlElement attachElement = attachmentDoc.GetElementsByTagName("AttachmentContent").Cast<XmlElement>().FirstOrDefault();

                requestRequest.GetElementsByTagName("ns2:SendRequestRequest")[0].InnerXml =
                requestRequest.GetElementsByTagName("ns2:SendRequestRequest")[0].InnerXml +
                    "<AttachmentContentList></AttachmentContentList>";

                requestRequest.GetElementsByTagName("AttachmentContentList")[0].AppendChild(
                    requestRequest.ImportNode(attachElement, true));
            }


            return requestRequest;
        }

        public static XmlDocument CreateGetResponse(XmlDocument docForSign, XmlElement sign)
        {
            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.PreserveWhitespace = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ns=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1\" xmlns:ns1=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1\">");
            sb.Append("<soapenv:Header/>");
            sb.Append("<soapenv:Body>");
            sb.Append("<ns:GetResponseRequest>");
            sb.Append("</ns:GetResponseRequest>");
            sb.Append("</soapenv:Body>");
            sb.Append("</soapenv:Envelope>");
            xmlResponse.LoadXml(sb.ToString());

            xmlResponse.GetElementsByTagName("ns:GetResponseRequest")[0].AppendChild(
                xmlResponse.ImportNode(docForSign.GetElementsByTagName("MessageTypeSelector").Cast<XmlElement>().FirstOrDefault(), true));

            xmlResponse.GetElementsByTagName("ns:GetResponseRequest")[0].InnerXml =
                xmlResponse.GetElementsByTagName("ns:GetResponseRequest")[0].InnerXml +
                    "<ns:CallerInformationSystemSignature></ns:CallerInformationSystemSignature>";

            xmlResponse.GetElementsByTagName("ns:CallerInformationSystemSignature")[0].AppendChild(
                xmlResponse.ImportNode(sign, true));

            return xmlResponse;
        }

        public static XmlDocument CreateAck(XmlDocument docForSign, XmlElement sign)
        {
            XmlDocument xmlAck = new XmlDocument();
            xmlAck.PreserveWhitespace = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("<soapenv:Envelope xmlns:ns1=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/basic/1.1\" xmlns:ns=\"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.1\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.Append("<soapenv:Header/>");
            sb.Append("<soapenv:Body>");
            sb.Append("<ns:AckRequest>");
            sb.Append("</ns:AckRequest>");
            sb.Append("</soapenv:Body>");
            sb.Append("</soapenv:Envelope>");
            xmlAck.LoadXml(sb.ToString());

            xmlAck.GetElementsByTagName("ns:AckRequest")[0].AppendChild(
                xmlAck.ImportNode(docForSign.GetElementsByTagName("AckTargetMessage").Cast<XmlElement>().FirstOrDefault(), true));

            xmlAck.GetElementsByTagName("ns:AckRequest")[0].InnerXml =
                xmlAck.GetElementsByTagName("ns:AckRequest")[0].InnerXml +
                    "<ns:CallerInformationSystemSignature></ns:CallerInformationSystemSignature>";

            xmlAck.GetElementsByTagName("ns:CallerInformationSystemSignature")[0].AppendChild(
                xmlAck.ImportNode(sign, true));

            return xmlAck;
        }

        public static XmlDocument CreateSendSisGmp(XmlDocument doc, XmlElement sign, X509Certificate2 certGmp, String tagFogSign)
        {
            XmlDocument docForSign = new XmlDocument();
            docForSign.PreserveWhitespace = true;
            docForSign.LoadXml(doc.InnerXml);

            docForSign.GetElementsByTagName(tagFogSign)[0].AppendChild(
                docForSign.ImportNode(sign, true));

            //gmpDocumentBase.Save(@"C:\Temp\GisGmp\TestGmp.xml");

            XmlDocument requestRequest = SmevSign.SignXmlFile(docForSign, certGmp);

            return requestRequest;
        }

        public static void CreateAttachments(SMEVEGRN entity, String fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<request xmlns=\"http://rosreestr.ru/services/v0.12/TRequest\">");
            sb.Append("<statementFile>");
            sb.Append($"<fileName>{fileName}.xml</fileName>");
            //sb.Append($"<fileName>40e5d060-e9e9-4d8d-b5be-bce49d6dfbdb.xml</fileName>");
            sb.Append("</statementFile>");
            sb.Append("<file>");
            sb.Append("<fileName>request.xml</fileName>");
            sb.Append("</file>");
            sb.Append("<requestType>111300003000</requestType>");
            sb.Append("</request>");
            XmlDocument requestXml = new XmlDocument();
            requestXml.LoadXml(sb.ToString());
            StringWriter elementSW = new StringWriter();
            XmlTextWriter elementTW = new XmlTextWriter(elementSW);
            requestXml.WriteTo(elementTW);
            var element = elementSW.ToString();
            StreamWriter streamWriterElement = new StreamWriter(@"C:\FileStore\Attach\request.xml");
            streamWriterElement.WriteLine(element);
            streamWriterElement.Close();

            sb = new StringBuilder();
            sb.Append($"<ns4:EGRNRequest _id=\"MFC-{entity.Id}\" xmlns:ns7=\"http://rosreestr.ru/services/v0.1/commons/TObject\" xmlns:ns6=\"http://rosreestr.ru/services/v0.1/commons/Address\" xmlns:ns5=\"http://rosreestr.ru/services/v0.1/commons/Subjects\" xmlns:ns4=\"http://rosreestr.ru/services/v0.18/TStatementRequestEGRN\" xmlns:ns3=\"http://rosreestr.ru/services/v0.1/commons/Commons\" xmlns:ns2=\"http://rosreestr.ru/services/v0.1/commons/Documents\" xmlns=\"http://rosreestr.ru/services/v0.1/TStatementCommons\">");
            sb.Append("</ns4:EGRNRequest>");
            XmlDocument resultXML = new XmlDocument();
            resultXML.LoadXml(sb.ToString());

            XmlDocument headerBlock = new XmlDocument();

            headerBlock.Load(@"C:\FileStore\Attach\40e5d060-e9e9-4d8d-b5be-bce49d6dfbdb.xml");

            XmlElement headerElement = headerBlock.GetElementsByTagName("ns4:header").Cast<XmlElement>().FirstOrDefault();

            resultXML.GetElementsByTagName("ns4:EGRNRequest")[0].AppendChild(
                resultXML.ImportNode(headerElement, true));

            //добавляем блок declarant
            Declarant declarant = new Declarant();
            declarant.Id = Guid.Parse("36521ac0-8eec-4b8c-b7d8-b963eae7e81e");
            Person person = new Person();
            person.Name = entity.PersonName;
            person.Surname = entity.PersonSurname;
            person.Type = "ns5:TEGRNRequestPerson";

            IdDocumentRef idDocumnetRef = new IdDocumentRef();
            idDocumnetRef.DocumentId = Guid.Parse("36521ac0-8eec-4b8c-b7d8-b963eae7e81e");

            person.IdDocumentRef = idDocumnetRef;
            declarant.DeclarantKind = entity.EGRNApplicantType.Code;
            declarant.Person = person;


            XmlSerializer serializerDeclarant = new XmlSerializer(typeof(Declarant));

            using (XmlTextWriter xmltw = new XmlTextWriter(@"C:\FileStore\Attach\Declarant.xml",
               new UTF8Encoding(false)))
            {
                serializerDeclarant.Serialize(xmltw, declarant);
            }

            XmlDocument declarantBlock = new XmlDocument();

            declarantBlock.Load(@"C:\FileStore\Attach\Declarant.xml");

            XmlElement declarantElement = declarantBlock.GetElementsByTagName("declarant").Cast<XmlElement>().FirstOrDefault();

            resultXML.GetElementsByTagName("ns4:EGRNRequest")[0].AppendChild(
                resultXML.ImportNode(declarantElement, true));

            //добавляем блок requestDetails
            sb = new StringBuilder();
            sb.Append("<ns4:root xmlns:ns4=\"http://rosreestr.ru/services/v0.18/TStatementRequestEGRN\" xmlns:ns7=\"http://rosreestr.ru/services/v0.1/commons/TObject\">");
            sb.Append("<ns4:requestDetails>");
            sb.Append("<ns4:requestEGRNAccessAction>");
            sb.Append("<ns4:requiredDataType>");
            sb.Append($"<ns7:dataType>{entity.RequestType.ToString()}</ns7:dataType>");
            //sb1.Append($"<ns7:objectCadastralNumber>{egrn.CadastralNumber}</ns7:objectCadastralNumber>");
            sb.Append("</ns4:requiredDataType>");
            sb.Append("</ns4:requestEGRNAccessAction>");
            sb.Append("</ns4:requestDetails>");
            sb.Append("</ns4:root>");
            XmlDocument requestDetails = new XmlDocument();
            requestDetails.LoadXml(sb.ToString());

            XmlElement requestDetailsElement = requestDetails.GetElementsByTagName("ns4:requestDetails").Cast<XmlElement>().FirstOrDefault();

            resultXML.GetElementsByTagName("ns4:EGRNRequest")[0].AppendChild(
                resultXML.ImportNode(requestDetailsElement, true));


            //добавляем блок deliveryDetails
            sb = new StringBuilder();
            sb.Append("<ns4:root xmlns:ns4=\"http://rosreestr.ru/services/v0.18/TStatementRequestEGRN\">");
            sb.Append("<ns4:deliveryDetails>");
            sb.Append("<ns4:resultDeliveryMethod>");
            sb.Append("<ns4:recieveResultTypeCode>webService</ns4:recieveResultTypeCode>");
            sb.Append("</ns4:resultDeliveryMethod>");
            sb.Append("</ns4:deliveryDetails>");
            sb.Append("</ns4:root>");
            XmlDocument deliveryDetails = new XmlDocument();
            deliveryDetails.LoadXml(sb.ToString());

            XmlElement deliveryDetailsElement = deliveryDetails.GetElementsByTagName("ns4:deliveryDetails").Cast<XmlElement>().FirstOrDefault();

            resultXML.GetElementsByTagName("ns4:EGRNRequest")[0].AppendChild(
                resultXML.ImportNode(deliveryDetailsElement, true));


            //добавляем блок statementAgreements
            sb = new StringBuilder();
            sb.Append("<ns4:root xmlns:ns4=\"http://rosreestr.ru/services/v0.18/TStatementRequestEGRN\">");
            sb.Append("<ns4:statementAgreements>");
            sb.Append("<ns4:persDataProcessingAgreement>01</ns4:persDataProcessingAgreement>");
            sb.Append("<ns4:actualDataAgreement>04</ns4:actualDataAgreement>");
            sb.Append("</ns4:statementAgreements>");
            sb.Append("</ns4:root>");
            XmlDocument statementAgreements = new XmlDocument();
            statementAgreements.LoadXml(sb.ToString());

            XmlElement statementAgreementsElement = statementAgreements.GetElementsByTagName("ns4:statementAgreements").Cast<XmlElement>().FirstOrDefault();

            resultXML.GetElementsByTagName("ns4:EGRNRequest")[0].AppendChild(
                resultXML.ImportNode(statementAgreementsElement, true));

            //Сохраняем
            elementSW = new StringWriter();
            elementTW = new XmlTextWriter(elementSW);
            resultXML.WriteTo(elementTW);
            element = elementSW.ToString();
            streamWriterElement = new StreamWriter($@"C:\FileStore\Attach\{fileName}.xml");
            streamWriterElement.WriteLine(element);
            streamWriterElement.Close();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"C:\FileStore\7-Zip\7z.exe";
            string targetCompressName = @"C:\FileStore\Attach\Request.zip";
            string filetozip = null;
            filetozip = "\"" + $@"C:\FileStore\Attach\{fileName}.xml" + " " + "\"" + $@"C:\FileStore\Attach\request.xml" + " ";
            startInfo.Arguments = "a -tzip \"" + targetCompressName + "\" \"" + filetozip + "\" -mx=9";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process x = Process.Start(startInfo);
            x.WaitForExit();
        }
    }
}
