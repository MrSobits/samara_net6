using SMEV3Library.Namespaces;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SMEV3Library.Entities.GetResponseResponse
{
    public class GetResponseResponse12 : GetResponseResponse
    {
        public GetResponseResponse12(HTTPResponse response) : base(response)
        {
            FullMessageElement = response.SoapXML;
            
            var GetResponseResponseElement = response.SoapXML.Element(SMEVNamespaces12.TypesNamespace + "GetResponseResponse");
            if (GetResponseResponseElement == null)
                return;

            var ResponseElement = GetResponseResponseElement.Element(SMEVNamespaces12.TypesNamespace + "ResponseMessage")?.Element(SMEVNamespaces12.TypesNamespace + "Response");
            if (ResponseElement == null)
                return;
            FSAttachmentsList = GetFSAttachments(ResponseElement);
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
                RequestRejected = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "RequestRejected");
                MessagePrimaryContent = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.BasicNamespace + "MessagePrimaryContent");
                AsyncProcessingStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "AsyncProcessingStatus");
                RequestStatus = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.TypesNamespace + "RequestStatus");
                //--AttachmentHeaderList---
                var AttachmentHeaderListElemnet = SenderProvidedResponseDataElement.Element(SMEVNamespaces12.BasicNamespace + "AttachmentHeaderList");
                try
                {
                    SetSignatureFileName(AttachmentHeaderListElemnet);
                }
                catch
                {
                    SetSignatureFileName(null);
                }
            }

            //-----AttachmentContentList-----
            var AttachmentContentListElement = GetResponseResponseElement.Element(SMEVNamespaces12.TypesNamespace + "ResponseMessage").Element(SMEVNamespaces12.BasicNamespace + "AttachmentContentList");
            try
            {
                SetFileName(AttachmentContentListElement);
            }
            catch
            { }

            //-----SMEVSignature-----
            //TODO: написать проверку в сервисе
        }

        private FsAttachmentProxy[] GetFSAttachments(XElement element)
        {
            List<FsAttachmentProxy> attList = new List<FsAttachmentProxy>();
            var attListElement = element.Element(SMEVNamespaces12.BasicNamespace + "FSAttachmentsList");
            if (attListElement == null)
            {
                return attList.ToArray();
            }
            foreach (var elem in attListElement.Elements())
            {
                attList.Add(new FsAttachmentProxy
                {
                    FileName = elem.Element(SMEVNamespaces12.BasicNamespace + "FileName").Value,
                    Password = elem.Element(SMEVNamespaces12.BasicNamespace + "Password").Value,
                    UserName = elem.Element(SMEVNamespaces12.BasicNamespace + "UserName").Value,
                    uuid = elem.Element(SMEVNamespaces12.BasicNamespace + "uuid").Value
                });
            }
            return attList.ToArray();
        }
    }
}
