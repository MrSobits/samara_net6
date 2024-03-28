namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewActCheck"</summary>
    public class ViewActIsolatedMap : PersistentObjectMap<ViewActIsolated>
    {
        
        public ViewActIsolatedMap() : 
                base("Bars.GkhGji.Entities.ViewActIsolated", "VIEW_GJI_ACTISOLATED")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.CountExecDoc, "Количество исполнительных документов").Column("COUNT_EXEC_DOC");
            this.Property(x => x.HasViolation, "Нарушения выявлены").Column("HAS_VIOLATION").NotNull();
            this.Property(x => x.InspectorNames, "Инспекторы").Column("INSPECTOR_NAMES");
            this.Property(x => x.RealityObjectCount, "Количество домов").Column("COUNT_RO");
            this.Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            this.Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            this.Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            this.Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            this.Property(x => x.MunicipalityId, "Мунниципальное образование первого жилого дома").Column("MU_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentNum, "Целая часть номера документа").Column("DOCUMENT_NUM");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.TypeBase, "Тип основания").Column("TYPE_BASE");
            this.Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            this.Property(x => x.InspectionId, "Идентификатор основания проверки").Column("INSPECTION_ID");
            this.Property(x => x.ContragentMuName, "Контрагент МО Name").Column("CTR_MU_NAME");
            this.Property(x => x.ContragentMuId, "Контрагент МО Id").Column("CTR_MU_ID");
            this.Property(x => x.ContragentName, "Контрагент").Column("CONTRAGENT_NAME");
            this.Property(x => x.ActIsolatedGjiId, "Документ ГЖИ").Column("DOCUMENT_ID");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
        }
    }
}
