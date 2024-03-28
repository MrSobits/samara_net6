/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Enums;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     public class ViewActRemovalMap : PersistentObjectMap<ViewActRemoval>
///     {
///         public ViewActRemovalMap(): base("VIEW_GJI_ACT_REMOVAL")
///         {
///             Map(x => x.InspectorNames, "INSPECTOR_NAMES");
///             Map(x => x.RealityObjectCount, "COUNT_ROBJECT");
///             Map(x => x.CountExecDoc, "COUNT_CHILDREN");
///             Map(x => x.ParentDocumentName, "PARENT_NAME");
///             Map(x => x.RealityObjectIds, "RO_IDS");
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.ActRemovalGjiId, "DOCUMENT_ID");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.TypeParentDocument, "PARENT_TYPE").CustomType<TypeDocumentGji>();
///             Map(x => x.ParentContragentMuName, "CTR_MU_NAME");
///             Map(x => x.ParentContragentMuId, "CTR_MU_ID");
///             Map(x => x.ParentContragent, "CONTRAGENT_NAME");
///             Map(x => x.InspectionId, "INSPECTION_ID");
///             Map(x => x.TypeBase, "TYPE_BASE").CustomType<TypeBase>();
///             Map(x => x.TypeDocumentGji, "TYPE_DOC").CustomType<TypeDocumentGji>();
///             Map(x => x.ParentDocumentId, "PARENT_ID");
///             Map(x => x.TypeRemoval, "TYPE_REMOVAL").CustomType<YesNoNotSet>();
///             Map(x => x.MunicipalityId, "MU_ID");
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewActRemoval"</summary>
    public class ViewActRemovalMap : PersistentObjectMap<ViewActRemoval>
    {
        
        public ViewActRemovalMap() : 
                base("Bars.GkhGji.Entities.ViewActRemoval", "VIEW_GJI_ACT_REMOVAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.InspectorNames, "Инспекторы").Column("INSPECTOR_NAMES");
            Property(x => x.RealityObjectCount, "Количество домов").Column("COUNT_ROBJECT");
            Property(x => x.CountExecDoc, "Количество исполнительных документов").Column("COUNT_CHILDREN");
            Property(x => x.ParentDocumentName, "Наименование родительского документа").Column("PARENT_NAME");
            Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            Property(x => x.RealityObjectAddresses, "Адреса жилых домов").Column("RO_ADDRESSES");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.ActRemovalGjiId, "Акт проверки предписания (Он же акт устранения нарушений)").Column("DOCUMENT_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Целая часть номера документа").Column("DOCUMENT_NUM");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            Property(x => x.TypeParentDocument, "Тип родительского документа").Column("PARENT_TYPE");
            Property(x => x.ParentContragentMuName, "Контрагент МО Name").Column("CTR_MU_NAME");
            Property(x => x.ParentContragentMuId, "Контрагент МО Id").Column("CTR_MU_ID");
            Property(x => x.ParentContragent, "Юридическое лицо - то есть родительского документа (Предписание/Протокол)").Column("CONTRAGENT_NAME");
            Property(x => x.InspectionId, "Идентификатор основания проверки").Column("INSPECTION_ID");
            Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            Property(x => x.ParentDocumentId, "Документ ГЖИ тоесть родительского документа").Column("PARENT_ID");
            Property(x => x.TypeRemoval, "Признак устранено нарушение или нет").Column("TYPE_REMOVAL");
            Property(x => x.MunicipalityId, "Мунниципальное образование первого жилого дома").Column("MU_ID");
            Property(x => x.ControlType, "Тип Контроля надзора").Column("CONTROL_TYPE");
            Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
        }
    }
}
