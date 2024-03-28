namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class TasksPreventiveMeasuresMap : BaseGkhDictMap<TasksPreventiveMeasures>
    {
        /// <inheritdoc />
        public TasksPreventiveMeasuresMap()
            : base("Задачи профилактических мероприятий", "GJI_DICT_TASKS_PREVENTIVE_MEASURES")
        {
        }
    }
}