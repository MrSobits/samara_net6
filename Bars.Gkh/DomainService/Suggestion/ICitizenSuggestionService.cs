namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Entities.Suggestion;

    public interface ICitizenSuggestionService
    {
        IDataResult ListExecutor(BaseParams baseParams);

        void SaveAnswerFile(BaseParams baseParams, CitizenSuggestion citSuggestion);

        void SaveCommentAnswerFile(BaseParams baseParams, SuggestionComment suggestionComment);
    }
}