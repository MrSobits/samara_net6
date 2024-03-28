namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class ControlOrganizationControlTypeRelationMap : BaseEntityMap<ControlOrganizationControlTypeRelation>
    {
        /// <inheritdoc />
        public ControlOrganizationControlTypeRelationMap()
            : base("Bars.GkhGji.Regions.Tatarstan.Entities.ControlOrganizationControlTypeRelation", "GKH_CONTROL_ORGANIZATION_TYPE_RELATION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ControlOrganization, "ControlOrganization").Column("control_org_id").NotNull();
            this.Reference(x => x.ControlType, "ControlType").Column("control_type_id").NotNull();
        }
    }
}
