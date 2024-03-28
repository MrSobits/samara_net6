/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
/// 	using B4.DataAccess.ByCode;
/// 	using Gkh.Map;
/// 	using Entities;
/// 
/// 	/// <summary>
/// 	/// Маппинг для сущности "Сметный расчет по работе"
/// 	/// </summary>
/// 	public class EstimateCalculationMap : BaseGkhEntityByCodeMap<EstimateCalculation>
/// 	{
/// 		public EstimateCalculationMap()
/// 			: base("CR_OBJ_ESTIMATE_CALC")
/// 		{
/// 			Map(x => x.OtherCost, "OTHER_COST");
/// 			Map(x => x.TotalEstimate, "TOTAL_ESTIMATE");
/// 			Map(x => x.TotalDirectCost, "TOTAL_DIRECT_COST");
/// 			Map(x => x.OverheadSum, "OVERHEAD_SUM");
/// 			Map(x => x.Nds, "NDS");
/// 			Map(x => x.EstimateProfit, "ESTIMATE_PROFIT");
/// 			Map(x => x.IsSumWithoutNds, "IS_WITHOUT_NDS");
/// 
/// 			Map(x => x.ResourceStatmentDocumentName, "RES_STAT_DOC_NAME", false, 300);
/// 			Map(x => x.ResourceStatmentDocumentNum, "RES_STAT_DOC_NUM", false, 50);
/// 			Map(x => x.ResourceStatmentDateFrom, "RES_STAT_DOC_DATE");
/// 
/// 			Map(x => x.EstimateDocumentName, "ESTIMATE_DOC_NAME", false, 300);
/// 			Map(x => x.EstimateDocumentNum, "ESTIMATE_DOC_NUM", false, 50);
/// 			Map(x => x.EstimateDateFrom, "ESTIMATE_DOC_DATE");
/// 
/// 			Map(x => x.FileEstimateDocumentName, "ESTIMATE_FILE_DOC_NAME", false, 300);
/// 			Map(x => x.FileEstimateDocumentNum, "ESTIMATE_FILE_DOC_NUM", false, 50);
/// 			Map(x => x.FileEstimateDateFrom, "ESTIMATE_FILE_DOC_DATE");
/// 			Map(x => x.EstimationType, "ESTIMATION_TYPE");
/// 
/// 			References(x => x.ObjectCr, "OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
/// 			References(x => x.TypeWorkCr, "TYPE_WORK_CR_ID", ReferenceMapConfig.Fetch);
/// 
/// 			References(x => x.ResourceStatmentFile, "FILE_RES_STATMENT_ID", ReferenceMapConfig.Fetch);
/// 			References(x => x.EstimateFile, "FILE_ESTIMATE_ID", ReferenceMapConfig.Fetch);
/// 			References(x => x.FileEstimateFile, "FILE_ESTIMATE_FILE_ID", ReferenceMapConfig.Fetch);
/// 			References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
/// 		}
/// 	}
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Сметный расчет по работе"</summary>
    public class EstimateCalculationMap : BaseImportableEntityMap<EstimateCalculation>
    {
        
        public EstimateCalculationMap() : 
                base("Сметный расчет по работе", "CR_OBJ_ESTIMATE_CALC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.TypeWorkCr, "Вид работы").Column("TYPE_WORK_CR_ID").Fetch();
            Property(x => x.ResourceStatmentDocumentName, "Документ ведомости ресурсов").Column("RES_STAT_DOC_NAME").Length(300);
            Property(x => x.EstimateDocumentName, "Документ сметы").Column("ESTIMATE_DOC_NAME").Length(300);
            Property(x => x.FileEstimateDocumentName, "Документ файла сметы").Column("ESTIMATE_FILE_DOC_NAME").Length(300);
            Property(x => x.ResourceStatmentDocumentNum, "Номер документа ведомости ресурсов").Column("RES_STAT_DOC_NUM").Length(50);
            Property(x => x.EstimateDocumentNum, "Номер документа сметы").Column("ESTIMATE_DOC_NUM").Length(50);
            Property(x => x.FileEstimateDocumentNum, "Номер документа файла сметы").Column("ESTIMATE_FILE_DOC_NUM").Length(50);
            Property(x => x.ResourceStatmentDateFrom, "Дата от ведомости ресурсов").Column("RES_STAT_DOC_DATE");
            Property(x => x.EstimateDateFrom, "Дата от сметы").Column("ESTIMATE_DOC_DATE");
            Property(x => x.FileEstimateDateFrom, "Дата от файла сметы").Column("ESTIMATE_FILE_DOC_DATE");
            Reference(x => x.ResourceStatmentFile, "Файл от ведомости ресурсов").Column("FILE_RES_STATMENT_ID").Fetch();
            Reference(x => x.EstimateFile, "Файл от сметы").Column("FILE_ESTIMATE_ID").Fetch();
            Reference(x => x.FileEstimateFile, "Файл от файла сметы").Column("FILE_ESTIMATE_FILE_ID").Fetch();
            Property(x => x.OtherCost, "Другие затраты").Column("OTHER_COST");
            Property(x => x.TotalEstimate, "Итого по смете").Column("TOTAL_ESTIMATE");
            Property(x => x.TotalDirectCost, "Итого прямые затраты").Column("TOTAL_DIRECT_COST");
            Property(x => x.OverheadSum, "Накладные расходы").Column("OVERHEAD_SUM");
            Property(x => x.Nds, "НДС").Column("NDS");
            Property(x => x.EstimateProfit, "Сметная прибыль").Column("ESTIMATE_PROFIT");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Property(x => x.IsSumWithoutNds, "Сумма по ресурсам/материалам указана без НДС").Column("IS_WITHOUT_NDS");
            Property(x => x.EstimationType, "Тип сметы").Column("ESTIMATION_TYPE");
            Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
        }
    }
}
