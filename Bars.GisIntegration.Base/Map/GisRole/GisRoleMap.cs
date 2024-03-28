namespace Bars.GisIntegration.Base.Map.GisRole
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities.GisRole;

    /// <summary>
    /// Маппинг для роли ГИС
    /// </summary>
    public class GisRoleMap : BaseEntityMap<GisRole> 
    {
        public GisRoleMap()
            : base("Bars.GisIntegration.Base.Entities.GisRole.GisRole", "GIS_ROLE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME");
        }
    }
}
