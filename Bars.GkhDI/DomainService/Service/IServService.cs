namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сервис для работы с услугами
    /// </summary>
    public interface IServService
    {
        /// <summary>
        /// Возвращает количество обязательных услуг
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult GetCountMandatory(BaseParams baseParams);

        /// <summary>
        /// Копирует услуги между периодами
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult CopyService(BaseParams baseParams);

        /// <summary>
        /// Возвращает информацию о периоде раскрытия
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult GetInfoServPeriod(BaseParams baseParams);

        /// <summary>
        /// Копирует услуги между периодами массово
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult CopyServPeriod(BaseParams baseParams);

        /// <summary>
        /// Возвращает список не добавленных обязательных услуг
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult GetUnfilledMandatoryServs(BaseParams baseParams);

        /// <summary>
        /// Возвращает список не добавленных обязательных услуг
        /// </summary>
        /// <param name="disclosureInfoRealityObjId">Идентификатор DisclosureInfoRealityObj"/></param>
        /// <returns>Список названий услуг</returns>
        IList<string> GetUnfilledMandatoryServsNameList(long disclosureInfoRealityObjId);

        /// <summary>
        /// Возвращает список не добавленных обязательных услуг
        /// </summary>
        /// <param name="diRoQuery">Подзапрос</param>
        /// <returns>Список названий услуг</returns>
        IDictionary<long, string[]> GetUnfilledMandatoryServsNameList(IQueryable<DisclosureInfoRealityObj> diRoQuery);

        /// <summary>
        /// Копирование сведений об использовании нежилых помещений из периода в период
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Результат операции</returns>
        IDataResult CopyUninhabitablePremisesPeriod(BaseParams baseParams);

        /// <summary>
        /// Копирование сведений об использовании мест общего пользования из периода в период
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Результат операции</returns>
        IDataResult CopyCommonAreasPeriod(BaseParams baseParams);
    }
}