namespace Bars.Gkh.Regions.Tyumen.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.Gkh.ExecutionAction;

    public class MigrationCitizenSuggestionAction : BaseExecutionAction
    {
        public override string Description => "Перевести старые обращения граждан под новый функционал (Тюмень)";

        public override string Name => "Перевести старые обращения граждан под новый функционал (Тюмень)";

        public override Func<IDataResult> Action => this.MigrationCitizenSuggestion;

        protected BaseDataResult MigrationCitizenSuggestion()
        {
            var suggestionService = this.Container.Resolve<IDomainService<CitizenSuggestion>>();
            var commentService = this.Container.Resolve<IDomainService<SuggestionComment>>();
            var sugFilesService = this.Container.Resolve<IDomainService<CitizenSuggestionFiles>>();

            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            using (this.Container.Using(suggestionService, commentService, sugFilesService, sessionProvider))
            {
                var suggestions =
                    suggestionService.GetAll()
                        .Where(x => !commentService.GetAll().Any(y => y.CitizenSuggestion.Id == x.Id && y.IsFirst))
                        .ToList();

                var sugFiles =
                    sugFilesService.GetAll().Where(x => suggestionService.GetAll().Any(y => y.Id == x.CitizenSuggestion.Id)).ToList();

                var listCommentToCreate = new List<SuggestionComment>();

                var listFilesToCreate = new List<SuggestionCommentFiles>();

                try
                {
                    foreach (var sug in suggestions)
                    {
                        var comment = new SuggestionComment
                        {
                            CitizenSuggestion = sug,
                            CreationDate = sug.CreationDate,
                            Answer = sug.AnswerText,
                            AnswerDate = sug.AnswerDate,
                            IsFirst = true,
                            ExecutorManagingOrganization = sug.ExecutorManagingOrganization,
                            ExecutorZonalInspection = sug.ExecutorZonalInspection,
                            ExecutorCrFund = sug.ExecutorCrFund,
                            ExecutorMunicipality = sug.ExecutorMunicipality,
                            Description = sug.Description,
                            HasAnswer = sug.HasAnswer,
                            ProblemPlace = sug.ProblemPlace
                        };
                        listCommentToCreate.Add(comment);

                        foreach (var sugFile in sugFiles.Where(x => x.CitizenSuggestion.Id == sug.Id))
                        {
                            var file = new SuggestionCommentFiles
                            {
                                SuggestionComment = comment,
                                DocumentFile = sugFile.DocumentFile,
                                DocumentDate = sugFile.DocumentDate,
                                DocumentNumber = sugFile.DocumentNumber,
                                isAnswer = sugFile.isAnswer,
                                Description = sugFile.Description
                            };
                            listFilesToCreate.Add(file);
                        }
                    }
                }
                catch
                {
                    return new BaseDataResult(false, "Возникла ошибка при выполнении действия");
                }

                sessionProvider.CloseCurrentSession();

                using (var session = sessionProvider.OpenStatelessSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            listCommentToCreate.ForEach(x => session.Insert(x));
                            listFilesToCreate.ForEach(x => session.Insert(x));
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            return new BaseDataResult(false, "Возникла ошибка при выполнении действия");
                        }
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}