namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using Bars.Gkh.RegOperator.Services.DataContracts.SyncUnconfirmedPayments;
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "Check/{login},{password},{service},{accountNum},{summ}")]
        SyncUnconfirmedPaymentsCheckResult Check(string login, string password, string service, string accountNum, string summ);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "Pay?login={login}&password={password}&service={service}&accountNum={accountNum}&summ={summ}&payId={payId}",ResponseFormat = WebMessageFormat.Xml)]
        SyncUnconfirmedPaymentsPayResult Pay(string login, string password, string service, string accountNum, string summ, string payId);
    }
}