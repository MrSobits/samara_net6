namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetContractsManagement/{manOrgId},{periodId}")]
        GetContractsManagementResponse GetContractsManagement(string manOrgId, string periodId);
    }
}
