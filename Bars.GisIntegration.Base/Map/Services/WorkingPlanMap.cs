namespace Bars.GisIntegration.Base.Map.Services
{
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.Services.WorkingPlan"
    /// </summary>
    public class WorkingPlanMap : BaseRisEntityMap<WorkingPlan>
    {
        public WorkingPlanMap() :
            base("Bars.Gkh.Ris.Entities.Services.WorkingPlan", "RIS_WORKPLAN")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.WorkList, "Перечень работ/услуг").Column("WORKLIST_ID").NotNull().Fetch();
            this.Property(x => x.Year, "Год").NotNull().Column("YEAR");
        }
    }
}