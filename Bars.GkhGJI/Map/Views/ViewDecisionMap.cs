namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewDisposal"</summary>
    public class ViewDecisionMap : PersistentObjectMap<ViewDecision>
    {
        
        public ViewDecisionMap() : 
                base("Bars.GkhGji.Entities.ViewDisposal", "VIEW_GJI_DECISION")
        {
        }
        
        protected override void Map()
        {
         
            this.Property(x => x.RealityObjectCount, "Количество домов").Column("RO_COUNT");
            this.Property(x => x.InspectorNames, "ФИО инспекторов").Column("INSPECTOR_NAMES");
            this.Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            this.Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            this.Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            this.Property(x => x.KindKNDGJI, "Вид контроля/надзора").Column("KIND_KND");
            this.Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");     
            this.Property(x => x.ContragentName, "Контрагент").Column("CONTRAGENT");
            this.Property(x => x.DateEnd, "Дата окончания обследования").Column("DATE_END");
            this.Property(x => x.IssuedDecision, "Поручитель").Column("ISSUED_DISPPOSAL_NAME");
            this.Property(x => x.DateStart, "Дата начала обследования").Column("DATE_START");         
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentNum, "Номер документа (целая часть)").Column("DOCUMENT_NUM");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.MunicipalityId, "Муниципальное образование первого жилого дома").Column("MU_ID");
            this.Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            this.Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            this.Property(x => x.KindCheckName, "Наименование вида проверки").Column("KIND_CHECK_NAME");
            this.Property(x => x.InspectionId, "Основание проверки").Column("INSPECTION_ID");
            this.Property(x => x.TypeDisposal, "Тип документа ГЖИ").Column("TYPE_DICISION");
            this.Property(x => x.TypeAgreementProsecutor, "TypeAgreementProsecutor").Column("TYPE_AGRPROSECUTOR").NotNull();
            this.Property(x => x.ERKNMID, "Ид в гис ерп").Column("ERKNMID");
            this.Property(x => x.ERPID, "Ид в гис ерп").Column("ERPID");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
        }
    }
}
