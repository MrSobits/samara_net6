namespace Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.GkhGji.Services.DataContracts.pmvGZHIgzhiPredLicUL;

    [ServiceContract(Namespace = "http://xsd.smev.ru/ppu/GZHIgzhiPredLicUL")]

    //[ServiceContract]
    public interface IPredLicUL
    {
        // CODEGEN: Generating message contract since the operation createSmvGzhiPredLicUL is neither RPC nor document wrapped.
        [OperationContract(Action = "", ReplyAction = "")]
        [XmlSerializerFormat(SupportFaults = true)]
        createSmvGzhiPredLicULResponse1 createSmvGzhiPredLicUL(createSmvGzhiPredLicULRequest1 request);
    }
}