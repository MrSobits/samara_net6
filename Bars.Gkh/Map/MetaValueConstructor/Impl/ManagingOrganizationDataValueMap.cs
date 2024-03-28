namespace Bars.Gkh.Map.MetaValueConstructor.Impl
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Маппинг <see cref="ManagingOrganizationDataValue"/>
    /// </summary>
    public class ManagingOrganizationDataValueMap : JoinedSubClassMap<ManagingOrganizationDataValue>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ManagingOrganizationDataValueMap()
            : base("Bars.Gkh.MetaValueConstructor.Impl.ManagingOrganizationDataValue", "GKH_EF_CONSTRUCTOR_MO_VALUE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Dynamics, "Динамика роста").Column("DYNAMICS").NotNull();
            this.Reference(x => x.EfManagingOrganization, "Управляющая организация").Column("EF_MANAGING_ORG_ID").NotNull().Fetch();
        }
    }
}
