namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    /// <summary>
    /// Mapping <see cref="TaskActionIsolatedKnmAction"/>
    /// </summary>
    public class TaskActionIsolatedKnmActionMap : BaseKnmActionMainEntityRefMap<TaskActionIsolatedKnmAction, TaskActionIsolated>
    {
        /// <inheritdoc />
        public TaskActionIsolatedKnmActionMap()
            : base(nameof(TaskActionIsolatedKnmAction), "GJI_TASK_ACTIONISOLATED_KNM_ACTION")
        {
        }

        /// <inheritdoc />
        protected override string MainEntityName() => "Задание КНМ";

        /// <inheritdoc />
        protected override string MainEntityColumn() => "TASK_ACTIONISOLATED_ID";
    }
}