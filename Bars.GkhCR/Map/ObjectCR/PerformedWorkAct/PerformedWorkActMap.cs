namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Акт выполненных работ"</summary>
    public class PerformedWorkActMap : BaseImportableEntityMap<PerformedWorkAct>
    {
        
        public PerformedWorkActMap() : 
                base("Акт выполненных работ", "CR_OBJ_PERFOMED_WORK_ACT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(250);
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").Fetch();
            Reference(x => x.TypeWorkCr, "Работа").Column("TYPE_WORK_CR_ID").NotNull();
            Property(x => x.DocumentNum, "Номер акта").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Volume, "Объем").Column("VOLUME");
            Property(x => x.FactVolume, "Объем").Column("FACT_VOLUME");
            Property(x => x.Sum, "Сумма").Column("SUM");
            Property(x => x.OverLimits, "Превышение плановой суммы").Column("OVER_LIMITS");
            Property(x => x.DateFrom, "Дата от").Column("DATE_FROM");
            Property(x => x.SumTransfer, "Сумма перевода").Column("CR_OBJ_PERFOMED_WORK_ACT");
            Property(x => x.DateFromTransfer, "Дата перевода").Column("DATE_FROM_TRANSFER");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Reference(x => x.CostFile, "Справка о стоимости выполненных работ и затрат").Column("COST_FILE_ID").Fetch();
            Reference(x => x.DocumentFile, "Документ акта").Column("DOC_FILE_ID").Fetch();
            Reference(x => x.AdditionFile, "Приложение к акту").Column("ADDIT_FILE_ID").Fetch();
            Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
            Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            Property(x => x.GisGkhDocumentGuid, "ГИС ЖКХ GUID документа акта").Column("GIS_GKH_DOC_GUID").Length(36);
        }
    }
}
