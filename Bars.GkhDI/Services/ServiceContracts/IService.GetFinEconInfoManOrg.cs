namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetFinEconInfoManOrg/{manOrgId},{periodId}")]
        GetFinEconInfoManOrgResponse GetFinEconInfoManOrg(string manOrgId, string periodId);
    }
}
