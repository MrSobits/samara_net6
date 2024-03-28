using SMEV3Library.Namespaces;
using System.Xml.Linq;

namespace SMEV3Library.Entities.SendRequestResponse
{
    public class SendRequestResponse12 : SendRequestResponse
    {
        public SendRequestResponse12(HTTPResponse response) : base(response)
        {
            var sendRequestResponseXml = response.SoapXML?.Element(SMEVNamespaces12.TypesNamespace + "SendRequestResponse");

            if (sendRequestResponseXml == null)
                return;

            XElement MessageMetadataElement = sendRequestResponseXml.Element(SMEVNamespaces12.TypesNamespace + "MessageMetadata");
            if (MessageMetadataElement == null)
                return;

            MessageId = MessageMetadataElement.Element(SMEVNamespaces12.TypesNamespace + "MessageId")?.Value;
            MessageType = MessageMetadataElement.Element(SMEVNamespaces12.TypesNamespace + "MessageType")?.Value;
            Sender = MessageMetadataElement.Element(SMEVNamespaces12.TypesNamespace + "Sender")?.Element(SMEVNamespaces12.TypesNamespace + "Mnemonic")?.Value;
            SendingTimestamp = MessageMetadataElement.Element(SMEVNamespaces12.TypesNamespace + "SendingTimestamp")?.Value;
            Recipient = MessageMetadataElement.Element(SMEVNamespaces12.TypesNamespace + "Recipient")?.Element(SMEVNamespaces12.TypesNamespace + "Mnemonic")?.Value;
            Status = MessageMetadataElement.Element(SMEVNamespaces12.TypesNamespace + "Status")?.Value;
        }
    }
}
