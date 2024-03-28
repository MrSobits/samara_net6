namespace Bars.GkhGji.Regions.Tatarstan.DomainService.InspectionActionIsolated
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса для <see cref="InspectionActionIsolated"/>
    /// </summary>
    public interface IInspectionActionIsolatedService
    {
        /// <summary>
        /// Список мероприятий
        /// </summary>
        IDataResult ListTaskActionIsolated(BaseParams baseParams);
    }
}