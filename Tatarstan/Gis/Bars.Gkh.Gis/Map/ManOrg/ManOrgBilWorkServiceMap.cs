namespace Bars.Gkh.Gis.Map.ManOrg
{
    using B4.Modules.Mapping.Mappers;
    using Entities.ManOrg;

    /// <summary>
    /// Маппинг для ManOrgBilWorkService
    /// </summary>
    public class ManOrgBilWorkServiceMap : BaseEntityMap<ManOrgBilWorkService>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ManOrgBilWorkServiceMap()
            : base("Работы и услуги управляющей организации", "MANORG_BIL_WORK_SERVICE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANORG_ID").NotNull().Fetch();
            this.Reference(x => x.BilService, "Услуга из биллинга").Column("BILSERVICE_ID").NotNull().Fetch();
            this.Property(x => x.Purpose, "Purpose").Column("PURPOSE");
            this.Property(x => x.Type, "Type").Column("TYPE");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }
}