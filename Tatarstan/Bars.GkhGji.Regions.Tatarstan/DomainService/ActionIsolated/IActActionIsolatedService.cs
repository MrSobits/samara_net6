namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    /// <summary>
    /// Сервис для <see cref="ActActionIsolated"/>
    /// </summary>
    public interface IActActionIsolatedService
    {
        /// <summary>
        /// Сохранить дома по проверке
        /// </summary>
        IDataResult SaveRealityObjects(BaseParams baseParams);
        
        /// <summary>
        /// Получить список домов из задачи
        /// </summary>
        IDataResult GetRealityObjectsList(BaseParams baseParams);
        
        /// <summary>
        /// Получить список домов для определения
        /// </summary>
        IDataResult GetRealityObjectsForDefinition(BaseParams baseParams);

        /// <summary>
        /// Получить список актов по КНМ без взаимодействия для реестра
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список актов по КНМ</returns>
        IDataResult ListForRegistry(BaseParams baseParams);
    }
}