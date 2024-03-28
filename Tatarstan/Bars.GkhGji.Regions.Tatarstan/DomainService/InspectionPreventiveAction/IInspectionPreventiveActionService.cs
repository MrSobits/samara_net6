namespace Bars.GkhGji.Regions.Tatarstan.DomainService.InspectionPreventiveAction
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для <see cref="InspectionActionIsolated"/>
    /// </summary>
    public interface IInspectionPreventiveActionService
    {
        /// <summary>
        /// Список профилактических мероприятий
        /// </summary>
        IDataResult ListPreventiveAction(BaseParams baseParams);
    }
}