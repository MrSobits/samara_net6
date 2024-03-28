namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Orgs;

    public class ControlOrganizationMap : BaseEntityMap<ControlOrganization>
    {
        /// <inheritdoc />
        public ControlOrganizationMap()
            : base("Bars.GkhGji.Regions.Tatarstan.Entities.Orgs.ControlOrganization", "GKH_CONTROL_ORGANIZATION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID").NotNull();
	        this.Property(x => x.TorId, "TorId").Column("TOR_ID");
        }
    }
}
