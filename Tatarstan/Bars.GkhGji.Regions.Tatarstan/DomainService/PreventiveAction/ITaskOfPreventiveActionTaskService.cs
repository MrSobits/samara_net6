namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction
{
    using Bars.B4;

    public interface ITaskOfPreventiveActionTaskService
    {
        /// <summary>
        /// Добавить задачи
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult AddTasks(BaseParams baseParams);
        
        /// <summary>
        /// Получить список всех задач
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetAllTasks(BaseParams baseParams);
    }
}