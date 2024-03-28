namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskObjectiveMap : BaseEntityMap<PreventiveActionTaskObjective>
    {
        /// <inheritdoc />
        public PreventiveActionTaskObjectiveMap()
            : base(nameof(PreventiveActionTaskObjective), "PREVENTIVE_ACTION_TASK_OBJECTIVE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.PreventiveActionTask, "Задание профилактического мероприятия").Column("PREVENTIVE_ACTION_TASK_ID").NotNull();
            this.Reference(x => x.ObjectivesPreventiveMeasure, "Цель профилактического мероприятия").Column("OBJECTIVE_ID").NotNull();
        }
    }
}
