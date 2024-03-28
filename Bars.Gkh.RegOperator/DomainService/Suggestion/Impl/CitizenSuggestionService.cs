namespace Bars.Gkh.RegOperator.Domain.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Castle.Windsor;

    public class CitizenSuggestionService : ICitizenSuggestionService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RegOperator> RegOpDomain { get; set; }

        public IDomainService<ContragentContact> ContragentContactDomain { get; set; }

        public IDataResult ListExecutor(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var executorType = baseParams.Params.GetAs<ExecutorType>("executorType");

            switch (executorType)
            {
                case ExecutorType.Mo:
                {
                    var manOrgDomain = this.Container.Resolve<IDomainService<ManagingOrganization>>();
                    try
                    {
                        var data = manOrgDomain.GetAll()
                            .Select(x => new {x.Id, x.Contragent.Name})
                            .Filter(loadParams, this.Container);

                        var totalCount = data.Count();

                        return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                    }
                    finally
                    {
                        this.Container.Release(manOrgDomain);
                    }
                    
                }
                case ExecutorType.Mu:
                {
                    var localGovermentDomain = this.Container.Resolve<IDomainService<LocalGovernmentMunicipality>>();
                    var municipalityDomain = this.Container.Resolve<IDomainService<Municipality>>();
                    try
                    {
                        var localGovMun = localGovermentDomain.GetAll()
                            .Where(x => x.LocalGovernment != null && x.Municipality != null)
                            .Select(x => new {x.Municipality.Id, LocalGov = x.LocalGovernment})
                            .AsEnumerable()
                            .GroupBy(x => x.Id)
                            .ToDictionary(x => x.Key, y => y.First().LocalGov);

                        var names = municipalityDomain.GetAll()
                            .ToDictionary(x => x.Id, y => y.Name);

                        foreach (var newName in localGovMun)
                        {
                            if (names.ContainsKey(newName.Key))
                            {
                                names[newName.Key] = newName.Value.Contragent.Name;
                            }
                        }

                        var totalCount = names.Count();

                        var data = names.Select(x => new {Id = x.Key, Name = x.Value})
                            .AsQueryable()
                            .Filter(loadParams, this.Container);

                        return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                    }
                    finally
                    {
                        this.Container.Release(localGovermentDomain);
                        this.Container.Release(municipalityDomain);
                    }

                }
                case ExecutorType.Gji:
                {
                    var zonalInspectionDomain = this.Container.Resolve<IDomainService<ZonalInspection>>();

                    try
                    {
                        var data = zonalInspectionDomain.GetAll()
                            .Select(x => new {x.Id, x.Name})
                            .Filter(loadParams, this.Container);

                        var totalCount = data.Count();

                        return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                    }
                    finally
                    {
                        this.Container.Release(zonalInspectionDomain);
                    }

                }
                case ExecutorType.CrFund:
                {
                    var regOperator = this.RegOpDomain.GetAll().FirstOrDefault();
                    if (regOperator != null)
                    {
                        var data = this.ContragentContactDomain.GetAll()
                            .Where(x => x.Contragent.Id == regOperator.Contragent.Id)
                            .Select(x => new {x.Id, Name = string.Format("{0}, {1}", x.FullName, x.Position.Name)})
                            .Filter(loadParams, this.Container);

                        var totalCount = data.Count();

                        return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                    }

                    return BaseDataResult.Error("Не указан ни 1 региональный оператор");
                }
            }

            return new ListDataResult();
        }

        public void SaveAnswerFile(BaseParams baseParams, CitizenSuggestion citSuggestion)
        {
            var file = baseParams.Files.ContainsKey("AnswerFile") ? baseParams.Files["AnswerFile"] : null;
            var id = citSuggestion.Id;

            var fileManager = this.Container.Resolve<IFileManager>();
            var service = this.Container.Resolve<IDomainService<CitizenSuggestion>>();
            var serviceFiles = this.Container.Resolve<IDomainService<CitizenSuggestionFiles>>();

            if (file != null && id != 0)
            {
                var fileInfo = fileManager.SaveFile(file);

                var citSugFile = new CitizenSuggestionFiles
                {
                    CitizenSuggestion = service.Load(id),
                    DocumentDate = DateTime.Now,
                    DocumentFile = fileInfo,
                    Name = fileInfo.Name,
                    isAnswer = true
                };

                serviceFiles.Save(citSugFile);
            }
        }

        public void SaveCommentAnswerFile(BaseParams baseParams, SuggestionComment suggestionComment)
        {
            var file = baseParams.Files.ContainsKey("CommentAnswerFile") ? baseParams.Files["CommentAnswerFile"] : null;
            var id = suggestionComment.Id;

            var fileManager = this.Container.Resolve<IFileManager>();
            var service = this.Container.Resolve<IDomainService<SuggestionComment>>();
            var serviceFiles = this.Container.Resolve<IDomainService<SuggestionCommentFiles>>();

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