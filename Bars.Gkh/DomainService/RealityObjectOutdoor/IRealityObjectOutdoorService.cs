namespace Bars.Gkh.DomainService.RealityObjectOutdoor
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Entities.RealityObj;

    /// <summary>
    /// Сервис для работы со дворами.
    /// </summary>
    public interface IRealityObjectOutdoorService
    {
        /// <summary>
        /// Обновляет ссылку на двор у дома.
        /// </summary>
        ActionResult UpdateOutdoorInRealityObjects(BaseParams baseParams);

        /// <summary>
        /// Удаляет ссылку на двор у дома.
        /// </summary>
        ActionResult DeleteOutdoorFromRealityObject(BaseParams baseParams);

        /// <summary>
        /// Формирование списка дворов в зависимости от назначения
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="baseParams">Параметры для списка</param>
        /// <returns>Список дворов</returns>
        IDataResult GetList(IDomainService<RealityObjectOutdoor> domainService, BaseParams baseParams);
    }
}
