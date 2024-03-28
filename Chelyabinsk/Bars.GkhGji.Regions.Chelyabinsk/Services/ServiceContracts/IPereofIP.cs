namespace Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.GkhGji.Services.DataContracts.pmvGZHIgzhiPereofIP;

    [ServiceContract(Namespace = "http://xsd.smev.ru/ppu/GZHIgzhiPereofIPL")]
    public interface IPereofIP
    {
        // CODEGEN: Generating message contract since the operation createSmvGzhiPereofIP is neither RPC nor document wrapped.
        [OperationContract(Action = "", ReplyAction = "")]
        [XmlSerializerFormat(SupportFaults = true)]
        createSmvGzhiPereofIPResponse1 createSmvGzhiPereofIP(createSmvGzhiPereofIPRequest1 request);
    }
}