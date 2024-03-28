namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated
{
    using Bars.B4;

    /// <summary>
    /// Сервис для домов заданий КНМ без взаимодействия с контролируемыми лицами
    /// </summary>
    public interface ITaskActionIsolatedRealityObjectService
    {
        /// <summary>
        /// Добавить дома
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат добавления домов</returns>
        IDataResult AddRealityObjects(BaseParams baseParams);
    }
}