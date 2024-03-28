namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для "Bars.GkhDi.Entities.RealityObjectResOrgService"</summary>
    public class RealityObjectResOrgServiceMap : BaseImportableEntityMap<RealityObjectResOrgService>
    {

        public RealityObjectResOrgServiceMap() : 
                base("Bars.GkhDi.Entitie.RealityObjectResOrgService", "DI_RORESORG_SERVICE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.StartDate, "StartDate").Column("STARTDATE");
            this.Property(x => x.EndDate, "EndDate").Column("ENDDATE");

            this.Reference(x => x.RoResOrg, "RoResOrg").Column("GKH_OBJ_RESORG_ID").Fetch();
            this.Reference(x => x.Service, "Service").Column("DI_TMP_SRV_ID").Fetch();
        }
    }
}
