/// <mapping-converter-backup>
/// using Bars.B4.DataAccess;
/// using Bars.GkhGji.Regions.Tomsk.Entities.ResolPros;
/// using Bars.GkhGji.Regions.Tomsk.Enums;
/// 
/// namespace Bars.GkhGji.Regions.Tomsk.Map.ResolPros
/// {
///     public class ResolProsDefinitionMap : BaseEntityMap<ResolProsDefinition>
///     {
///         public ResolProsDefinitionMap()
///             : base("GJI_RESOL_PROS_DEFINITION")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.ExecutionDate, "EXECUTION_DATE");
///             Map(x => x.ExecutionTime, "EXECUTION_TIME");
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.ResolutionInitAdminViolation, "RESOL_INIT_VIOLATION").Length(500);
///             Map(x => x.ReturnReason, "RETURN_REASON").Length(500);
///             Map(x => x.RequestNeed, "REQUEST_NEED").Length(500);
///             Map(x => x.AdditionalDocuments, "ADDITIONAL_DOCUMENTS").Length(255);
///             Map(x => x.TypeResolProsDefinition, "TYPE_RES_PROS_DEFINITION").Not.Nullable().CustomType<TypeResolProsDefinition>();
///             Map(x => x.DateSubmissionDocument, "DATE_SUBMISSION_DOCUMENT");
/// 
///             References(x => x.ResolPros, "RESOL_PROS_ID").Not.Nullable().Fetch.Join();
///             References(x => x.IssuedDefinition, "ISSUED_DEFINITION_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map.ResolPros
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities.ResolPros;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.ResolPros.ResolProsDefinition"</summary>
    public class ResolProsDefinitionMap : BaseEntityMap<ResolProsDefinition>
    {
        
        public ResolProsDefinitionMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.ResolPros.ResolProsDefinition", "GJI_RESOL_PROS_DEFINITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.ExecutionDate, "ExecutionDate").Column("EXECUTION_DATE");
            Property(x => x.ExecutionTime, "ExecutionTime").Column("EXECUTION_TIME");
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            Property(x => x.ResolutionInitAdminViolation, "ResolutionInitAdminViolation").Column("RESOL_INIT_VIOLATION").Length(500);
            Property(x => x.ReturnReason, "ReturnReason").Column("RETURN_REASON").Length(500);
            Property(x => x.RequestNeed, "RequestNeed").Column("REQUEST_NEED").Length(500);
            Property(x => x.AdditionalDocuments, "AdditionalDocuments").Column("ADDITIONAL_DOCUMENTS").Length(255);
            Property(x => x.TypeResolProsDefinition, "TypeResolProsDefinition").Column("TYPE_RES_PROS_DEFINITION").NotNull();
            Property(x => x.DateSubmissionDocument, "DateSubmissionDocument").Column("DATE_SUBMISSION_DOCUMENT");
            Reference(x => x.ResolPros, "ResolPros").Column("RESOL_PROS_ID").NotNull().Fetch();
            Reference(x => x.IssuedDefinition, "IssuedDefinition").Column("ISSUED_DEFINITION_ID").Fetch();
        }
    }
}
