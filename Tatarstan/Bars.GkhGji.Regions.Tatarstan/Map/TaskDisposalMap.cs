namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    /// <summary>
    /// Маппинг полей сущности <see cref="TaskDisposal"/>
    /// </summary>
    public class TaskDisposalMap : JoinedSubClassMap<TaskDisposal>
    {
        /// <inheritdoc />
        public TaskDisposalMap()
            : base("Задание", "GJI_TASK_DISPOSAL")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
        }
    }
}
