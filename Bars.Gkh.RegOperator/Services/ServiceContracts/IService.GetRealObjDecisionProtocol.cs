using Bars.Gkh.RegOperator.Services.DataContracts;

namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetRealObjDecisionProtocol/{roId}")]
        GetRealObjDecisionProtocolResponse GetRealObjDecisionProtocol(string roId);
    }
}