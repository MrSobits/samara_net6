namespace Bars.GisIntegration.Base.Map.CapitalRepair
{
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для сущности Bars.GisIntegration.CapitalReapair.Entities.RisCrPlan
    /// </summary>
    public class RisCrPlanMap : BaseRisEntityMap<RisCrPlan>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RisCrPlanMap() :
            base("Bars.GisIntegration.CapitalReapair.Entities.RisCrPlan", "GI_CR_PLAN")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME").Length(500);
            this.Property(x => x.MunicipalityCode, "MunicipalityCode").Column("MUNICIPALITY_CODE").Length(11);
            this.Property(x => x.MunicipalityName, "MunicipalityName").Column("MUNICIPALITY_NAME").Length(500);
            this.Property(x => x.StartMonthYear, "StartMonthYear").Column("START_MONTH_YEAR");
            this.Property(x => x.EndMonthYear, "EndMonthYear").Column("END_MONTH_YEAR");
        }
    }
}
