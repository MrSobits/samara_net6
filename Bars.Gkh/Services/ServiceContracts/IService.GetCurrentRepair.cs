namespace Bars.Gkh.Services.ServiceContracts
{
    using System.ServiceModel;

    using Bars.Gkh.Services.DataContracts.CurrentRepair;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetCurrentRepair/{houseId}")]
        GetCurrentRepairResponse GetCurrentRepair(string houseId);
    }
}