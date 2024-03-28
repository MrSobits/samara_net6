namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    /// <summary>Маппинг для "Вид работы КР"</summary>
    public class SpecialTypeWorkCrMap : BaseImportableEntityMap<SpecialTypeWorkCr>
    {
        public SpecialTypeWorkCrMap()
            : base("Вид работы КР", "CR_SPECIAL_OBJ_TYPE_WORK")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(250);
            this.Property(x => x.HasPsd, "Наличие ПСД").Column("HAS_PSD").NotNull();
            this.Property(x => x.Volume, "Объем (плановый)").Column("VOLUME");
            this.Property(x => x.SumMaterialsRequirement, "Потребность материалов").Column("SUM_MAT");
            this.Property(x => x.Sum, "Сумма (плановая)").Column("SUM");
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.DateStartWork, "Дата начала работ").Column("DATE_START_WORK");
            this.Property(x => x.DateEndWork, "Дата окончания работ").Column("DATE_END_WORK");
            this.Property(x => x.VolumeOfCompletion, "Объем выполнения").Column("VOLUME_COMPLETION");
            this.Property(x => x.ManufacturerName, "Производитель").Column("MANUFACTURER_NAME").Length(2000);
            this.Property(x => x.PercentOfCompletion, "Процент выполнения").Column("PERCENT_COMPLETION");
            this.Property(x => x.CostSum, "Сумма расходов").Column("COST_SUM");
            this.Property(x => x.CountWorker, "Чиленность рабочих(дробное так как м.б. смысл поля как пол ставки)").Column("COUNT_WORKER");
            this.Property(x => x.AdditionalDate, "Доп. срок").Column("ADD_DATE_END");
            this.Property(x => x.YearRepair, "Год ремонта").Column("YEAR_REPAIR");
            this.Property(x => x.IsActive, "Признак является ли запись Активной").Column("IS_ACTIVE");
            this.Property(x => x.IsDpkrCreated, "Признак создана ли запись из ДПКР").Column("IS_DPKR_CREATED");

            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.FinanceSource, "Разрез финансирования").Column("FIN_SOURCE_ID").Fetch();
            this.Reference(x => x.Work, "Вид работы").Column("WORK_ID").Fetch();
            this.Reference(x => x.StageWorkCr, "Этап работы").Column("STAGE_WORK_CR_ID").Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}