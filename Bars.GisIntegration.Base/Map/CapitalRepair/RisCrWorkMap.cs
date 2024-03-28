namespace Bars.GisIntegration.Base.Map.CapitalRepair
{
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для сущности Bars.GisIntegration.CapitalReapair.Entities.RisCrWork
    /// </summary>
    public class RisCrWorkMap : BaseRisEntityMap<RisCrWork>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RisCrWorkMap() :
            base("Bars.GisIntegration.CapitalReapair.Entities.RisCrWork", "GI_CR_WORK")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.WorkPlanGUID, "WorkPlanGUID").Column("PLANWORK_GUID").Length(50);
            this.Property(x => x.ApartmentBuildingFiasGuid, "ApartmentBuildingFiasGuid").Column("BUILDING_FIAS_GUID").Length(50);
            this.Property(x => x.WorkKindCode, "WorkKindCode").Column("WORK_KIND_CODE").Length(10);
            this.Property(x => x.WorkKindGuid, "WorkKindGuid").Column("WORK_KIND_GUID").Length(50);
            this.Property(x => x.EndMonthYear, "EndMonthYear").Column("END_MONTH_YEAR");           
            this.Reference(x => x.Contract, "RisCrContract").Column("CONTRACT_ID");
            this.Property(x => x.StartDate, "StartDate").Column("START_DATE");
            this.Property(x => x.EndDate, "EndDate").Column("END_DATE");
            this.Property(x => x.Cost, "Cost").Column("COST");
            this.Property(x => x.OtherUnit, "OtherUnit").Column("OTHER_UNIT").Length(50);
            this.Property(x => x.CostPlan, "CostPlan").Column("COST_PLAN");
            this.Property(x => x.Volume, "Volume").Column("VOLUME");
            this.Property(x => x.AdditionalInfo, "AdditionalInfo").Column("ADDITIONAL_INFO").Length(500);
            this.Property(x => x.Okei, "Okei").Column("OKEI").Length(50);
        }
    }
}
