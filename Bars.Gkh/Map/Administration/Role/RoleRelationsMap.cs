namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг <see cref="LocalAdminRoleRelations"/>
    /// </summary>
    public class RoleRelationsMap : GkhBaseEntityMap<LocalAdminRoleRelations>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RoleRelationsMap() : base("GKH_LOCAL_ADMIN_ROLE_RELATIONS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ParentRole, "ParentRole").Column("PARENT_ROLE_ID");
            this.Reference(x => x.ChildRole, "ChildRole").Column("CHILD_ROLE_ID");
        }
    }
}
