namespace Bars.GisIntegration.Base.Map.Inspection
{
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Inspection.Entities.InspectionPlan"
    /// </summary>
    public class InspectionPlanMap : BaseRisEntityMap<InspectionPlan>
    {
        public InspectionPlanMap() : 
                base("Bars.GisIntegration.Inspection.Entities.InspectionPlan", "GI_INSPECTION_PLAN")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Year, "Year").Column("YEAR");
            this.Property(x => x.ApprovalDate, "ApprovalDate").Column("APPROVAL_DATE");
            this.Property(x => x.UriRegistrationNumber, "UriRegistrationNumber").Column("URI_REGISTRATION_NUMBER");
        }
    }
}