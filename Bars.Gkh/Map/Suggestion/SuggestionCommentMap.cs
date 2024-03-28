/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Suggestion
/// {
///     using Bars.Gkh.Entities.Suggestion;
/// 
///     public class SuggestionCommentMap : BaseGkhEntityMap<SuggestionComment>
///     {
///         public SuggestionCommentMap() : base("GKH_CIT_SUG_COMMENT")
///         {
///             References(x => x.CitizenSuggestion, "SUG_ID").Not.Nullable().Fetch.Join();
///             
///             Map(x => x.CreationDate, "CREATION_DATE");
///             Map(x => x.Question, "QUESTION");
///             Map(x => x.Answer, "ANSWER");
///             Map(x => x.AnswerDate, "ANSWER_DATE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;
    
    
    /// <summary>Маппинг для "Комментарии"</summary>
    public class SuggestionCommentMap : BaseImportableEntityMap<SuggestionComment>
    {
        
        public SuggestionCommentMap() : 
                base("Комментарии", "GKH_CIT_SUG_COMMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.CreationDate, "CreationDate").Column("CREATION_DATE");
            Property(x => x.Question, "Question").Column("QUESTION");
            Property(x => x.Answer, "Answer").Column("ANSWER");
            Property(x => x.AnswerDate, "AnswerDate").Column("ANSWER_DATE");
            Property(x => x.IsFirst, "IsFirst").Column("IS_FIRST");
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.HasAnswer, "HasAnswer").Column("HAS_ANSWER");
            Reference(x => x.CitizenSuggestion, "CitizenSuggestion").Column("SUG_ID").NotNull().Fetch();
            Reference(x => x.ExecutorManagingOrganization, "ExecutorManagingOrganization").Column("EXECUTOR_MANORG_ID").Fetch();
            Reference(x => x.ExecutorMunicipality, "ExecutorMunicipality").Column("EXECUTOR_MUNICIPALITY_ID").Fetch();
            Reference(x => x.ExecutorZonalInspection, "ExecutorZonalInspection").Column("EXECUTOR_ZONAL_INSP_ID").Fetch();
            Reference(x => x.ExecutorCrFund, "ExecutorCrFund").Column("EXECUTOR_CR_FUND_ID").Fetch();
            Reference(x => x.ProblemPlace, "ProblemPlace").Column("PROBLEM_PLACE_ID").Fetch();
        }
    }
}
