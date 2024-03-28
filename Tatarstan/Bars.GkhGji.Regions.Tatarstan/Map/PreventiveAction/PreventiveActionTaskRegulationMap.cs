namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskRegulationMap : BaseEntityMap<PreventiveActionTaskRegulation>
    {
        /// <inheritdoc />
        public PreventiveActionTaskRegulationMap()
            : base(nameof(PreventiveActionTaskRegulation), "PREVENTIVE_ACTION_TASK_REGULATION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            Reference(x => x.Task, "Задание профилактического мероприятия").Column("TASK_ID");
            Reference(x => x.NormativeDoc, "Нормативный документ").Column("NORMATIVE_DOC_ID");
        }
    }
}