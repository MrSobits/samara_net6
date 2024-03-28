namespace Bars.Gkh.DomainService
{
    using System;
    using System.Linq;

    using B4;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public class CitizenSuggestionService : ICitizenSuggestionService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListExecutor(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var executorType = baseParams.Params.GetAs<ExecutorType>("executorType");

            switch (executorType)
            {
                case ExecutorType.Mo:
                {
                    var data = Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                        .Select(x => new {x.Id, x.Contragent.Name});

                    var totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                }
                case ExecutorType.Mu:
                {
                    var data = Container.Resolve<IDomainService<Municipality>>().GetAll()
                        .Select(x => new {x.Id, x.Name});

                    var totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                }
                case ExecutorType.Gji:
                {
                    var data = Container.Resolve<IDomainService<ZonalInspection>>().GetAll()
                        .Select(x => new {x.Id, x.Name});

                    var totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                }

            }

            return new ListDataResult();
        }


        public void SaveAnswerFile(BaseParams baseParams, CitizenSuggestion citSuggestion)
        {
            var file = baseParams.Files.ContainsKey("AnswerFile") ? baseParams.Files["AnswerFile"] : null;
            var id = citSuggestion.Id;
                                   
            var fileManager = Container.Resolve<IFileManager>();
            var service = Container.Resolve<IDomainService<CitizenSuggestion>>();
            var serviceFiles = Container.Resolve<IDomainService<CitizenSuggestionFiles>>();

            if (file != null && id != 0)
            {
                var fileInfo = fileManager.SaveFile(file);

                var citSugFile = new CitizenSuggestionFiles
                {
                    CitizenSuggestion = service.Load(id),
                    DocumentDate = DateTime.Now,
                    DocumentFile = fileInfo,
                    isAnswer = true
                };

                serviceFiles.Save(citSugFile);
            }
        }

        public void SaveCommentAnswerFile(BaseParams baseParams, SuggestionComment suggestionComment)
        {
            var file = baseParams.Files.ContainsKey("CommentAnswerFile") ? baseParams.Files["CommentAnswerFile"] : null;
            var id = suggestionComment.Id;

            var fileManager = Container.Resolve<IFileManager>();
            var service = Container.Resolve<IDomainService<SuggestionComment>>();
            var serviceFiles = Container.Resolve<IDomainService<SuggestionCommentFiles>>();

            if (file != null && id != 0)
            {
                var fileInfo = fileManager.SaveFile(file);

                var citSugCommentFile = new SuggestionCommentFiles
                {
                    SuggestionComment = service.Load(id),
                    DocumentDate = DateTime.Now,
                    DocumentFile = fileInfo,
                    isAnswer = true
                };

                serviceFiles.Save(citSugCommentFile);
            }
        }
    }
}