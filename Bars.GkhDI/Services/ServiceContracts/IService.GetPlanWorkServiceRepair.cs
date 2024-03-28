namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPlanWorkServiceRepair/{houseId},{periodId}")]
        GetPlanWorkServiceRepairResponse GetPlanWorkServiceRepair(string houseId, string periodId);
    }
}
