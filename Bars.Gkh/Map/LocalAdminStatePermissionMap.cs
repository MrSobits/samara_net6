namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class LocalAdminStatePermissionMap : PersistentObjectMap<LocalAdminStatePermission>
    {
        /// <inheritdoc />
        public LocalAdminStatePermissionMap()
            : base(nameof(LocalAdminStatePermission), "GKH_LOCAL_ADMIN_STATE_PERMISSION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.PermissionId, "PermissionId").Column("PERMISSION_ID");
            this.Reference(x => x.Role, "Role").Column("ROLE_ID");
            this.Reference(x => x.State, "State").Column("STATE_ID");
        }
    }
}