namespace Bars.Gkh.Gis.Map.ManOrg
{
    using B4.Modules.Mapping.Mappers;
    using Entities.ManOrg;

    /// <summary>
    /// Маппинг для ManOrgBilAdditionService
    /// </summary>
    public class ManOrgBilAdditionServiceMap : BaseEntityMap<ManOrgBilAdditionService>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ManOrgBilAdditionServiceMap()
            : base("Дополнительные услуги управляющей организации", "MANORG_BIL_ADDITION_SERVICE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANORG_ID").NotNull().Fetch();
            this.Reference(x => x.BilService, "Услуга из биллинга").Column("BILSERVICE_ID").NotNull().Fetch();
        }
    }
}