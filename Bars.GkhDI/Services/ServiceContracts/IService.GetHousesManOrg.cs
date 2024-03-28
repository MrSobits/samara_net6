namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using Bars.GkhDi.Services.DataContracts.HousesManOrg;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesManOrg/{moId},{periodId}")]
        GetHousesManOrgResponse GetHousesManOrg(string moId, string periodId);
    }
}