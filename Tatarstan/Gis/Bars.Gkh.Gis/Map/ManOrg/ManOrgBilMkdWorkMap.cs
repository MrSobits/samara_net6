namespace Bars.Gkh.Gis.Map.ManOrg
{
    using B4.Modules.Mapping.Mappers;
    using Entities.ManOrg;

    /// <summary>
    /// Маппинг для ManOrgBilMkdWork
    /// </summary>
    public class ManOrgBilMkdWorkMap : BaseEntityMap<ManOrgBilMkdWork>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ManOrgBilMkdWorkMap() : base("Работы и услуги управляющей организации по МКД", "MANORG_BIL_MKDWORK")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.MkdWork, "Работа по МКД").Column("MKDWORK_ID").NotNull().Fetch();
            this.Reference(x => x.WorkService, "Работы и услуги управляющей организации").Column("WORKSERVICE_ID").NotNull().Fetch();
        }
    }
}
