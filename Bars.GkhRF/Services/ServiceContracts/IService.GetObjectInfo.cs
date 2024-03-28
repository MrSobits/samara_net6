namespace Bars.GkhRf.Services.ServiceContracts
{
    using System.ServiceModel;
    using Bars.GkhRf.Services.DataContracts.GetObjectInfo;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "ObjectInfo/{houseId}")]
        GetObjectInfoResponse GetObjectInfo(string houseId);
    }
}