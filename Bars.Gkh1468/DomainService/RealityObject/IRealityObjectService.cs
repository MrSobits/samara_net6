namespace Bars.Gkh1468.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса работы с домами
    /// </summary>
    public interface IRealityObjectService
    {
        /// <summary>
        /// Получение данных по жилым домам
        /// </summary>
        /// <returns>Результат запроса</returns>
        IDataResult ListRealityObjectInfo();

        /// <summary>
        /// Возвращает то же самое что и метод List, но данные берет из ViewRealityObject. 
        /// Т.к. нужны наименования управляющих организаций и договоров с жилыми домами
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult ListView(BaseParams baseParams);

        /// <summary>
        /// Получение данных по жилым домам, фильтрация по типу контрагента и организации оператора
        /// </summary>
        /// <returns>Результат запроса</returns>
        IDataResult ListForPassport(BaseParams baseParams);

        /// <summary>
        /// Список Жилых домов по поставщику ресурсов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult ListByPublicServOrg(BaseParams baseParams);

        /// <summary>
        /// Список Жилых домов по муниципальным образованиям поставщика ресурсов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult ListRoByPublicServOrg(BaseParams baseParams);

        /// <summary>
        /// Список Жилых домов по договору РСО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        IDataResult ListByPublicServOrgContract(BaseParams baseParams);
    }
}