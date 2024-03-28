namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Security;
    using B4.Utils;

    using Authentification;
    using Entities;
    using Entities.Suggestion;
    using Utils.EntityExtensions;

    public class CitizenSuggestionViewModel : BaseViewModel<CitizenSuggestion>
    {
        public override IDataResult List(IDomainService<CitizenSuggestion> domainService, BaseParams baseParams)
        {
            var suggestionCommentDomain = Container.ResolveDomain<SuggestionComment>();
            var operatorContragentDomain = Container.ResolveDomain<OperatorContragent>();
            var operatorMunicipalityDomain = Container.ResolveDomain<OperatorMunicipality>();
            var operatorInspectorDomain = Container.ResolveDomain<OperatorInspector>();
            var zonalInspectorInspectionDomain = Container.ResolveDomain<ZonalInspectionInspector>();
            var userManager = Container.Resolve<IGkhUserManager>();
            var userRoleDomain = Container.ResolveDomain<UserRole>();
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
                var loadParams = GetLoadParam(baseParams);

                var allowedRoles = new[] { "Администратор", "Администратор1", "Администратор2", "МО", "ГЖИ", "Фонд КР", "Регфонд/ФРО" };

                if (activeUser == null || !allowedRoles.Contains(userRole))
                {
                    return null;
                }

                var data = domainService.GetAll()
                    //.WhereIf(executorManOrgIds != null, x => executorManOrgIds.Contains(x.ExecutorManagingOrganization.Contragent.Id))
                    //.WhereIf(executorMuIds != null, x => executorMuIds.Contains(x.ExecutorMunicipality.Id))
                    //.WhereIf(executorZonalInspIds != null, x => executorZonalInspIds.Contains(x.ExecutorZonalInspection.Id))
                    //.WhereIf(executorCrFundIds != null, x => executorCrFundIds.Contains(x.ExecutorCrFund.Contragent.Id))
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
                        x.Description,
                        ProblemPlace = x.ProblemPlace != null ? x.ProblemPlace.Name : null,
                        SugTypeProblem = x.SugTypeProblem != null ? x.SugTypeProblem.Name : null,
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
                    .Filter(loadParams, Container)
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

                list = list.Select(x => new
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
                    x.SugTypeProblem,
                    x.Address,                    
                    x.RoomNum,
                    x.RoomAddress,
                    x.MunicipalityName,
                    x.Settlement,
                    x.RubricName,
                    x.AnswerText,
                    x.HasAnswer,
                    AllHasAnswer = !x.AnswerText.IsEmpty() && (!suggestionAnswers.ContainsKey(x.Id) || suggestionAnswers.Get(x.Id))
                }).ToList();

                return new ListDataResult(list, totalCount);
            }
            finally
            {
                Container.Release(suggestionCommentDomain);
                Container.Release(operatorContragentDomain);
                Container.Release(operatorMunicipalityDomain);
                Container.Release(operatorInspectorDomain);
                Container.Release(zonalInspectorInspectionDomain);
                Container.Release(userManager);
                Container.Release(userRoleDomain);
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
                    Id = (long?) value.RealityObject?.Id,
                    value.RealityObject?.Address,
                    Municipality = value.RealityObject?.Municipality.Name
                },
                RoomAddress = value.RealityObject?.Address + (value.Flat != null ? ", кв. " + value.Flat.RoomNum : string.Empty),
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