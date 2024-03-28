namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;
    using Bars.Gkh.Services.DataContracts.GetHousesInfoByEditDate;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHousesInfoByEditDate/{date}")]
        GetHousesInfoByEditDateResponse GetHousesInfoByEditDate(string date);
    }
}