namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;
    using Bars.Gkh.RegOperator.Services.DataContracts.GetChargePeriods;

    public partial interface IService
    {

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPaymentDocument/{accountNumber},{periodId},{saveSnapshot}")]
        PaymentDocumentResponse GetPaymentDocument(string accountNumber, string periodId, bool saveSnapshot);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetSberDocument/{accId},{periodId},{token}")]
        PaymentDocumentResponse GetSberDocument(string accId, string periodId, string token);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPayDocCheckAddress/{accountNumber},{periodId},{address},{saveSnapshot}")]
        PaymentDocumentResponse GetPayDocCheckAddress(string accountNumber, string periodId, string address, bool saveSnapshot);
    }
}
