namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskRealityObjectMap: BaseEntityMap<TaskActionIsolatedRealityObject>
    {
        public TaskRealityObjectMap()
            : base(nameof(TaskActionIsolatedRealityObject), "GJI_TASK_ACTION_ROBJECT")
        {
        }

        protected override void Map()
        {
            this.Reference(x=>x.Task, "Task").Column("TASK_ID");
            this.Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID");
        }
    }
}
