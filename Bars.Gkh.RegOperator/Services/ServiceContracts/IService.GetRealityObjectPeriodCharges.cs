namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;

    using Bars.Gkh.RegOperator.Services.DataContracts;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetRealityObjectPeriodCharges/{houseId},{periodId}")]
        GetRealityObjectPeriodChargesResponse GetRealityObjectPeriodCharges(string houseId, string periodId);
    }
}