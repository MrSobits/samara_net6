/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
///     using Bars.GkhCr.Enums;
/// 
///     public class ArchiveSmrMap : BaseGkhEntityMap<ArchiveSmr>
///     {
///         public ArchiveSmrMap() : base("CR_OBJ_CMP_ARCHIVE")
///         {
///             Map(x => x.VolumeOfCompletion, "VOLUME_COMPLETION");
///             Map(x => x.ManufacturerName, "MANUFACTURER_NAME").Length(300);
///             Map(x => x.PercentOfCompletion, "PERCENT_COMPLETION");
///             Map(x => x.CostSum, "COST_SUM");
///             Map(x => x.CountWorker, "COUNT_WORKER");
///             Map(x => x.DateChangeRec, "DATE_CHANGE_REC");
///             Map(x => x.TypeArchiveSmr, "TYPE_ARCHIVE_CMP").Not.Nullable().CustomType<TypeArchiveSmr>();
/// 
///             References(x => x.StageWorkCr, "STAGE_WORK_CR_ID").Fetch.Join();
///             References(x => x.TypeWorkCr, "TYPE_WORK_CR_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Архив значений в мониторинге СМР"</summary>
    public class ArchiveSmrMap : BaseImportableEntityMap<ArchiveSmr>
    {
        
        public ArchiveSmrMap() : 
                base("Архив значений в мониторинге СМР", "CR_OBJ_CMP_ARCHIVE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.VolumeOfCompletion, "Объем выполнения").Column("VOLUME_COMPLETION");
            Property(x => x.ManufacturerName, "Производитель").Column("MANUFACTURER_NAME").Length(300);
            Property(x => x.PercentOfCompletion, "Процент выполнения").Column("PERCENT_COMPLETION");
            Property(x => x.CostSum, "Сумма расходов").Column("COST_SUM");
            Property(x => x.CountWorker, "Чиленность рабочих(дробное так как м.б. смысл поля как пол ставки)").Column("COUNT_WORKER");
            Property(x => x.DateChangeRec, "Дата изменения записи").Column("DATE_CHANGE_REC");
            Property(x => x.TypeArchiveSmr, "Тип архива значений в мониторинге СМР").Column("TYPE_ARCHIVE_CMP").NotNull();
            Reference(x => x.StageWorkCr, "Этап работы").Column("STAGE_WORK_CR_ID").Fetch();
            Reference(x => x.TypeWorkCr, "Вид работы").Column("TYPE_WORK_CR_ID").Fetch();
        }
    }
}
