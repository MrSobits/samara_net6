namespace Bars.GkhGji.Regions.Chelyabinsk.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.GkhGji.Services.DataContracts.pmvGZHIgzhiPereofUL;

    [ServiceContract(Namespace = "http://xsd.smev.ru/ppu/GZHIgzhiPereofUL")]
    public interface IPereofUL
    {
        // CODEGEN: Generating message contract since the operation createSmvGzhiPereofUL is neither RPC nor document wrapped.
      
        [OperationContract(Action = "", ReplyAction = "")]
        [XmlSerializerFormat(SupportFaults = true)]
        createSmvGzhiPereofULResponse1 createSmvGzhiPereofUL(createSmvGzhiPereofULRequest1 request);
    }
}