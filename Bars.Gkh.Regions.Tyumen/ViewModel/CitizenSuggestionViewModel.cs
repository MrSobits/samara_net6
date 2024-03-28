namespace Bars.Gkh.Regions.Tyumen.ViewModel
{
    using System.Linq;
    using Authentification;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Security;
    using B4.Utils;
    using Entities;
    using Entities.Suggestion;
    using Utils.EntityExtensions;

    public class CitizenSuggestionViewModel : BaseViewModel<CitizenSuggestion>
    {
        public override IDataResult List(IDomainService<CitizenSuggestion> domainService, BaseParams baseParams)
        {
            var suggestionCommentDomain = this.Container.ResolveDomain<SuggestionComment>();
            var operatorContragentDomain = this.Container.ResolveDomain<OperatorContragent>();
            var operatorMunicipalityDomain = this.Container.ResolveDomain<OperatorMunicipality>();
            var operatorInspectorDomain = this.Container.ResolveDomain<OperatorInspector>();
            var zonalInspectorInspectionDomain = this.Container.ResolveDomain<ZonalInspectionInspector>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var userRoleDomain = this.Container.ResolveDomain<UserRole>();
            var activeUser = userManager.GetActiveUser();
            var userRole = string.Empty;
            if (activeUser != null)
            {
                var lastUserRole = userRoleDomain.GetAll().FirstOrDefault(x => x.User.Id == activeUser.Id);
                if (lastUserRole != null)
                {
                    userRole = lastUserRole.Role != null ? lastUserRole.Role.Name : string.Empty;
                }
            }

            long[] executorManOrgIds = null;
            long[] executorMuIds = null;
            long[] executorZonalInspIds = null;
            long[] executorCrFundIds = null;

            switch (userRole)
            {
                case "УК":
                     executorManOrgIds = operatorContragentDomain.GetAll()
                        .Where(x => x.Operator.User.Id == activeUser.Id)
                        .Select(x => x.Contragent.Id)
                        .ToArray();
                    break;
                case "МО":
                    executorMuIds = operatorMunicipalityDomain.GetAll()
                        .Where(x => x.Operator.User.Id == activeUser.Id)
                        .Select(x => x.Municipality.Id)
                        .ToArray();
                    break;
                case "ГЖИ":
                    var inspectorIds =
                        operatorInspectorDomain.GetAll()
                            .Where(x => x.Operator.User.Id == activeUser.Id)
                            .Select(x => x.Inspector.Id)
                            .ToArray();
                    executorZonalInspIds = zonalInspectorInspectionDomain.GetAll()
                        .Where(x => inspectorIds.Contains(x.Inspector.Id))
                        .Select(x => x.ZonalInspection.Id)
                        .ToArray();
                    break;
                case "Фонд КР":
                    executorCrFundIds = operatorContragentDomain.GetAll()
                        .Where(x => x.Operator.User.Id == activeUser.Id)
                        .Select(x => x.Contragent.Id)
                        .ToArray();
                    break;
            }

            try
            {
                var loadParams = this.GetLoadParam(baseParams);

                var allowedRoles = new[] {"Администратор", "Администратор2", "УК", "МО", "ГЖИ", "Фонд КР", "Оператор"};

                if (activeUser == null || !allowedRoles.Contains(userRole))
                {
                    return null;
                }

                var firstComment = suggestionCommentDomain.GetAll()
                    .Where(x => x.IsFirst)
                    .Select(x => new
                    {
                        x.CitizenSuggestion.Id,
                        x.Description,
                        ProblemPlace = x.ProblemPlace != null ? x.ProblemPlace.Name : null
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => new
                    {
                        y.First().Description,
                        y.First().ProblemPlace
                    });

                var comment = suggestionCommentDomain.GetAll()
                    .Where(
                        x =>
                            x.ExecutorManagingOrganization != null || x.ExecutorZonalInspection != null ||
                            x.ExecutorMunicipality != null || x.ExecutorCrFund != null)
                    .Select(x => new
                    {
                        sugId = x.CitizenSuggestion.Id,
                        x.Id,
                        x.CreationDate,
                        ExecutorManagingOrganization = x.ExecutorManagingOrganization != null ? x.ExecutorManagingOrganization.Contragent.Id : 0,
                        ExecutorZonalInspection = x.ExecutorZonalInspection != null ? x.ExecutorZonalInspection.Id : 0,
                        ExecutorMunicipality = x.ExecutorMunicipality != null ? x.ExecutorMunicipality.Id : 0,
                        ExecutorCrFund = x.ExecutorCrFund != null ? x.ExecutorCrFund.Contragent.Id : 0
                    });

                var lastComments = comment.AsEnumerable()
                    .GroupBy(x => x.sugId)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Id)
                        .Select(y => new
                        {
                            y.ExecutorManagingOrganization,
                            y.ExecutorZonalInspection,
                            y.ExecutorMunicipality,
                            y.ExecutorCrFund
                        }).FirstOrDefault());

                var data = domainService.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.Number,
                        x.ApplicantAddress,
                        x.ApplicantEmail,
                        x.ApplicantFio,
                        x.ApplicantPhone,
                        x.CreationDate,
                        x.RealityObject.Address,
                        x.Flat.RoomNum,
                        RoomAddress = x.RealityObject.Address + ", кв. " + x.Flat.RoomNum,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        Settlement = x.RealityObject.MoSettlement.Name,
                        RubricName = x.Rubric.Name,
                        x.AnswerText,
                        x.HasAnswer,
                        AllHasAnswer = false
                    })
                    .ToList()
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.Number,
                        x.ApplicantAddress,
                        x.ApplicantEmail,
                        x.ApplicantFio,
                        x.ApplicantPhone,
                        x.CreationDate,
                        Description =
                            firstComment.ContainsKey(x.Id) ? firstComment[x.Id].Description : null,
                        ProblemPlace =
                            firstComment.ContainsKey(x.Id) ? firstComment[x.Id].ProblemPlace : null,
                        x.Address,
                        x.RoomNum,
                        x.RoomAddress,
                        x.MunicipalityName,
                        x.Settlement,
                        x.RubricName,
                        x.AnswerText,
                        x.HasAnswer,
                        AllHasAnswer = false,
                        ExecutorManagingOrganization = lastComments.ContainsKey(x.Id) ? lastComments[x.Id].ExecutorManagingOrganization : 0,
                        ExecutorZonalInspection = lastComments.ContainsKey(x.Id) ? lastComments[x.Id].ExecutorZonalInspection : 0,
                        ExecutorMunicipality = lastComments.ContainsKey(x.Id) ? lastComments[x.Id].ExecutorMunicipality : 0,
                        ExecutorCrFund = lastComments.ContainsKey(x.Id) ? lastComments[x.Id].ExecutorCrFund : 0
                    })
                    .AsQueryable()
                    //.WhereIf(executorManOrgIds != null, x => executorManOrgIds.Contains(x.ExecutorManagingOrganization))
                    //.WhereIf(executorMuIds != null, x => executorMuIds.Contains(x.ExecutorMunicipality))
                    //.WhereIf(executorZonalInspIds != null, x => executorZonalInspIds.Contains(x.ExecutorZonalInspection))
                    //.WhereIf(executorCrFundIds != null, x => executorCrFundIds.Contains(x.ExecutorCrFund))
                    .Filter(loadParams, this.Container)
                    .Order(loadParams);

                var totalCount = data.Count();
                var list = data.Paging(loadParams).ToList();

                var suggIds = list.Select(x => x.Id).ToList();
                var suggestionAnswers = suggestionCommentDomain.GetAll()
                    .Where(x => suggIds.Contains(x.CitizenSuggestion.Id))
                    .Select(x => new
                    {
                        x.CitizenSuggestion.Id,
                        x.Answer
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.All(x => !x.Answer.IsEmpty()));

                list = list
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.Number,
                        x.ApplicantAddress,
                        x.ApplicantEmail,
                        x.ApplicantFio,
                        x.ApplicantPhone,
                        CreationDate = x.CreationDate.Date,
                        x.Description,
                        x.ProblemPlace,
                        x.Address,
                        x.RoomNum,
                        x.RoomAddress,
                        x.MunicipalityName,
                        x.Settlement,
                        x.RubricName,
                        x.AnswerText,
                        x.HasAnswer,
                        AllHasAnswer =
                            !x.AnswerText.IsEmpty() &&
                            (!suggestionAnswers.ContainsKey(x.Id) || suggestionAnswers.Get(x.Id)),
                        x.ExecutorManagingOrganization,
                        x.ExecutorZonalInspection,
                        x.ExecutorMunicipality,
                        x.ExecutorCrFund

                    }).ToList();

                return new ListDataResult(list, totalCount);
            }
            finally
            {
                this.Container.Release(suggestionCommentDomain);
                this.Container.Release(operatorContragentDomain);
                this.Container.Release(operatorMunicipalityDomain);
                this.Container.Release(operatorInspectorDomain);
                this.Container.Release(zonalInspectorInspectionDomain);
                this.Container.Release(userManager);
                this.Container.Release(userRoleDomain);
            }
        }

        public override IDataResult Get(IDomainService<CitizenSuggestion> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var value = domainService.Get(id);

            var executorType = value.GetCurrentExecutorType();

            var result = new
            {
                value.Id,
                value.Number,
                value.ApplicantAddress,
                value.ApplicantEmail,
                value.ApplicantFio,
                value.ApplicantPhone,
                CreationDate = value.CreationDate.Date,
                value.Description,
                value.ProblemPlace,
                RealityObject = new
                {
                    Id = (long?) value.RealityObject.Id,
                    value.RealityObject.Address,
                    Municipality = value.RealityObject.Municipality.Name
                },
                RoomAddress =
                    value.RealityObject.Address + (value.Flat != null ? ", кв. " + value.Flat.RoomNum : string.Empty),
                value.Rubric,
                value.State,
                value.AnswerText,
                Deadline = value.Deadline.ToDateTime().Date,
                value.AnswerDate,
                ExecutorType = executorType,
                Executor = value.GetExecutor(executorType),
                MessageSubject = value.MessageSubject != null ? value.MessageSubject.Name : "",
                CategoryPosts = value.MessageSubject != null ? value.MessageSubject.CategoryPosts.Name : ""
            };

            return new BaseDataResult(result);
        }
    }
}