namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Дефектная ведомость"</summary>
    public class SpecialDefectListMap : BaseImportableEntityMap<SpecialDefectList>
    {
        
        public SpecialDefectListMap() : 
                base("Дефектная ведомость", "CR_SPECIAL_OBJ_DEFECT_LIST")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.TypeWork, "Вид работы").Column("TYPE_WORK_ID").Fetch();
            this.Reference(x => x.Work, "Вид работы").Column("WORK_ID").Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.Sum, "Сумма по ведомости, руб").Column("SUM");
            this.Property(x => x.Volume, "Объем").Column("DL_VOL");
            this.Property(x => x.CostPerUnitVolume, "Стоимость на единицу объема по ведомости").Column("COST_PER_UNIT");
            this.Property(x => x.TypeDefectList, "Тип дефектной ведомости").Column("TYPE_DEFECT_LIST");
            this.Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
        }
    }
}
