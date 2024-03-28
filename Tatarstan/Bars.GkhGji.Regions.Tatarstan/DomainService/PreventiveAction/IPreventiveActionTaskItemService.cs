namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction
{
    using Bars.B4;

    public interface IPreventiveActionTaskItemService
    {
        /// <summary>
        /// Добавить предметы задания профилактического мероприятия
        /// </summary>
        IDataResult AddItems(BaseParams baseParams);
    }
}