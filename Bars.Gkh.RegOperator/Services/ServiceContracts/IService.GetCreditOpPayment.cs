namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;

    using Bars.Gkh.RegOperator.Services.DataContracts.GetCreditOpPayment;

    public partial interface IService
    {
        /// <summary>
        /// Сведения об оплате КР
        /// </summary>
        /// <param name="roId"></param>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetCreditOpPayment/{roId}")]
        CreditOpPaymentResponse GetCreditOpPayment(string roId);
    }
}
