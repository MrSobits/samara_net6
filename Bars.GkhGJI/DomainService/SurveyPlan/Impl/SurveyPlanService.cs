namespace Bars.GkhGji.DomainService.SurveyPlan.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;
    using Gkh.Domain;
    using Gkh.DomainService;
    using Gkh.Enums;
    using Entities;
    using Entities.SurveyPlan;
    using Enums;
    using Tasks.SurveyPlan;

    using Castle.Windsor;

	/// <summary>
	/// Сервис для работы с План проверки
	/// </summary>
    public class SurveyPlanService : ISurveyPlanService
    {
        private readonly IWindsorContainer _container;
	    private readonly IDomainService<ContragentAuditPurpose> _contragentAuditPurposeDomain;
	    private readonly IDomainService<SurveyPlanContragent> _surveyPlanContragentDomain;
        private readonly IDomainService<BaseJurPerson> _baseJurPersonDomain;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="contragentAuditPurposeDomain">Цели контрагента</param>
        /// <param name="surveyPlanContragentDomain">Контрагент плана проверки</param>
        /// <param name="baseJurPersonDomain">Основание плановой проверки юр. лиц ГЖИ</param>
        public SurveyPlanService(IWindsorContainer container, 
                                 IDomainService<ContragentAuditPurpose> contragentAuditPurposeDomain, 
                                 IDomainService<SurveyPlanContragent> surveyPlanContragentDomain,
                                 IDomainService<BaseJurPerson> baseJurPersonDomain )
        {
            _container = container;
            _contragentAuditPurposeDomain = contragentAuditPurposeDomain;
            _surveyPlanContragentDomain = surveyPlanContragentDomain;
            _baseJurPersonDomain = baseJurPersonDomain;

        }

		/// <summary>
		/// Включить контрагентов в план проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public IDataResult AddCandidates(BaseParams baseParams)
        {
            var recordIds = baseParams.Params.GetAs("recordIds", string.Empty).ToLongArray();
            if (recordIds.Length == 0)
            {
                return BaseDataResult.Error("Не выбраны записи для добавления");
            }

            var surveyPlanDomain = _container.ResolveDomain<SurveyPlan>();
            var candidateDomain = _container.ResolveDomain<SurveyPlanCandidate>();
            var contragentDomain = _container.ResolveDomain<SurveyPlanContragent>();

            try
            {
                var surveyPlanId = baseParams.Params.GetAsId("surveyPlanId");
                var surveyPlan = surveyPlanDomain.Get(surveyPlanId);
                if (surveyPlan == null)
                {
                    return BaseDataResult.Error("Передан неверный идентификатор плана");
                }

                _container.InTransaction(
                    () =>
                        {
                            var candidates = candidateDomain.GetAll().Where(x => recordIds.Contains(x.Id));
                            foreach (var candidate in candidates)
                            {
	                            var contragent = contragentDomain.GetAll()
		                            .Where(x => x.Contragent.Id == candidate.Contragent.Id)
		                            .Where(x => x.AuditPurpose.Id == candidate.AuditPurpose.Id)
		                            .Where(x => x.SurveyPlan.Id == surveyPlan.Id)
		                            .FirstOrDefault() ?? new SurveyPlanContragent();

                                contragent.AuditPurpose = candidate.AuditPurpose;
                                contragent.Contragent = candidate.Contragent;
                                contragent.PlanYear = candidate.PlanYear;
                                contragent.PlanMonth = candidate.PlanMonth;
                                contragent.Reason = candidate.Reason;
                                contragent.SurveyPlan = surveyPlan;
                                contragent.FromLastAuditDate = candidate.FromLastAuditDate;

                                contragentDomain.SaveOrUpdate(contragent);
                            }
                        });

                return new BaseDataResult();
            }
            finally
            {
                _container.Release(surveyPlanDomain);
                _container.Release(candidateDomain);
                _container.Release(contragentDomain);
            }
        }

		/// <summary>
		/// Создать кандидатов для плана проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Описатель задачи</returns>
        public IDataResult CreateCandidates(BaseParams baseParams)
        {
            var taskManager = _container.Resolve<ITaskManager>();
            try
            {
                return taskManager.CreateTasks(new CreateSurveyPlanCandidatesTaskProvider(_container), baseParams);
            }
            finally
            {
                _container.Release(taskManager);
            }
        }

		/// <summary>
		/// Создать/обновить план проверки
		/// </summary>
		/// <param name="contragent">Контрагент плана проверки</param>
        public void CreateOrUpdateSurvey(SurveyPlanContragent contragent)
        {
            var inspDomain = _container.ResolveDomain<BaseJurPerson>();
            var contragentService = _container.Resolve<IContragentService>();
            try
            {
                CreateOrUpdateSurveyInternal(contragentService, inspDomain, contragent);
            }
            finally
            {
                _container.Release(inspDomain);
                _container.Release(contragentService);
            }
        }

	    /// <summary>
	    /// Создать план проверки
	    /// </summary>
	    /// <param name="surveyPlan">План проверки</param>
	    public void CreateSurveys(SurveyPlan surveyPlan)
	    {
	        var contragets = GetContragents(surveyPlan);

            foreach (var contragent in contragets)
	        {
                this.CreateOrUpdateSurvey(contragent);
	        }
	    }

	    private void CreateOrUpdateSurveyInternal(
            IContragentService contragentService,
            IDomainService<BaseJurPerson> inspDomain,
            SurveyPlanContragent contragent)
        {
            var contragentDomain = _container.ResolveDomain<SurveyPlanContragent>();
            var insp = contragent.Inspection ?? new BaseJurPerson();
            insp.TypeBase = TypeBase.PlanJuridicalPerson;
            insp.TypeFact = TypeFactInspection.NotSet;
            insp.Plan = contragent.SurveyPlan.PlanJurPerson;
            insp.TypeJurPerson = contragentService.GetTypeJurPerson(contragent.Contragent);
            insp.Contragent = contragent.Contragent;
            insp.DateStart = new DateTime(
                contragent.SurveyPlan.PlanJurPerson.DateStart.Return(x => x != null ? x.Value.Year : 0),
                (int)contragent.PlanMonth,
                1);

            switch (contragent.Contragent.TypeEntrepreneurship)
            {
                case TypeEntrepreneurship.Micro:
                    insp.CountHours = 15;
                    insp.CountDays = null;
                    break;
                case TypeEntrepreneurship.Small:
                    insp.CountHours = 50;
                    insp.CountDays = null;
                    break;
                default:
                    insp.CountHours = null;
                    insp.CountDays = 20;
                    break;
            }

            insp.TypeBaseJuralPerson = contragent.FromLastAuditDate ? TypeBaseJurPerson.LastWorkAfter3Years : TypeBaseJurPerson.NotSet;
            if (contragent.FromLastAuditDate)
            {
                if (contragent.Inspection != null)
                {
                    insp.TypeBaseJuralPerson = contragent.Inspection.TypeBaseJuralPerson;
                }
            }
            else
            {
                insp.AnotherReasons = contragent.Reason;
            }

            var formExit = contragentDomain.GetAll()
                .Where(x => x.SurveyPlan.Id == contragent.SurveyPlan.Id)
                .Where(x => x.AuditPurpose.Code == "05" || x.AuditPurpose.Code == "01")
                .Where(x => x.Contragent.Id == contragent.Contragent.Id).Count();

            if (formExit != 0)
            {
                insp.TypeForm = TypeFormInspection.Exit;
            }
            else
            {
                switch (contragent.AuditPurpose.Code)
                {
                    case "01":
                    case "05":
                        insp.TypeForm = TypeFormInspection.Exit;
                        break;
                    case "02":
                    case "03":
                    case "04":
                        insp.TypeForm = TypeFormInspection.Documentary;
                        break;
                }
            }

            inspDomain.SaveOrUpdate(insp);
            contragent.Inspection = insp;
        }

        /// <summary>
        /// Получение списка контрагентов
        /// </summary>
        /// <param name="surveyPlan">План проверки</param>
        /// <returns></returns>
        public List<SurveyPlanContragent> GetContragents( SurveyPlan surveyPlan )
	    {
            var contragets = new List<SurveyPlanContragent>();
            var groups = _surveyPlanContragentDomain.GetAll()
                       .Where(x => x.SurveyPlan.Id == surveyPlan.Id && !x.IsExcluded)
                       .GroupBy(x => x.Contragent.Id, x => x);

            foreach (var group in groups)
            {
                var contragentAuditPurposes = _contragentAuditPurposeDomain.GetAll()
                    .Where(x => x.Contragent.Id == group.Key)
                    .Where(x => x.LastInspDate != null)
                    .AsEnumerable()
                    .Where(x => group.Any(y => y.AuditPurpose.Id == x.AuditPurpose.Id))
                    .OrderBy(x => x.LastInspDate)
                    .ToArray();

                if (contragentAuditPurposes.Length == 0)
                {
                    var contraget = group.OrderBy(x => x.AuditPurpose.Code)
                        .FirstOrDefault(x => x.AuditPurpose.Code == "05" || x.AuditPurpose.Code == "01");
                    if (contraget != null)
                    {
                        contragets.Add(contraget);
                    }
                    else
                    {
                        contraget = group.First();
                        contragets.Add(contraget);
                    }
                }
                else if (contragentAuditPurposes.Length == 1)
                {
                    var contraget = group.First(x => contragentAuditPurposes.Any(y => y.AuditPurpose.Id == x.AuditPurpose.Id));
                    contragets.Add(contraget);
                }
                else
                {
                    var contraget =
                        group.Where(x => contragentAuditPurposes.Any(y => y.AuditPurpose.Id == x.AuditPurpose.Id))
                            .OrderBy(x => x.PlanYear)
                            .ThenBy(x => x.PlanMonth).Last();
                    contragets.Add(contraget);
                }
            }
            return contragets;
	    }
    }
}