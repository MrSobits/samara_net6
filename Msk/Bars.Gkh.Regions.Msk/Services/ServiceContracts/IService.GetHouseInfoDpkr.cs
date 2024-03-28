namespace Bars.Gkh.Regions.Msk.Services
{
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Bars.Gkh.Overhaul.Services.DataContracts;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetHouseInfoDpkr/{id}")]
        GetHouseInfoDpkrResponse GetHouseInfoDpkr(string id);
    }
}