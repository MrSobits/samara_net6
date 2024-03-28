namespace Bars.Gkh.Services.ServiceContracts.Suggestion
{
    using System.ServiceModel;

    using Bars.Gkh.Services.DataContracts.Suggestion;

    using CoreWCF.Web;

    [ServiceContract]
    public interface ISuggestionService
    {

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetRubrics")]
        Rubric[] GetRubrics();

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetProblemPlaces")]
        ProblemPlace[] GetProblemPlaces();

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetTypeProblems/{id}")]
        TypeProblem[] GetTypeProblems(long id);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "CreateSuggestion")]
        long CreateSuggestion(Suggestion suggestion);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetSuggestionList")]
        Suggestion[] GetSuggestionList(long[] ids);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetSuggestion/{id}")]
        Suggestion GetSuggestion(long id);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "AddComment")]
        long AddComment(Comment comment);

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetCategoryPosts")]
        CategoryPosts[] GetCategoryPosts();

        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetMessageSubjects/{id}")]
        MessageSubject[] GetMessageSubjects(long id);
    }
}