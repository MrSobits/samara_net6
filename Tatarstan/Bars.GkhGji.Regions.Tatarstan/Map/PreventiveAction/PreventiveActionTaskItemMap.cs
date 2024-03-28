namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskItemMap : BaseEntityMap<PreventiveActionTaskItem>
    {
        /// <inheritdoc />
        public PreventiveActionTaskItemMap()
            : base(nameof(PreventiveActionTaskItem), "PREVENTIVE_ACTION_TASK_ITEM")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Task, "Задание профилактического мероприятия").Column("TASK_ID");
            this.Reference(x => x.Item, "Предмет профилактического мероприятия").Column("ITEM_ID");
        }
    }
}