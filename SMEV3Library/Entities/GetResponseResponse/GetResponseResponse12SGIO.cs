using SMEV3Library.Namespaces;
using System.Xml.Linq;

namespace SMEV3Library.Entities.GetResponseResponse
{
    public class GetResponseResponse12SGIO : GetResponseResponse
    {
        public GetResponseResponse12SGIO(HTTPResponse response) : base(response)
        {
            var GetResponseResponseElement = response.SoapXML.Element(SMEVNamespaces12.TypesNamespace + "GetResponseResponse");
            if (GetResponseResponseElement == null)
                return;

            var ResponseElement = GetResponseResponseElement.Element(SMEVNamespaces12.TypesNamespace + "ResponseMessage")?.Element(SMEVNamespaces12.TypesNamespace + "Response");
            if (ResponseElement == null)
                return;

            isAnswerPresent = true;

            //-----Response-----            
            //---OriginalMessageId---
            OriginalMessageId = ResponseElement.Element(SMEVNamespaces12.TypesNamespace + "OriginalMessageId")?.Value;
            //---SenderProvidedResponseData---
            XElement SenderProvidedResponseDataElement = ResponseElement.Element(SMEVNamespaces12.TypesNamespace + "SenderProvidedResponseData");
            if (SenderProvidedResponseDataElement != null)
            {
                //
                MessageId = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "MessageID")?.Value;
                To = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "To")?.Value;
                ReplyTo = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "ReplyTo")?.Value;
                RequestRejected = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "RequestRejected");
                MessagePrimaryContent = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.BasicNamespace + "MessagePrimaryContent");
                AsyncProcessingStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "AsyncProcessingStatus");
                RequestStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "RequestStatus");
                //--AttachmentHeaderList---
                var AttachmentHeaderListElemnet = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.BasicNamespace + "AttachmentHeaderList");
                try
                {
                    SetSignatureFileNameSGIO(AttachmentHeaderListElemnet);
                }
                catch
                {
                    SetSignatureFileName(null);
                }
            }

            //-----AttachmentContentList-----
            var AttachmentContentListElement = GetResponseResponseElement.Element(SMEVNamespaces12.TypesNamespace + "ResponseMessage").Element(SMEVNamespaces12.BasicNamespace + "AttachmentContentList");
            SetFileNameSGIO(AttachmentContentListElement);
            response.Attachments = Attachments;

            //-----SMEVSignature-----
            //TODO: написать проверку в сервисе
        }
    }
}
