namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class MotivatedPresentationViolationMap
        : BaseEntityMap<MotivatedPresentationViolation>
    {
        /// <inheritdoc />
        public MotivatedPresentationViolationMap()
            : base("Нарушение мотивированного представления", "GJI_MOTIVATED_PRESENTATION_VIOLATION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.MotivatedPresentationRealityObject, "Дом мотивированного представления").Column("MOTIVATED_PRESENTATION_ROBJECT_ID");
            this.Reference(x => x.Violation, "Нарушение").Column("VIOLATION_ID");
        }
    }
}