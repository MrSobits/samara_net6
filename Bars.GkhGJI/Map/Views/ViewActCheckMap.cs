/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Enums;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     public class ViewActCheckMap : PersistentObjectMap<ViewActCheck>
///     {
///         public ViewActCheckMap() : base("VIEW_GJI_ACTCHECK")
///         {
///             Map(x => x.CountExecDoc, "COUNT_EXEC_DOC");
///             Map(x => x.HasViolation, "HAS_VIOLATION").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.InspectorNames, "INSPECTOR_NAMES");
///             Map(x => x.RealityObjectCount, "COUNT_RO");
///             Map(x => x.RealityObjectIds, "RO_IDS");
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.TypeBase, "TYPE_BASE").CustomType<TypeBase>();
///             Map(x => x.TypeDocumentGji, "TYPE_DOC").CustomType<TypeDocumentGji>();
///             Map(x => x.InspectionId, "INSPECTION_ID");
///             Map(x => x.ContragentMuName, "CTR_MU_NAME");
///             Map(x => x.ContragentMuId, "CTR_MU_ID");
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.ActCheckGjiId, "DOCUMENT_ID");
/// 
///             References(x => x.State, "STATE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewActCheck"</summary>
    public class ViewActCheckMap : PersistentObjectMap<ViewActCheck>
    {
        
        public ViewActCheckMap() : 
                base("Bars.GkhGji.Entities.ViewActCheck", "VIEW_GJI_ACTCHECK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CountExecDoc, "Количество исполнительных документов").Column("COUNT_EXEC_DOC");
            Property(x => x.HasViolation, "Нарушения выявлены").Column("HAS_VIOLATION").NotNull();
            Property(x => x.InspectorNames, "Инспекторы").Column("INSPECTOR_NAMES");
            Property(x => x.RealityObjectCount, "Количество домов").Column("COUNT_RO");
            Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            Property(x => x.RealityObjectAddresses, "Адреса жилых домов").Column("RO_ADDRESSES");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MunicipalityId, "Мунниципальное образование первого жилого дома").Column("MU_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Целая часть номера документа").Column("DOCUMENT_NUM");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            Property(x => x.TypeBase, "Тип основания").Column("TYPE_BASE");
            Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            Property(x => x.InspectionId, "Идентификатор основания проверки").Column("INSPECTION_ID");
            Property(x => x.ContragentMuName, "Контрагент МО Name").Column("CTR_MU_NAME");
            Property(x => x.ContragentMuId, "Контрагент МО Id").Column("CTR_MU_ID");
            Property(x => x.ContragentName, "Контрагент").Column("CONTRAGENT_NAME");
            Property(x => x.ActCheckGjiId, "Документ ГЖИ").Column("DOCUMENT_ID");
            Property(x => x.ControlType, "Тип Контроля надзора").Column("CONTROL_TYPE");
            Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
        }
    }
}
