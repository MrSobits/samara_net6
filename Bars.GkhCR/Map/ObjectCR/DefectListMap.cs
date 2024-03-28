/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
///     using Bars.GkhCr.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Дефектная ведомость"
///     /// </summary>
///     public class DefectListMap : BaseGkhEntityByCodeMap<DefectList>
///     {
///         public DefectListMap() : base("CR_OBJ_DEFECT_LIST")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.Sum, "SUM");
///             Map(x => x.Volume, "DL_VOL");
///             Map(x => x.CostPerUnitVolume, "COST_PER_UNIT");
///             Map(x => x.DocumentName, "DOCUMENT_NAME", false, 300);
///             Map(x => x.TypeDefectList, "TYPE_DEFECT_LIST", false);
/// 
///             References(x => x.ObjectCr, "OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Work, "WORK_ID", ReferenceMapConfig.Fetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.TypeWork, "TYPE_WORK_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Дефектная ведомость"</summary>
    public class DefectListMap : BaseImportableEntityMap<DefectList>
    {
        
        public DefectListMap() : 
                base("Дефектная ведомость", "CR_OBJ_DEFECT_LIST")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(36);
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.TypeWork, "Вид работы").Column("TYPE_WORK_ID").Fetch();
            Reference(x => x.Work, "Вид работы").Column("WORK_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.Sum, "Сумма по ведомости, руб").Column("SUM");
            Property(x => x.Volume, "Объем").Column("DL_VOL");
            Property(x => x.CostPerUnitVolume, "Стоимость на единицу объема по ведомости").Column("COST_PER_UNIT");
            Property(x => x.TypeDefectList, "Тип дефектной ведомости").Column("TYPE_DEFECT_LIST");
            Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
        }
    }
}
