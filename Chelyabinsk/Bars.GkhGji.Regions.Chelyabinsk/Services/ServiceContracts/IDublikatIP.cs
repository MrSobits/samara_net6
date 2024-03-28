namespace Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.GkhGji.Services.DataContracts.pmvGZHIgzhiDublikatIP;

    [ServiceContract(Namespace = "http://xsd.smev.ru/ppu/GZHIgzhiDublikatIP")]
    public interface IDublikatIP
    {
        // CODEGEN: Generating message contract since the operation createSmvGzhiDublikatIP is neither RPC nor document wrapped.
        [OperationContract(Action = "", ReplyAction = "")]
        [XmlSerializerFormat(SupportFaults = true)]
        createSmvGzhiDublikatIPResponse1 createSmvGzhiDublikatIP(createSmvGzhiDublikatIPRequest1 request);
    }
}