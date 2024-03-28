namespace Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.GkhGji.Services.DataContracts.pmvGZHIgzhiPredLicIP;

    [ServiceContract(Namespace = "http://xsd.smev.ru/ppu/GZHIgzhiPredLicIP")]

    //, ConfigurationName="pmvGZHIgzhiPredLicIP.pmvGZHIgzhiPredLicIP")]
    public interface IPredLicIP
    {
        // CODEGEN: Generating message contract since the operation createSmvGzhiPredLicIP is neither RPC nor document wrapped.
        [OperationContract(Action = "", ReplyAction = "")]
        [XmlSerializerFormat(SupportFaults = true)]
        createSmvGzhiPredLicIPResponse1 createSmvGzhiPredLicIP(createSmvGzhiPredLicIPRequest1 request);
    }
}