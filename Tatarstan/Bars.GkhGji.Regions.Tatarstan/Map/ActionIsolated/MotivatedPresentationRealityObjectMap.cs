namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class MotivatedPresentationRealityObjectMap
        : BaseEntityMap<MotivatedPresentationRealityObject>
    {
        /// <inheritdoc />
        public MotivatedPresentationRealityObjectMap()
            : base("Дом мотивированного представления", "GJI_MOTIVATED_PRESENTATION_ROBJECT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.MotivatedPresentation, "Мотивированоое представление").Column("MOTIVATED_PRESENTATION_ID");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID");
        }
    }
}