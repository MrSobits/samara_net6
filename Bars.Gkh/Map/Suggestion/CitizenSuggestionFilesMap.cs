/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Suggestion
/// {
///     using Entities.Suggestion;
/// 
///     public class CitizenSuggestionFilesMap : BaseGkhEntityMap<CitizenSuggestionFiles>
///     {
///         public CitizenSuggestionFilesMap()
///             : base("GKH_CIT_SUG_FILES")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.Description, "DESCRIPTION");
///             Map(x => x.isAnswer, "IS_ANSWER");
///             
///             References(x => x.CitizenSuggestion, "SUGGESTION_ID").Not.Nullable().Fetch.Join();
///             References(x => x.DocumentFile, "DOCUMENT_FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;
    
    
    /// <summary>Маппинг для "Файл обращения граждан"</summary>
    public class CitizenSuggestionFilesMap : BaseImportableEntityMap<CitizenSuggestionFiles>
    {
        
        public CitizenSuggestionFilesMap() : 
                base("Файл обращения граждан", "GKH_CIT_SUG_FILES")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNumber, "DocumentNumber").Column("DOCUMENT_NUMBER");
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.isAnswer, "isAnswer").Column("IS_ANSWER");
            Reference(x => x.CitizenSuggestion, "CitizenSuggestion").Column("SUGGESTION_ID").NotNull().Fetch();
            Reference(x => x.DocumentFile, "DocumentFile").Column("DOCUMENT_FILE_ID").Fetch();
            Property(x => x.Name, "Наименование").Column("NAME").Length(500).NotNull();
            Property(x => x.Hash, "Hash").Column("HASH");
            Property(x => x.GisGkhGuid, "ГИС ЖКХ Guid").Column("GIS_GKH_GUID").Length(36);
        }
    }
}
