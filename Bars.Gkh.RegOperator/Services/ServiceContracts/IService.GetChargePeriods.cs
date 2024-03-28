namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;

    using Bars.Gkh.RegOperator.Services.DataContracts;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetChargePeriods")]
        GetChargePeriodsResponse GetChargePeriods();
        
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetChargePeriodsAll")]
        GetChargePeriodsResponse GetChargePeriodsAll();
    }
}