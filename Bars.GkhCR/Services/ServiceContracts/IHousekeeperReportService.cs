using System.ServiceModel;

namespace Bars.GkhCr.Services.ServiceContracts
{
    using CoreWCF.Web;

    public partial interface IService
    {
        /// <summary>
        /// Запрос на импорт обращений
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        InsertHousekeeperReportResponce InsertHousekeeperReport(HousekeeperReportProxy report, string token);

        /// <summary>
        /// Запрос на импорт обращений
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        InsertBuildControlReportResponce InsertBuildControlReport(BuildControlReportProxy report, string token);

        /// <summary>
        /// Запрос на импорт обращений
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetHousekeeperReportResponce GetHousekeeperReport(string Login, string token);

        /// <summary>
        /// Запрос на проверку пользователя
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        CheckBuildControlUserResponce CheckBuildControlUser(string Login, string PasswordHash, string token);

        /// <summary>
        /// Запрос на проверку пользователя
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetBuildControlObjectsResponce GetBuildControlObjects(long userId, string token);

        /// <summary>
        /// Запрос на проверку пользователя
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetBuildControlObjectResponce GetBuildControlObject(long objectId, string token);

        /// <summary>
        /// Запрос на проверку пользователя
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetBuildControlReportListResponce GetBuildControlReportList(long objectId, string token);

        /// <summary>
        /// Импорт сведений об обращении граждан по объекту капитального ремонта
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetHousekeeperReportResponce GetCrObjectReport(long crId, string token);

        /// <summary>
        /// Запрос на импорт обращений
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [XmlSerializerFormat]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        ObjectCRMobileResponce GetObjectsCRMobile(long RealityId, string token);
    }
}
