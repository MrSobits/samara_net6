namespace Bars.Gkh.DomainService
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерфейс сервиса работы с УО и домами
    /// </summary>
    public interface IManagingOrgRealityObjectService
    {
        /// <summary>
        /// Добавить дома в УО (не договоры управления)
        /// </summary>
        IDataResult AddRealityObjects(BaseParams baseParams);

        /// <summary>
        /// Получение текущего контракта между управляйкой и МКД
        /// </summary>
        /// <param name="realityObject">МКД</param>
        /// <returns>Контракт</returns>
        ManOrgBaseContract GetCurrentManOrg(RealityObject realityObject);

        /// <summary>
        /// Получение текущего контракта между управляйкой и МКД
        /// </summary>
        /// <param name="realityObject">МКД</param>
        /// <param name="date">Дата, на которую ищется УО</param>
        /// <returns>Контракт</returns>
        ManOrgBaseContract GetManOrgOnDate(RealityObject realityObject, DateTime date);

        /// <summary>
        /// Вернуть все активные протоколы на дату
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Запрос</returns>
        IQueryable<ManOrgContractRealityObject> GetAllActive(DateTime date);


        /// <summary>
        /// Вернуть все активные протоколы на текущую дату
        /// </summary>
        /// <returns>Запрос</returns>
        IQueryable<ManOrgContractRealityObject> GetAllActive();


        /// <summary>
        /// Вернуть все активные протоколы на дату
        /// </summary>
        /// <param name="dateStart">Дата начала</param>
        /// <param name="dateEnd">Дата окончания</param>
        /// <returns>Запрос</returns>
        IQueryable<ManOrgContractRealityObject> GetAllActive(DateTime dateStart, DateTime? dateEnd);
    }
}