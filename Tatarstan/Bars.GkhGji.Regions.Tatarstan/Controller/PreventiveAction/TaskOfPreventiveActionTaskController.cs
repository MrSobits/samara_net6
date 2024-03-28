namespace Bars.GkhGji.Regions.Tatarstan.Controller.PreventiveAction
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class TaskOfPreventiveActionTaskController: B4.Alt.DataController<TaskOfPreventiveActionTask>
    {
        public ActionResult AddTasks(BaseParams baseParams)
        {
            var taskOfPreventiveActionTaskService = this.Container.Resolve<ITaskOfPreventiveActionTaskService>();

            using (this.Container.Using(taskOfPreventiveActionTaskService))
            {
                return new JsonNetResult(taskOfPreventiveActionTaskService.AddTasks(baseParams));
            }
        }
        
        public ActionResult GetAllTasks(BaseParams baseParams)
        {
            var taskOfPreventiveActionTaskService = this.Container.Resolve<ITaskOfPreventiveActionTaskService>();

            using (this.Container.Using(taskOfPreventiveActionTaskService))
            {
                return new JsonNetResult(taskOfPreventiveActionTaskService.GetAllTasks(baseParams));
            }
        }
    }
}