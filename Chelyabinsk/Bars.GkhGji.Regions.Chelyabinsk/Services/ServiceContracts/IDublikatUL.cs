namespace Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.GkhGji.Services.DataContracts.pmvGZHIgzhiDublikatUL;

    [ServiceContract(Namespace = "http://xsd.smev.ru/ppu/GZHIgzhiDublikatUL")]
    public interface IDublikatUL
    {
        // CODEGEN: Generating message contract since the operation createSmvGzhiDublikatUL is neither RPC nor document wrapped.
        [OperationContract(Action = "", ReplyAction = "")]
        [XmlSerializerFormat(SupportFaults = true)]
        createSmvGzhiDublikatULResponse1 createSmvGzhiDublikatUL(createSmvGzhiDublikatULRequest1 request);
    }
}