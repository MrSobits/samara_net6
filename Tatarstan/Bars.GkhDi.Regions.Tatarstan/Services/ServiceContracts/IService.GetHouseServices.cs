namespace Bars.GkhDi.Regions.Tatarstan.Services
{
    using System.ServiceModel;
    using CoreWCF.Web;

    using Bars.GkhDi.Services;
    
    
    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHouseServices/{houseId},{periodId}")]
        GetHouseServicesResponse GetHouseServices(string houseId, string periodId);
    }
}
