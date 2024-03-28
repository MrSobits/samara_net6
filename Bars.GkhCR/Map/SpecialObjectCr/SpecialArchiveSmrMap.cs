namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    /// <summary>Маппинг для "Архив значений в мониторинге СМР"</summary>
    public class SpecialArchiveSmrMap : BaseImportableEntityMap<SpecialArchiveSmr>
    {
        public SpecialArchiveSmrMap() : 
                base("Архив значений в мониторинге СМР", "CR_SPECIAL_OBJ_CMP_ARCHIVE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.VolumeOfCompletion, "Объем выполнения").Column("VOLUME_COMPLETION");
            this.Property(x => x.ManufacturerName, "Производитель").Column("MANUFACTURER_NAME").Length(300);
            this.Property(x => x.PercentOfCompletion, "Процент выполнения").Column("PERCENT_COMPLETION");
            this.Property(x => x.CostSum, "Сумма расходов").Column("COST_SUM");
            this.Property(x => x.CountWorker, "Чиленность рабочих(дробное так как м.б. смысл поля как пол ставки)").Column("COUNT_WORKER");
            this.Property(x => x.DateChangeRec, "Дата изменения записи").Column("DATE_CHANGE_REC");
            this.Property(x => x.TypeArchiveSmr, "Тип архива значений в мониторинге СМР").Column("TYPE_ARCHIVE_CMP").NotNull();

            this.Reference(x => x.StageWorkCr, "Этап работы").Column("STAGE_WORK_CR_ID").Fetch();
            this.Reference(x => x.TypeWorkCr, "Вид работы").Column("TYPE_WORK_CR_ID").Fetch();
        }
    }
}
