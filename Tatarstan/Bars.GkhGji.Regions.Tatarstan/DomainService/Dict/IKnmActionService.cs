namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Dict
{
    using Bars.B4;

    /// <summary>
    /// Сервис для работы с действиями в рамках КНМ
    /// </summary>
    public interface IKnmActionService
    {
        /// <summary>
        /// Добавить запланированные действия решения
        /// </summary>
        IDataResult AddDecisionPlannedActions(BaseParams baseParams);

        /// <summary>
        /// Добавить запланированные действия задания КНМ
        /// </summary>
        IDataResult AddTaskActionIsolatedPlannedActions(BaseParams baseParams);
    }
}