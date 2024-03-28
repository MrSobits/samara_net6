namespace Bars.GisIntegration.Base.Map.Services
{
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.Services.RisHouseService"
    /// </summary>
    public class WorkListItemMap : BaseRisEntityMap<WorkListItem>
    {
        public WorkListItemMap() :
            base("Bars.Gkh.Ris.Entities.Services.WorkListItem", "RIS_WORKLIST_ITEM")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.WorkList, "Перечень работ/услуг").Column("WORKLIST_ID").Fetch().NotNull();
            this.Property(x => x.TotalCost, "Общая стоимость").Column("TOTAL_COST");
            this.Property(x => x.WorkItemCode, "Код работы/услуги организации (НСИ 59)").Column("WORK_ITEM_CODE");
            this.Property(x => x.WorkItemGuid, "Гуид работы/услуги организации (НСИ 59)").Column("WORK_ITEM_GUID");
            this.Property(x => x.Index, "Номер строки в перечне работ и услуг").Column("INDEX");
        }
    }
}