namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class LocalAdminRolePermissionMap : PersistentObjectMap<LocalAdminRolePermission>
    {
        /// <inheritdoc />
        public LocalAdminRolePermissionMap()
            : base(nameof(LocalAdminRolePermission), "GKH_LOCAL_ADMIN_ROLE_PERMISSION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.PermissionId, "PermissionId").Column("PERMISSION_ID");
            this.Reference(x => x.Role, "Role").Column("ROLE_ID");
        }
    }
}