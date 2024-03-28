namespace Bars.Gkh.RegOperator.Wcf.Services
{
    using System.ServiceModel;
    using Bars.Gkh.RegOperator.Services.DataContracts.SyncUnconfirmedPayments;
    using CoreWCF.Web;
    using Bars.Gkh.RegOperator.Wcf.Contracts.ExchangeDocument;
    using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;

    [ServiceContract]
    public interface IClientBankService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "ExchangeDocument")]
        ExchangeDocument ExchangeDocument();


        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "Pay?login={login}&password={password}&service={service}&accountNum={accountNum}&summ={summ}&payId={payId}", ResponseFormat = WebMessageFormat.Xml)]
        SyncUnconfirmedPaymentsPayResult Pay(string login, string password, string service, string accountNum, string summ, string payId);

 
    }
}
