namespace Bars.GisIntegration.Base.Map.GisRole
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities.GisRole;

    /// <summary>
    /// Маппинг для связи роли ГИС с методом
    /// </summary>
    public class GisRoleMethodMap : BaseEntityMap<GisRoleMethod> 
    {
        public GisRoleMethodMap()
            : base("Bars.GisIntegration.Base.Entities.GisRole.GisRoleMethod", "GIS_ROLE_METHOD")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.MethodId, "MethodId").Column("METHOD_ID").NotNull();
            this.Property(x => x.MethodName, "MethodName").Column("METHOD_NAME").NotNull();
            this.Reference(x => x.Role, "Role").Column("GIS_ROLE_ID").NotNull().Fetch();
        }
    }
}
