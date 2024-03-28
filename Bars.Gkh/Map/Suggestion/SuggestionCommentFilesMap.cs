/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Suggestion
/// {
///     using Entities.Suggestion;
/// 
///     public class SuggestionCommentFilesMap : BaseGkhEntityMap<SuggestionCommentFiles>
///     {
///         public SuggestionCommentFilesMap()
///             : base("GKH_CIT_SUG_COMMENT_FILES")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.Description, "DESCRIPTION");
///             Map(x => x.isAnswer, "IS_ANSWER");
///             
///             References(x => x.SuggestionComment, "SUG_COMMENT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.DocumentFile, "DOCUMENT_FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.Suggestion.SuggestionCommentFiles"</summary>
    public class SuggestionCommentFilesMap : BaseImportableEntityMap<SuggestionCommentFiles>
    {
        
        public SuggestionCommentFilesMap() : 
                base("Bars.Gkh.Entities.Suggestion.SuggestionCommentFiles", "GKH_CIT_SUG_COMMENT_FILES")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNumber, "DocumentNumber").Column("DOCUMENT_NUMBER");
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.isAnswer, "isAnswer").Column("IS_ANSWER");
            Reference(x => x.SuggestionComment, "SuggestionComment").Column("SUG_COMMENT_ID").NotNull().Fetch();
            Reference(x => x.DocumentFile, "DocumentFile").Column("DOCUMENT_FILE_ID").Fetch();
        }
    }
}
