/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
/// 
///     using Enums;
///     using Entities;
/// 
///     public class InspectionDocGjiReferenceMap : BaseGkhEntityMap<InspectionDocGjiReference>
///     {
///         public InspectionDocGjiReferenceMap() : base("GJI_INSPECTION_DOC_REF")
///         {
///             Map(x => x.TypeReference, "TYPE_REFERENCE").Not.Nullable().CustomType<TypeInspectionDocGjiReference>();
/// 
///             References(x => x.Inspection, "INSPECTION_ID").Not.Nullable().LazyLoad();
///             References(x => x.Document, "DOCUMENT_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "сущность связи основания проверки и документа гжи"</summary>
    public class InspectionDocGjiReferenceMap : BaseEntityMap<InspectionDocGjiReference>
    {
        
        public InspectionDocGjiReferenceMap() : 
                base("сущность связи основания проверки и документа гжи", "GJI_INSPECTION_DOC_REF")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeReference, "Тип связи").Column("TYPE_REFERENCE").NotNull();
            Reference(x => x.Inspection, "Основание проверки").Column("INSPECTION_ID").NotNull();
            Reference(x => x.Document, "Документ ГЖИ").Column("DOCUMENT_ID").NotNull();
        }
    }
}
