namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetManContractApp/{houseId},{periodId}")]
        GetManContractAppResponse GetManContractApp(string houseId, string periodId);
    }
}
