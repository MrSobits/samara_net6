/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     public class ViewPresentationMap : PersistentObjectMap<ViewPresentation>
///     {
///         public ViewPresentationMap() : base("VIEW_GJI_PRESENTATION")
///         {
///             Map(x => x.RealityObjectIds, "RO_IDS");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.PhysicalPerson, "PHYSICAL_PERSON");
///             Map(x => x.TypeInitiativeOrg, "TYPE_INITIATIVE_ORG").CustomType<TypeInitiativeOrgGji>();
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.TypeBase, "TYPE_BASE").CustomType<TypeBase>();
///             Map(x => x.TypeDocumentGji, "TYPE_DOC").CustomType<TypeDocumentGji>();
///             Map(x => x.InspectionId, "INSPECTION_ID");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.MunicipalityName, "MUNICIPALITY_NAME");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.OfficialId, "OFFICIAL_ID");
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewPresentation"</summary>
    public class ViewPresentationMap : PersistentObjectMap<ViewPresentation>
    {
        
        public ViewPresentationMap() : 
                base("Bars.GkhGji.Entities.ViewPresentation", "VIEW_GJI_PRESENTATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.RealityObjectIds, "Строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Целая часть номер документа").Column("DOCUMENT_NUM");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON");
            Property(x => x.TypeInitiativeOrg, "Кем вынесено").Column("TYPE_INITIATIVE_ORG");
            Property(x => x.ContragentName, "Контрагент (исполнитель)").Column("CONTRAGENT_NAME");
            Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            Property(x => x.InspectionId, "Идентификатор основания проверки").Column("INSPECTION_ID");
            Property(x => x.MunicipalityId, "Идентификатор муниципального образования контрагента").Column("MU_ID");
            Property(x => x.MunicipalityName, "Наименование муниципального образования контрагента").Column("MUNICIPALITY_NAME");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.OfficialId, "Идентификатор ДЛ вынесшего представление").Column("OFFICIAL_ID");
            Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
        }
    }
}
