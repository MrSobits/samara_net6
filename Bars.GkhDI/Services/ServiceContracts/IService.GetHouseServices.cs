namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHouseServices/{houseId},{periodId}")]
        GetHouseServicesResponse GetHouseServices(string houseId, string periodId);
    }
}
