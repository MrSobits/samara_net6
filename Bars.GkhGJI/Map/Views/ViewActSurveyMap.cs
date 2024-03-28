/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     public class ViewActSurveyMap : PersistentObjectMap<ViewActSurvey>
///     {
///         public ViewActSurveyMap() : base("VIEW_GJI_ACTSURVEY")
///         {
///             Map(x => x.InspectorNames, "INSPECTOR_NAMES");
///             Map(x => x.RealityObjectAddresses, "RO_ADDRESS");
///             Map(x => x.RealityObjectIds, "RO_IDS");
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.ActSurveyGjiId, "DOCUMENT_ID");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.TypeBase, "TYPE_BASE").CustomType<TypeBase>();
///             Map(x => x.TypeDocumentGji, "TYPE_DOC").CustomType<TypeDocumentGji>();
///             Map(x => x.TypeSurvey, "FACT_SURVEYED").CustomType<SurveyResult>();
///             Map(x => x.InspectionId, "INSPECTION_ID");
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewActSurvey"</summary>
    public class ViewActSurveyMap : PersistentObjectMap<ViewActSurvey>
    {
        
        public ViewActSurveyMap() : 
                base("Bars.GkhGji.Entities.ViewActSurvey", "VIEW_GJI_ACTSURVEY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.InspectorNames, "Инспекторы").Column("INSPECTOR_NAMES");
            Property(x => x.RealityObjectAddresses, "Адреса жилых домов").Column("RO_ADDRESS");
            Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.ActSurveyGjiId, "Акт обследования").Column("DOCUMENT_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Целая часть номера документа").Column("DOCUMENT_NUM");
            Property(x => x.DocumentNumber, "номер документа").Column("DOCUMENT_NUMBER");
            Property(x => x.MunicipalityId, "Наименование муниципального образования").Column("MU_ID");
            Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            Property(x => x.TypeSurvey, "Результат обследования").Column("FACT_SURVEYED");
            Property(x => x.InspectionId, "Идентификатор проверки").Column("INSPECTION_ID");
            Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
        }
    }
}
