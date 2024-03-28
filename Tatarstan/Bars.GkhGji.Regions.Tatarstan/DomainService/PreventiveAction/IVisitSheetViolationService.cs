namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Интерфейс сервиса для <see cref="VisitSheetViolation"/>
    /// </summary>
    public interface IVisitSheetViolationService
    {
        /// <summary>
        /// Добавить нарушения
        /// </summary>
        IDataResult AddViolations(BaseParams baseParams);
    }
}