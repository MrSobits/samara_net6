namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class MotivatedPresentationMap
        : GkhJoinedSubClassMap<MotivatedPresentation>
    {
        /// <inheritdoc />
        public MotivatedPresentationMap()
            : base("GJI_MOTIVATED_PRESENTATION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.CreationPlace, "Место составления").Column("CREATION_PLACE_ID");
            this.Reference(x => x.IssuedMotivatedPresentation, "Должностное лицо (ДЛ), вынесшее мотивированное представление").Column("ISSUED_MOTIVATED_PRESENTATION_ID");
            this.Reference(x => x.ResponsibleExecution, "Ответственный за исполнение").Column("RESPONSIBLE_EXECUTION_ID");
        }
    }
}