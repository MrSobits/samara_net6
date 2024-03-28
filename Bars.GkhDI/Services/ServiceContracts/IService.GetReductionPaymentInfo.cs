namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetReductionPaymentInfo/{houseId},{periodId}")]
        GetReductionPaymentInfoResponse GetReductionPaymentInfo(string houseId, string periodId);
    }
}
