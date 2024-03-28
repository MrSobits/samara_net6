namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetGenMeetProtocols/{houseId},{periodId}")]
        GetGenMeetProtocolsResponse GetGenMeetProtocols(string houseId, string periodId);
    }
}
