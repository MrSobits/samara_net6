namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.SupResContractServiceResource"
    /// </summary>
    public class SupResContractServiceResourceMap : BaseRisEntityMap<SupResContractServiceResource>
    {
        public SupResContractServiceResourceMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.SupResContractServiceResource", "SUP_RES_CONTRACT_SERVICE_RESOURCE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Contract, "Contract").Column("CONTRACT_ID").Fetch();
            this.Property(x => x.ServiceTypeCode, "ServiceTypeCode").Column("SERVICE_TYPE_CODE");
            this.Property(x => x.ServiceTypeGuid, "ServiceTypeGuid").Column("SERVICE_TYPE_GUID");
            this.Property(x => x.MunicipalResourceCode, "MunicipalResourceCode").Column("MUNICIPAL_RESOURCE_CODE");
            this.Property(x => x.MunicipalResourceGuid, "MunicipalResourceGuid").Column("MUNICIPAL_RESOURCE_GUID");
            this.Property(x => x.StartSupplyDate, "StartSupplyDate").Column("START_SUPPLY_DATE");
            this.Property(x => x.EndSupplyDate, "EndSupplyDate").Column("END_SUPPLY_DATE");
        }
    }
}
