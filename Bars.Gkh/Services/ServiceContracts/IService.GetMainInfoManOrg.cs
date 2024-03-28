namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.Gkh.Services.DataContracts.GetMainInfoManOrg;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetMainInfoManOrg/{moId}")]
        GetMainInfoManOrgResponse GetMainInfoManOrg(string moId);
    }
}