namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.Gkh.Services.DataContracts.GetMkdOrgInfo;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetMkdOrgInfo/{roId}")]
        GetMkdOrgInfoResponse GetMkdOrgInfo(string roId);
    }
}