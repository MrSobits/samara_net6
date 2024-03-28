namespace Bars.Gkh.Gis.Map.ManOrg
{
    using B4.Modules.Mapping.Mappers;
    using Entities.ManOrg;

    /// <summary>
    /// Маппинг для ManOrgBilCommunalService
    /// </summary>
    public class ManOrgBilCommunalServiceMap : BaseEntityMap<ManOrgBilCommunalService>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ManOrgBilCommunalServiceMap()
            : base("Коммунальные услуги управляющей организации", "MANORG_BIL_COMMUNAL_SERVICE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANORG_ID").NotNull().Fetch();
            this.Reference(x => x.BilService, "Услуга из биллинга").Column("BILSERVICE_ID").NotNull().Fetch();
            this.Property(x => x.Name, "Name").Column("NAME");
            this.Property(x => x.Resource, "Resource").Column("RESOURCE");
        }
    }
}