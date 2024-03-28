namespace Bars.GisIntegration.Base.Map.Services
{
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг Bars.Gkh.Ris.Entities.Services.RisCompletedWork
    /// </summary>
    public class RisCompletedWorkMap : BaseRisEntityMap<RisCompletedWork>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public RisCompletedWorkMap() :
            base("Bars.Gkh.Ris.Entities.Services.RisCompletedWork", "RIS_COMPLETED_WORK")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ActDate, "Дата акта").Column("ACT_DATE");
            this.Property(x => x.ActNumber, "Номер акта").Column("ACT_NUMBER");
            this.Reference(x => x.WorkPlanItem, "Плановая работа").Column("WORK_PLAN_ITEM_ID").Fetch().NotNull();
            this.Reference(x => x.ObjectPhoto, "Фотография объекта").Column("OBJECT_PHOTO_ID").Fetch();
            this.Reference(x => x.ActFile, "Файл акта").Column("ACT_FILE_ID").Fetch().NotNull();
        }
    }
}
