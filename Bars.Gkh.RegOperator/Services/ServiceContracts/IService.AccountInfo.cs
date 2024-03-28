namespace Bars.Gkh.RegOperator.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;
    using Bars.Gkh.RegOperator.Services.DataContracts;
    using Bars.Gkh.RegOperator.Services.DataContracts.Accounts;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "IsAccountValid/{numLs},{lastName},{flatNum},{ownerType}")]
        bool IsAccountValid(string numLs, string lastName, string flatNum, string ownerType = "0");

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetAccountBalance/{numLs},{month},{year}")]
        AccountPeriodSummary GetAccountBalance(string numLs, string month, string year);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPersonalAccountNum/{exNum},{mId}")]
        string GetPersonalAccountNum(string exNum, string mId);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPersonalAccountNumMobile/{exNum},{address},{lastName},{firstName},{middleName}")]
        PersonalAccountMobileDTO GetPersonalAccountNumMobileQR(string exNum, string address, string lastName, string firstName, string middleName);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetListDebtors/{houseId}")]
        GetListDebtorsResponse GetListDebtorsResponse(string houseId); //

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetLastPeriodAccountBalance/{numLs}")]
        AccountPeriodSummary GetLastPeriodAccountBalance(string numLs);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetAllPeriodsAccountBalance/{lsId}")]
        AccountPeriodSummary[] GetAllPeriodsAccountBalance(long lsId);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetReportLsResponse/{numLs}")]
        GetReportLsResponse GetReportLsResponse(string numLs);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetDebtInfo/{account},{token}")]
        PersonalAccountDebtInfoResponse GetDebtInfo(string account, string token);
    }
}