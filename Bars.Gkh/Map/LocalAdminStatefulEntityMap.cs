namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class LocalAdminStatefulEntityMap : PersistentObjectMap<LocalAdminStatefulEntity>
    {
        /// <inheritdoc />
        public LocalAdminStatefulEntityMap()
            : base(nameof(LocalAdminStatefulEntity), "GKH_LOCAL_ADMIN_STATEFUL_ENTITY")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.TypeId, "TypeId").Column("TYPE_ID");
            this.Reference(x => x.Role, "Role").Column("ROLE_ID");
        }
    }
}