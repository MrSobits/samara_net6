namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.SupResContractTemperatureChart"
    /// </summary>
    public class SupResContractTemperatureChartMap : BaseRisEntityMap<SupResContractTemperatureChart>
    {
        public SupResContractTemperatureChartMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.SupResContractTemperatureChart", "SUP_RES_CONTRACT_TEMPERATURE_CHART")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Contract, "Contract").Column("CONTRACT_ID").Fetch();
            this.Property(x => x.OutsideTemperature, "OutsideTemperature").Column("OUTSIDE_TEMPERATURE");
            this.Property(x => x.FlowLineTemperature, "FlowLineTemperature").Column("FLOWLINE_TEMPERATURE");
            this.Property(x => x.OppositeLineTemperature, "OppositeLineTemperature").Column("OPPOSITELINE_TEMPERATURE");
        }
    }
}
