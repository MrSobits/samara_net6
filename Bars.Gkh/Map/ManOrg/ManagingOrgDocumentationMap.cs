/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Документы управляющей организации"
///     /// </summary>
///     public class ManagingOrgDocumentationMap : BaseGkhEntityMap<ManagingOrgDocumentation>
///     {
///         public ManagingOrgDocumentationMap() : base("GKH_MAN_ORG_DOC")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
/// 
///             References(x => x.ManagingOrganization, "MAN_ORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Организационно-техническая документация"</summary>
    public class ManagingOrgDocumentationMap : BaseImportableEntityMap<ManagingOrgDocumentation>
    {
        
        public ManagingOrgDocumentationMap() : 
                base("Организационно-техническая документация", "GKH_MAN_ORG_DOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Название документа").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MAN_ORG_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
