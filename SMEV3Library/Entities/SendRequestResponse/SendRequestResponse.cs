using SMEV3Library.Namespaces;

namespace SMEV3Library.Entities.SendRequestResponse
{
    /// <summary>
    /// Ответ на SendRequestRequest
    /// </summary>
    public abstract class SendRequestResponse : SMEVResponse
    {
        public SendRequestResponse(HTTPResponse response)
        {
            Error = response.Error;
            SendedData = response.sendedData;
            ReceivedData = response.receivedData;
            FaultXML = response.SoapXML?.Element(SoapNamespaces.SoapNamespace + "Fault");
        }
    }
}
