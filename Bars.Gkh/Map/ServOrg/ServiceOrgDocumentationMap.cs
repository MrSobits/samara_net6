/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Документы обслуживающей организации"
///     /// </summary>
///     public class ServiceOrgDocumentationMap : BaseGkhEntityMap<ServiceOrgDocumentation>
///     {
///         public ServiceOrgDocumentationMap() : base("GKH_SERV_ORG_DOC")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(100);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
/// 
///             References(x => x.ServiceOrganization, "SERV_ORG_ID").Not.Nullable().Fetch.Join();
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
    public class ServiceOrgDocumentationMap : BaseImportableEntityMap<ServiceOrgDocumentation>
    {
        
        public ServiceOrgDocumentationMap() : 
                base("Организационно-техническая документация", "GKH_SERV_ORG_DOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Название документа").Column("DOCUMENT_NAME").Length(100);
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            Reference(x => x.ServiceOrganization, "Управляющая организация").Column("SERV_ORG_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
