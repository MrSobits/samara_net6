namespace Bars.Gkh.DomainService
{
    using System.Collections;
    using System.Collections.Generic;

    using B4;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Сервис для работы с обращением за лицензией
    /// </summary>
    public interface IManOrgLicenseRequestService
    {
        /// <summary>
        /// Получить список
        /// </summary>
        IList GetList(BaseParams baseParams, bool isPaging, out int totalCount);

        /// <summary>
        /// Добавить должностное лицо
        /// </summary>
        IDataResult AddPersons(BaseParams baseParams);

        /// <summary>
        /// Добавить документ к заявке
        /// </summary>
        IDataResult AddProvDocs(BaseParams baseParams);

        /// <summary>
        /// Получить информацию по контрагенту
        /// </summary>
        IDataResult GetContragentInfo(BaseParams baseParams);

        /// <summary>
        /// Получить список УО
        /// </summary>
        IDataResult ListManOrg(BaseParams baseParams);

        /// <summary>
        /// Получить должностное лицо по контрагенту
        /// </summary>
        IDataResult GetListPersonByContragent(BaseParams baseParams, bool isPaging, out int totalCount);

        /// <summary>
        /// Получить список УО по типу обращения
        /// </summary>
        IDataResult ListManOrgForRequestType(BaseParams baseParams);

        /// <summary>
        /// Получить список лицензий для УО
        /// </summary>
        IList<ManOrgLicense> ListLicenseByManOrg(long contragentId);

        /// <summary>
        /// Получить список документов для лицензии
        /// </summary>
        IList<ManOrgLicenseDoc> ListLicenseDocs(long licenseId);

        /// <summary>
        /// Добавить документ к заявке
        /// </summary>
        IDataResult AddSMEVRequest(BaseParams baseParams, RequestSMEVType requestType, long requestId);
    }
}