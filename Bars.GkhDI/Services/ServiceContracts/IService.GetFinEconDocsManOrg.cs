namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetFinEconDocsManOrg/{manOrgId},{periodId}")]
        GetFinEconDocsManOrgResponse GetFinEconDocsManOrg(string manOrgId, string periodId);
    }
}
