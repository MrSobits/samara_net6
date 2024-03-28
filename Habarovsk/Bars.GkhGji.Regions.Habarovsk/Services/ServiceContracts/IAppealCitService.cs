using System.ServiceModel;

namespace Bars.GkhGji.Regions.Habarovsk.Services.ServiceContracts
{
    using Bars.GkhGji.Regions.Habarovsk.Services.DataContracts.SyncAppealCit;

    /// <summary>
    /// Сервис обмена данными с АС ДОУ
    /// </summary>
    [ServiceContract]
    public interface IAppealCitService
    {
        /// <summary>
        /// Получение обращений граждан
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        AppealCitResult GetAppealCit(AppealCitRecord appealCitRecord, string token);

        [OperationContract]
        [XmlSerializerFormat]
        AppealCitPortalResult ImportPortalAppeal(PortalAppeal appealCitRecord, string token);

        [OperationContract]
        [XmlSerializerFormat]
        ReportResult SetReportState(ReportState reportState, string token);

        [OperationContract]
        [XmlSerializerFormat]
        AppealCitStateResult GetState(string Id, string token);

        [OperationContract]
        [XmlSerializerFormat]
        AppealCitResult SendOSSPApproveRequest(OSSPApproveRequest OSSPApproveRequest);

        [OperationContract]
        [XmlSerializerFormat]
        SendOSSPApproveResult GetOSSProotocols(string requestId);

        [OperationContract]
        [XmlSerializerFormat]
        GetProtocolTypesResult GetProtocolTypes();

        [OperationContract]
        [XmlSerializerFormat]
        OSSRequestStateResponce GetOSSRequestsState(string erknmid);

        [OperationContract]
        [XmlSerializerFormat]
        OSSRequestHistoryResponce GetOSSRequestHistory(string requestId);
    }
}
