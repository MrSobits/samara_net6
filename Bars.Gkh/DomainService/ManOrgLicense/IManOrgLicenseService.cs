namespace Bars.Gkh.DomainService
{
    using System.Collections;

    using Bars.B4;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Сервис для работы с Лицензиями УО
    /// </summary>
    public interface IManOrgLicenseService
    {
        /// <summary>
        /// Получить информацию
        /// </summary>
        IDataResult GetInfo(BaseParams baseParams);

        /// <summary>
        /// Получить информацию
        /// </summary>
        IDataResult GetInfo(string type, long id);

        /// <summary>
        /// Получить информацию о статусе лицензии
        /// </summary>
        IDataResult GetStateInfo(BaseParams baseParams);

        /// <summary>
        /// Получить список
        /// </summary>
        IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount);

        /// <summary>
        /// Получить список по типу обращения
        /// </summary>
        IDataResult ListForRequestType(BaseParams baseParams);

        /// <summary>
        /// Добавить должностные лица
        /// </summary>
        IDataResult AddPersons(BaseParams baseParams);

        /// <summary>
        /// Получить должностное лицо по контрагенту
        /// </summary>
        IDataResult GetListPersonByContragent(BaseParams baseParams, bool isPaging, out int totalCount);

        /// <summary>
        /// Получить информацию по контрагенту
        /// </summary>
        IDataResult GetContragentInfo(BaseParams baseParams);

        IDataResult GetPrintFormResult(BaseParams baseParams);
    }

    /// <summary>
    /// Прокси-класс Лицензии УО
    /// </summary>
    public class ManOrgLicenseInfo
    {
        public long licenseId { get; set; }

        public long requestId { get; set; }

        public LicenseRequestType? requestType { get; set; }
    }
}