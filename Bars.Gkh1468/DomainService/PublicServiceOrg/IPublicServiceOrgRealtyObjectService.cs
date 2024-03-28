namespace Bars.Gkh1468.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис по работе с домами РСО
    /// </summary>
    public interface IPublicServiceOrgRealtyObjectService
    {
        /// <summary>
        /// Добавить дом к РСО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        IDataResult AddRealtyObjects(BaseParams baseParams);

        /// <summary>
        /// Добавить дом к контракту РСО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        IDataResult AddRealtyObjectsToContract(BaseParams baseParams);

        /// <summary>
        /// Получить контрактов РСО для жилого дома
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        IDataResult ListByRealityObject(BaseParams baseParams);
    }
}