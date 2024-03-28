namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetAdminRespManOrg/{manOrgId},{periodId}")]
        GetAdminRespManOrgResponse GetAdminRespManOrg(string manOrgId, string periodId);
    }
}
