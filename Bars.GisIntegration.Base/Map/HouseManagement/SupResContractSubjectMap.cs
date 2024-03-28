namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.SupResContractSubject"
    /// </summary>
    public class SupResContractSubjectMap : BaseRisEntityMap<SupResContractSubject>
    {
        public SupResContractSubjectMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.SupResContractSubject", "SUP_RES_CONTRACT_SUBJECT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Contract, "Contract").Column("CONTRACT_ID").Fetch();
            this.Property(x => x.ServiceTypeCode, "ServiceTypeCode").Column("SERVICE_TYPE_CODE");
            this.Property(x => x.ServiceTypeGuid, "ServiceTypeGuid").Column("SERVICE_TYPE_GUID");
            this.Property(x => x.MunicipalResourceCode, "MunicipalResourceCode").Column("MUNICIPAL_RESOURCE_CODE");
            this.Property(x => x.MunicipalResourceGuid, "MunicipalResourceGuid").Column("MUNICIPAL_RESOURCE_GUID");
            this.Property(x => x.HeatingSystemType, "HeatingSystemType").Column("HEATING_SYSTEM_TYPE");
            this.Property(x => x.ConnectionSchemeType, "ConnectionSchemeType").Column("CONNECTION_SCHEME_TYPE");
            this.Property(x => x.StartSupplyDate, "StartSupplyDate").Column("START_SUPPLY_DATE");
            this.Property(x => x.EndSupplyDate, "EndSupplyDate").Column("END_SUPPLY_DATE");
            this.Property(x => x.PlannedVolume, "PlannedVolume").Column("PLANNED_VOLUME");
            this.Property(x => x.Unit, "Unit").Column("UNIT");
            this.Property(x => x.FeedingMode, "FeedingMode").Column("FEEDING_MODE");
        }
    }
    
}
