namespace Bars.GkhDi.Regions.Tatarstan.Services
{
    using System.ServiceModel;
    using CoreWCF.Web;

    using Bars.GkhDi.Services.DataContracts.GetPeriods;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetManOrgRealtyObjectInfo/{houseId}")]
        GetManOrgRealtyObjectInfoResponse GetManOrgRealtyObjectInfo(string houseId, string periodId);
    }
}