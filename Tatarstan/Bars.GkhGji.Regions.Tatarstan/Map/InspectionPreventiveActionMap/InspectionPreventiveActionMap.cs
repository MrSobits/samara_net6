namespace Bars.GkhGji.Regions.Tatarstan.Map.InspectionPreventiveActionMap
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionPreventiveAction;

    public class InspectionPreventiveActionMap : JoinedSubClassMap<InspectionPreventiveAction>
    {
        /// <inheritdoc />
        public InspectionPreventiveActionMap()
            : base("Проверки по профилактическому мероприятию", "GJI_INSPECTION_PREVENTIVEACTION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.TypeForm, "Форма проверки").Column("TYPE_FORM");
            this.Reference(x => x.PreventiveAction, "Профилаткическое мероприятия").Column("PREVENTIVE_ACTION_ID");
        }
    }
}