namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using Bars.GkhDi.Services.DataContracts.GetPeriods;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetManOrgRealtyObjectInfo/{houseId}/{periodId=null}")]
        GetManOrgRealtyObjectInfoResponse GetManOrgRealtyObjectInfo(string houseId, string periodId);
    }
}