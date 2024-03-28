namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated
{
    using Bars.B4;

    public interface ITaskActionIsolatedItemService
    {
        /// <summary>
        /// Добавить предметы задания профилактического мероприятия
        /// </summary>
        IDataResult AddItems(BaseParams baseParams);
    }
}
