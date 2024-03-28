namespace Bars.GisIntegration.Base.Map.Services
{
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.Services.WorkPlanItem"
    /// </summary>
    public class WorkPlanItemMap : BaseRisEntityMap<WorkPlanItem>
    {
        public WorkPlanItemMap() :
            base("Bars.Gkh.Ris.Entities.Services.WorkPlanItem", "RIS_WORKPLAN_ITEM")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.WorkingPlan, "План по перечню работ/услуг").Column("WORKPLAN_ID").NotNull().Fetch();
            this.Reference(x => x.WorkListItem, "Работа/услуга перечня").Column("WORKLIST_ITEM_ID").NotNull().Fetch();
            this.Property(x => x.Year, "Год").NotNull().Column("YEAR");
            this.Property(x => x.Month, "Месяц").NotNull().Column("MONTH");
            this.Property(x => x.WorkDate, "Дата начала работ по плану").NotNull().Column("DATE_WORK");
            this.Property(x => x.WorkCount, "Количество работ").NotNull().Column("COUNT_WORK");
        }
    }
}