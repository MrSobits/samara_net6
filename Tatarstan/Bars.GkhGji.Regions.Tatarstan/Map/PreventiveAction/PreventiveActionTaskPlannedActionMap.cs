namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskPlannedActionMap : BaseEntityMap<PreventiveActionTaskPlannedAction>
    {
        /// <inheritdoc />
        public PreventiveActionTaskPlannedActionMap()
            : base(nameof(PreventiveActionTaskPlannedAction), "PREVENTIVE_ACTION_TASK_PLANNED_ACTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Action, "Действие").Column("ACTION").Length(1000);
            Property(x => x.Commentary, "Комментарий").Column("COMMENTARY").Length(1000);
            Reference(x => x.Task, "Задание профилактического мероприятия").Column("TASK_ID").NotNull().Fetch();
        }
    }
}