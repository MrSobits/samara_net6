using SMEV3Library.Namespaces;
using System.Xml.Linq;

namespace SMEV3Library.Entities.GetResponseResponse
{
    public class GetResponseResponse11 : GetResponseResponse
    {
        public GetResponseResponse11(HTTPResponse response) : base(response)
        {
            FullMessageElement = response.SoapXML;
            var GetResponseResponseElement = response.SoapXML?.Element(SMEVNamespaces11.TypesNamespace + "GetResponseResponse");
            if (GetResponseResponseElement == null)
                return;

            var ResponseElement = GetResponseResponseElement.Element(SMEVNamespaces11.TypesNamespace + "ResponseMessage")?.Element(SMEVNamespaces11.TypesNamespace + "Response");
            if (ResponseElement == null)
                return;

            isAnswerPresent = true;

            //-----Response-----
            //OriginalMessageId
            OriginalMessageId = ResponseElement.Element(SMEVNamespaces11.TypesNamespace + "OriginalMessageId")?.Value;
            //SenderProvidedResponseData
            XElement SenderProvidedResponseDataElement = ResponseElement.Element(SMEVNamespaces11.TypesNamespace + "SenderProvidedResponseData");
            if (SenderProvidedResponseDataElement != null)
            {
                MessageId = SenderProvidedResponseDataElement.Element(SMEVNamespaces11.TypesNamespace + "MessageID")?.Value;
                To = SenderProvidedResponseDataElement.Element(SMEVNamespaces11.TypesNamespace + "To")?.Value;
                RequestRejected = SenderProvidedResponseDataElement.Element(SMEVNamespaces11.TypesNamespace + "RequestRejected");
                MessagePrimaryContent = SenderProvidedResponseDataElement.Element(SMEVNamespaces11.BasicNamespace + "MessagePrimaryContent");
                AsyncProcessingStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "AsyncProcessingStatus");
                //--AttachmentHeaderList---
                SetSignatureFileName(SenderProvidedResponseDataElement.Element(SMEVNamespaces11.BasicNamespace + "AttachmentHeaderList"));
            }

            //-----AttachmentContentList-----
            SetFileName(GetResponseResponseElement.Element(SMEVNamespaces11.TypesNamespace + "ResponseMessage")?.Element(SMEVNamespaces11.BasicNamespace + "AttachmentContentList"));

            //-----SMEVSignature-----
            //TODO: написать проверку в сервисе

        }
    }
}
