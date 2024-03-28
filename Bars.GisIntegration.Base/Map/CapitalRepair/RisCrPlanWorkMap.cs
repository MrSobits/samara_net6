namespace Bars.GisIntegration.Base.Map.CapitalRepair
{
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для сущности Bars.GisIntegration.CapitalReapair.Entities.RisCrPlanWork
    /// </summary>
    public class RisCrPlanWorkMap : BaseRisEntityMap<RisCrPlanWork>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RisCrPlanWorkMap() :
            base("Bars.GisIntegration.CapitalReapair.Entities.RisCrPlanWork", "GI_CR_PLANWORK")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.PlanGuid, "PlanGuid").Column("PLAN_GUID").Length(50);
            this.Property(x => x.ApartmentBuildingFiasGuid, "ApartmentBuildingFiasGuid").Column("BUILDING_FIAS_GUID").Length(50);
            this.Property(x => x.WorkKindCode, "WorkKindCode").Column("WORK_KIND_CODE").Length(10);
            this.Property(x => x.WorkKindGuid, "WorkKindGuid").Column("WORK_KIND_GUID").Length(50);
            this.Property(x => x.EndMonthYear, "EndMonthYear").Column("END_MONTH_YEAR");
            this.Property(x => x.MunicipalityCode, "MunicipalityCode").Column("MUNICIPALITY_CODE").Length(11);
            this.Property(x => x.MunicipalityName, "MunicipalityName").Column("MUNICIPALITY_NAME").Length(500);
        }
    }
}
