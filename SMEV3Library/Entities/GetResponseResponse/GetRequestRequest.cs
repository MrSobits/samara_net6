using SMEV3Library.Exceptions;
using SMEV3Library.Namespaces;
using SMEV3Library.SoapHttpClient.Enums;
using System;
using System.Xml.Linq;

namespace SMEV3Library.Entities.GetResponseResponse
{
    public class GetRequestRequest : GetResponseResponse
    {

        private XElement GetBody(XElement SoapXml)
        {
            var msfConf = new SoapMessageConfiguration(SoapVersion.Soap11);
            if (SoapXml.Name.LocalName != "Envelope")
                throw new SMEV3LibraryException($"Server return XML {SoapXml.Name.LocalName}, not Envelope");

            var body = SoapXml.Element(msfConf.Schema + "Body");

            if (body == null)
                throw new SMEV3LibraryException("Server return XML without Body or Fault tag");

            return body;
        }
        public GetRequestRequest(HTTPResponse response) : base(response)
        {
            var GetResponseResponseElement = response.SoapXML.Element(SMEVNamespaces12.TypesNamespace + "GetRequestResponse");
            FullMessageElement = response.SoapXML;
            if (GetResponseResponseElement == null)
                return;

            var ResponseElement = GetResponseResponseElement.Element(SMEVNamespaces12.TypesNamespace + "RequestMessage")?.Element(SMEVNamespaces12.TypesNamespace + "Request");
            if (ResponseElement == null)
                return;

            isAnswerPresent = true;

            //-----Response-----            
            //---OriginalMessageId---
            ReferenceMessageID = ResponseElement.Element(SMEVNamespaces12.TypesNamespace + "ReferenceMessageID")?.Value;
            ReplyTo = ResponseElement.Element(SMEVNamespaces12.TypesNamespace + "ReplyTo")?.Value;
            //---SenderProvidedResponseData---
            XElement SenderProvidedResponseDataElement = ResponseElement.Element(SMEVNamespaces12.TypesNamespace + "SenderProvidedRequestData");
            if (SenderProvidedResponseDataElement != null)
            {
                //
                MessageId = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "MessageID")?.Value;
                To = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "To")?.Value;              
                RequestRejected = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "RequestRejected");
                MessagePrimaryContent = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.BasicNamespace + "MessagePrimaryContent");
                AsyncProcessingStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "AsyncProcessingStatus");
                RequestStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "RequestStatus");
                //--AttachmentHeaderList---
                try
                {
                    var AttachmentHeaderListElemnet = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.BasicNamespace + "AttachmentHeaderList");
                    SetSignatureFileNameSGIO(AttachmentHeaderListElemnet);
                }
                catch(Exception e)
                {
                    
                }
            }
            try
            {
                //-----AttachmentContentList-----
                var AttachmentContentListElement = GetResponseResponseElement.Element(SMEVNamespaces12.TypesNamespace + "RequestMessage").Element(SMEVNamespaces12.BasicNamespace + "AttachmentContentList");
                SetFileName(AttachmentContentListElement);
             }
            catch(Exception e)
            {
                    
            }
    //-----SMEVSignature-----
    //TODO: написать проверку в сервисе
}
    }
}
