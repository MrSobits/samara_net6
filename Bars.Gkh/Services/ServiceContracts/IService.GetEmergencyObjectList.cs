namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.Gkh.Services.DataContracts.GetEmergencyObjectList;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetEmergencyObjectList/{Id}")]
        GetEmergencyObjectListResponse GetEmergencyObjectList(string Id);
    }
}