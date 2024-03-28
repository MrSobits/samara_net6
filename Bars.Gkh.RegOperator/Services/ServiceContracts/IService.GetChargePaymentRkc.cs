using Bars.Gkh.RegOperator.Services.DataContracts;

namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetChargePaymentRkc/{inn},{count},{page},{month},{year}")]
        GetChargePaymentResponse GetChargePaymentRkc(string inn, string count, string page, string month, string year);
    }
}