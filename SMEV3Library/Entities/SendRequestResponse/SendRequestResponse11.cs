using SMEV3Library.Namespaces;
using System.Xml.Linq;

namespace SMEV3Library.Entities.SendRequestResponse
{
    public class SendRequestResponse11 : SendRequestResponse
    {
        public SendRequestResponse11(HTTPResponse response) : base(response)
        {
            var sendRequestResponseXml = response.SoapXML?.Element(SMEVNamespaces11.TypesNamespace + "SendRequestResponse");

            if (sendRequestResponseXml == null)
                return;

            XElement MessageMetadataElement = sendRequestResponseXml.Element(SMEVNamespaces11.TypesNamespace + "MessageMetadata");
            if (MessageMetadataElement == null)
                return;

            MessageId = MessageMetadataElement.Element(SMEVNamespaces11.TypesNamespace + "MessageId")?.Value;
            MessageType = MessageMetadataElement.Element(SMEVNamespaces11.TypesNamespace + "MessageType")?.Value;
            Sender = MessageMetadataElement.Element(SMEVNamespaces11.TypesNamespace + "Sender")?.Element(SMEVNamespaces11.TypesNamespace + "Mnemonic")?.Value;
            SendingTimestamp = MessageMetadataElement.Element(SMEVNamespaces11.TypesNamespace + "SendingTimestamp")?.Value;
            Recipient = MessageMetadataElement.Element(SMEVNamespaces11.TypesNamespace + "Recipient")?.Element(SMEVNamespaces11.TypesNamespace + "Mnemonic")?.Value;
            Status = MessageMetadataElement.Element(SMEVNamespaces11.TypesNamespace + "Status")?.Value;
        }
    }
}
