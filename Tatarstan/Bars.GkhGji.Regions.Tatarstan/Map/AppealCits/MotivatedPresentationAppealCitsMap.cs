namespace Bars.GkhGji.Regions.Tatarstan.Map.AppealCits
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;

    /// <summary>
    /// Маппинг для <see cref="MotivatedPresentationAppealCits"/>
    /// </summary>
    public class MotivatedPresentationAppealCitsMap : GkhJoinedSubClassMap<MotivatedPresentationAppealCits>
    {
        /// <inheritdoc />
        public MotivatedPresentationAppealCitsMap()
            : base("GJI_MOTIVATED_PRESENTATION_APPEALCITS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.AppealCits, nameof(MotivatedPresentationAppealCits.AppealCits)).Column("APPEAL_CITS_ID").NotNull();
            this.Property(x => x.PresentationType, nameof(MotivatedPresentationAppealCits.PresentationType)).Column("PRESENTATION_TYPE");
            this.Reference(x => x.Official, nameof(MotivatedPresentationAppealCits.Official)).Column("OFFICIAL_ID");
            this.Property(x => x.ResultType, nameof(MotivatedPresentationAppealCits.ResultType)).Column("RESULT_TYPE");
        }
    }
}