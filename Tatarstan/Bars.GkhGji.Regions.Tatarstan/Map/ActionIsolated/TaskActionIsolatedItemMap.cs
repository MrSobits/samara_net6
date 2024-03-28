namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedItemMap : BaseEntityMap<TaskActionIsolatedItem>
    {
        public TaskActionIsolatedItemMap()
            : base(nameof(TaskActionIsolatedItem), "GJI_TASK_ACTIONISOLATED_ITEM")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Task, "Задание КНМ без взаимодействия с контролируемыми лицами").Column("TASK_ID");
            this.Reference(x => x.Item, "Предмет КНМ без взаимодействия с контролируемыми лицами").Column("ITEM_ID");
        }
    }
}
