namespace Bars.Gkh.Map.MetaValueConstructor.Impl
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Маппинг <see cref="ManagingOrganizationEfficiencyRating"/>
    /// </summary>
    public class ManagingOrganizationEfficiencyRatingMap : BaseEntityMap<ManagingOrganizationEfficiencyRating>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ManagingOrganizationEfficiencyRatingMap()
            : base("Bars.Gkh.MetaValueConstructor.Impl.ManagingOrganizationEfficiencyRating", "GKH_EF_MANAGING_ORGANIZATION")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Dynamics, "Динамика роста").Column("DYNAMICS").NotNull();
            this.Property(x => x.Rating, "Рейтинг").Column("RATING").NotNull();

            this.Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANAGING_ORG_ID").NotNull().Fetch();
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID").NotNull().Fetch();
        }
    }
}