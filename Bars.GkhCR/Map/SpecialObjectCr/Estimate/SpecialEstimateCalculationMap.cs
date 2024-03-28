namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Сметный расчет по работе"</summary>
    public class SpecialEstimateCalculationMap : BaseImportableEntityMap<SpecialEstimateCalculation>
    {
        public SpecialEstimateCalculationMap() : 
                base("Сметный расчет по работе", "CR_SPECIAL_OBJ_ESTIMATE_CALC")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            this.Property(x => x.ResourceStatmentDocumentName, "Документ ведомости ресурсов").Column("RES_STAT_DOC_NAME").Length(300);
            this.Property(x => x.EstimateDocumentName, "Документ сметы").Column("ESTIMATE_DOC_NAME").Length(300);
            this.Property(x => x.FileEstimateDocumentName, "Документ файла сметы").Column("ESTIMATE_FILE_DOC_NAME").Length(300);
            this.Property(x => x.ResourceStatmentDocumentNum, "Номер документа ведомости ресурсов").Column("RES_STAT_DOC_NUM").Length(50);
            this.Property(x => x.EstimateDocumentNum, "Номер документа сметы").Column("ESTIMATE_DOC_NUM").Length(50);
            this.Property(x => x.FileEstimateDocumentNum, "Номер документа файла сметы").Column("ESTIMATE_FILE_DOC_NUM").Length(50);
            this.Property(x => x.ResourceStatmentDateFrom, "Дата от ведомости ресурсов").Column("RES_STAT_DOC_DATE");
            this.Property(x => x.EstimateDateFrom, "Дата от сметы").Column("ESTIMATE_DOC_DATE");
            this.Property(x => x.FileEstimateDateFrom, "Дата от файла сметы").Column("ESTIMATE_FILE_DOC_DATE");
            this.Property(x => x.OtherCost, "Другие затраты").Column("OTHER_COST");
            this.Property(x => x.TotalEstimate, "Итого по смете").Column("TOTAL_ESTIMATE");
            this.Property(x => x.TotalDirectCost, "Итого прямые затраты").Column("TOTAL_DIRECT_COST");
            this.Property(x => x.OverheadSum, "Накладные расходы").Column("OVERHEAD_SUM");
            this.Property(x => x.Nds, "НДС").Column("NDS");
            this.Property(x => x.EstimateProfit, "Сметная прибыль").Column("ESTIMATE_PROFIT");
            this.Property(x => x.IsSumWithoutNds, "Сумма по ресурсам/материалам указана без НДС").Column("IS_WITHOUT_NDS");
            this.Property(x => x.EstimationType, "Тип сметы").Column("ESTIMATION_TYPE");
            this.Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");

            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.TypeWorkCr, "Вид работы").Column("TYPE_WORK_CR_ID").Fetch();
            this.Reference(x => x.ResourceStatmentFile, "Файл от ведомости ресурсов").Column("FILE_RES_STATMENT_ID").Fetch();
            this.Reference(x => x.EstimateFile, "Файл от сметы").Column("FILE_ESTIMATE_ID").Fetch();
            this.Reference(x => x.FileEstimateFile, "Файл от файла сметы").Column("FILE_ESTIMATE_FILE_ID").Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
