namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class TaskOfPreventiveActionTaskMap: BaseEntityMap<TaskOfPreventiveActionTask>
    {
        public TaskOfPreventiveActionTaskMap()
            : base(nameof(TaskOfPreventiveActionTask), "GJI_TASKS_PREVENTIVE_ACTION_TASK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PreventiveActionTask, "Задание").Column("PREVENTIVE_ACTION_TASK_ID").NotNull().Fetch();
            Reference(x => x.TasksPreventiveMeasures, "Задача").Column("TASKS_PREVENTIVE_MEASURES_ID").NotNull().Fetch();
        }
    }
}