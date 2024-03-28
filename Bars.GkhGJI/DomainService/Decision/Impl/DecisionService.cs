namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService.Contracts;
    using Gkh.Authentification;
    using Entities;
    using Enums;
    using Castle.Windsor;
    using Utils = Bars.GkhGji.Utils.Utils;
    using Bars.GkhGji.Entities.Dict;
    using System.Collections.Generic;
    using Bars.Gkh.Domain;

    /// <summary>
    /// Сервис для Приказ
    /// </summary>
    public class DecisionService : IDecisionService
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

        public IDomainService<DecisionControlMeasures> DecisionControlMeasuresDomain { get; set; }

        public IDomainService<Decision> DecisionDomain { get; set; }

        public IDomainService<ControlActivity> ControlActivityDomain { get; set; }

        public IDomainService<DecisionAdminRegulation> DecisionAdminRegulationDomain { get; set; }
        public IDomainService<DecisionVerificationSubject> DecisionVerificationSubjectDomain { get; set; }
        public IDomainService<SurveySubject> SurveySubjectDomain { get; set; }

		public IDomainService<TypeSurveyGji> TypeSurveyDomain { get; set; }

        public IDomainService<TypeSurveyKindInspGji> TypeSurveyKindDomain { get; set; }
        /// <summary>
        /// Домен сервис для "Цель проверки"
        /// </summary>
        public IDomainService<TypeSurveyGoalInspGji> TypeSurveyPurposeDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Задачи проверки"
        /// </summary>
        public IDomainService<TypeSurveyTaskInspGji> TypeSurveyTaskInspGjiDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Правовое основание проведения проверки"
        /// </summary>
        public IDomainService<TypeSurveyInspFoundationGji> TypeSurveyInspFoundationGjiDomain { get; set; }

        /// <summary>
        /// Домен сервис для "НПА проверки"
        /// </summary>
        public IDomainService<TypeSurveyInspFoundationCheckGji> TypeSurveyInspFoundationCheckGjiDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Административные регламенты"
        /// </summary>
        public IDomainService<TypeSurveyAdminRegulationGji> TypeSurveyAdminRegulationGjiDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Предоставляемый документ Типа обследования"
        /// </summary>
        public IDomainService<TypeSurveyProvidedDocumentGji> TypeSurveyProvidedDocumentGjiDomain { get; set; }

        /// <summary>
        /// Получить информацию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var info = GetInfo(documentId);
                return new BaseDataResult(new { inspectorNames = info.InspectorNames, inspectorIds = info.InspectorIds, baseName = info.BaseName, planName = info.PlanName });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }

		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult ListView(BaseParams baseParams)
        {
            var docGjiCildrenServ = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");


                var data = GetViewList()
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.DateEnd,
                        x.DateStart,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.ERPID,
                        x.TypeBase,
                        x.IssuedDecision,
                        KindCheck = x.KindCheckName,
                        x.ContragentName,
                        x.TypeDisposal,
                        MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : "",
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        MunicipalityId = x.MunicipalityId??null,
                        x.RealityObjectCount,
                        x.InspectorNames,
                        x.InspectionId,
                        x.TypeDocumentGji,
                        x.TypeAgreementProsecutor,
                        x.KindKNDGJI,
                        HasActSurvey = docGjiCildrenServ.GetAll().Any(y => y.Parent.Id == x.Id && y.Children.TypeDocumentGji == TypeDocumentGji.ActSurvey)

                    })
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                Container.Release(docGjiCildrenServ);
            }

        }

        public IDataResult AddDisposalControlMeasures(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var controlMeasuresIds =
                    baseParams.Params.ContainsKey("controlMeasuresIds")
                        ? baseParams.Params["controlMeasuresIds"].ToString()
                        : string.Empty;

                var disposal = DecisionDomain.Get(documentId);

                if (!string.IsNullOrEmpty(controlMeasuresIds))
                {
                    // список уже добавленных мероприятий по контролю
                    var listTypes =
                        this.DecisionControlMeasuresDomain
                            .GetAll()
                            .Where(x => x.Decision.Id == documentId)
                            .Select(x => x.ControlActivity.Id)
                            .ToList();

                    foreach (var controlMeasureIds in controlMeasuresIds.Split(','))
                    {
                        var newId = controlMeasureIds.ToLong();

                        if (!listTypes.Contains(newId))
                        {
                            string controlActivityName = ControlActivityDomain.Get(newId).Name;
                            var newObj = new DecisionControlMeasures
                            {
                                Decision = new GkhGji.Entities.Decision { Id = documentId },
                                ControlActivity = new ControlActivity { Id = newId },
                                DateStart = disposal.DateStart,
                                DateEnd = disposal.DateEnd,
                                Description = controlActivityName
                            };

                            this.DecisionControlMeasuresDomain.Save(newObj);
                        }
                    }
                }
                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult AddAdminRegulations(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var ids = baseParams.Params.GetAs<long[]>("ids") ?? new long[0];
            var result = this.AddAdminRegulations(documentId, ids);

            return result;
        }

        private IDataResult AddAdminRegulations(long documentId, long[] ids)
        {
            try
            {
                var extingIds = this.DecisionAdminRegulationDomain.GetAll()
                    .Where(x => x.Decision.Id == documentId)
                    .Select(x => x.AdminRegulation.Id)
                    .ToArray();

                foreach (var id in ids.Distinct())
                {
                    if (!extingIds.Contains(id))
                    {
                        var newObj = new DecisionAdminRegulation()
                        {
                            Decision = new Decision { Id = documentId },
                            AdminRegulation = new Gkh.Entities.Dicts.NormativeDoc { Id = id }
                        };

                        this.DecisionAdminRegulationDomain.Save(newObj);
                    }
                }
                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        /// <summary>
		/// Добавить предметы проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult AddSurveySubjects(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var ids = baseParams.Params.GetAs<long[]>("ids") ?? new long[0];
            var result = this.AddSurveySubjects(documentId, ids);

            return result;
        }

        /// <summary>
        /// Добавить предметы проверки
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="ids">Идентификаторы новых записей</param>
        /// <returns>Результат выполнения запроса</returns>
        private IDataResult AddSurveySubjects(long documentId, long[] ids)
        {
            try
            {
                var disposal = this.DecisionDomain.Get(documentId);
                if (disposal == null)
                {
                    return new BaseDataResult(false, "Не найден документ решения");
                }

                this.Container.InTransaction(() =>
                {
                    var extingIds = this.DecisionVerificationSubjectDomain.GetAll()
                        .Where(x => x.Decision.Id == documentId)
                        .Select(x => x.SurveySubject.Id)
                        .ToArray();

                    var uniqueIds = new List<long>();

                    foreach (var id in ids)
                    {
                        if (!extingIds.Contains(id))
                        {
                            var newObj = new DecisionVerificationSubject
                            {
                                Decision = new Decision { Id = documentId },
                                SurveySubject = new SurveySubject() { Id = id }
                            };

                            this.DecisionVerificationSubjectDomain.Save(newObj);
                            uniqueIds.Add(id);
                        }
                    }

                    var codes = this.SurveySubjectDomain.GetAll()
                        .Where(x => uniqueIds.Contains(x.Id))
                        .Select(x => x.Code)
                        .ToArray();
                   
                });

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            catch (Exception)
            {
                return new BaseDataResult { Success = false, Message = "Произошла ошибка при добавлении предметов проверки" };
            }
        }

        public IDataResult AddExperts(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var expertIds = baseParams.Params.ContainsKey("expertIds") ? baseParams.Params["expertIds"].ToString() : "";

                // в этом списке будут id экспертов, которые уже связаны с этим распоряжением
                // (чтобы недобавлять несколько одинаковых экспертов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var serviceExperts = Container.Resolve<IDomainService<DecisionExpert>>();

                listIds.AddRange(serviceExperts.GetAll()
                                    .Where(x => x.Decision.Id == documentId)
                                    .Select(x => x.Expert.Id)
                                    .Distinct()
                                    .ToList());

                foreach (var id in expertIds.Split(','))
                {
                    var newId = id.ToLong();

                    // Если среди существующих экспертов уже есть такой эксперт то пролетаем мимо
                    if (listIds.Contains(newId))
                        continue;

                    // Если такого эксперта еще нет то добавляем
                    var newObj = new DecisionExpert
                    {
                        Decision = new Decision { Id = documentId },
                        Expert = new ExpertGji { Id = newId }
                    };

                    serviceExperts.Save(newObj);
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult AddControlLists(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var contrlistIds = baseParams.Params.ContainsKey("contrlistIds") ? baseParams.Params["contrlistIds"].ToString() : "";

                // в этом списке будут id экспертов, которые уже связаны с этим распоряжением
                // (чтобы недобавлять несколько одинаковых экспертов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var serviceControlLists = Container.Resolve<IDomainService<DecisionControlList>>();

                listIds.AddRange(serviceControlLists.GetAll()
                                    .Where(x => x.Decision.Id == documentId)
                                    .Select(x => x.ControlList.Id)
                                    .Distinct()
                                    .ToList());

                foreach (var id in contrlistIds.Split(','))
                {
                    var newId = id.ToLong();

                    // Если среди существующих экспертов уже есть такой эксперт то пролетаем мимо
                    if (listIds.Contains(newId))
                        continue;

                    // Если такого эксперта еще нет то добавляем
                    var newObj = new DecisionControlList
                    {
                        Decision = new Decision { Id = documentId },
                        ControlList = new ControlList { Id = newId }
                    };

                    serviceControlLists.Save(newObj);
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult AddInspectionReasons(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var inspreasonIds = baseParams.Params.ContainsKey("inspreasonIds") ? baseParams.Params["inspreasonIds"].ToString() : "";

                // в этом списке будут id экспертов, которые уже связаны с этим распоряжением
                // (чтобы недобавлять несколько одинаковых экспертов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var serviceInspectionReasons = Container.Resolve<IDomainService<DecisionInspectionReason>>();
                var decision = DecisionDomain.Get(documentId);

                listIds.AddRange(serviceInspectionReasons.GetAll()
                                    .Where(x => x.Decision.Id == documentId)
                                    .Select(x => x.InspectionReason.Id)
                                    .Distinct()
                                    .ToList());

                foreach (var id in inspreasonIds.Split(','))
                {
                    var newId = id.ToLong();

                    // Если среди существующих экспертов уже есть такой эксперт то пролетаем мимо
                    if (listIds.Contains(newId))
                        continue;

                    // Если такого эксперта еще нет то добавляем
                    var newObj = new DecisionInspectionReason
                    {
                        Decision = decision,
                        InspectionReason = new InspectionReason { Id = newId }
                    };

                    serviceInspectionReasons.Save(newObj);
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        /// <summary>
        /// Получить список пустых инспекций
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ListNullInspection(BaseParams baseParams)
        {
            var serviceDecision = Container.Resolve<IDomainService<Decision>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                /*
                 * В качестве фильтров приходят следующие параметры
                 * dateStart - Необходимо получить документы больше даты начала
                 * dateEnd - Необходимо получить документы меньше даты окончания
                 * realityObjectId - Необходимо получить документы по дому
                 */

                var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
                var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);


                var data = serviceDecision.GetAll()
                    .Where(x => x.TypeDisposal == TypeDisposalGji.NullInspection)
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .Select(x => new
                        {
                            x.Id,
                            x.State,
                            IssuedDisposal = x.IssuedDisposal.Fio,
                            x.DocumentNum,
                            x.DocumentNumber,
                            x.DocumentDate
                        })
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                Container.Release(serviceDecision);
            }
        }

		/// <summary>
		/// Получить список из вью
		/// </summary>
		/// <returns>Модифицированных запрос</returns>
        public virtual IQueryable<ViewDecision> GetViewList()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var docGjiInspectorService = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var decisionService = Container.Resolve<IDomainService<Decision>>();
            var insRealObjService = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var serviceViewDecision = Container.Resolve<IDomainService<ViewDecision>>();

            try
            {
                var inspectorList = userManager.GetInspectorIds();
                var municipalityList = userManager.GetMunicipalityIds();
                inspectorList.Clear();// убираем проверки на инспекторов
                return serviceViewDecision.GetAll()
                    .WhereIf(inspectorList.Count > 0, x => docGjiInspectorService.GetAll().Any(y => x.Id == y.DocumentGji.Id && inspectorList.Contains(y.Inspector.Id) && y.DocumentGji.TypeDocumentGji == TypeDocumentGji.Decision)
                                                       || decisionService.GetAll().Any(y => x.Id == y.Id && inspectorList.Contains(y.IssuedDisposal.Id)))
                    .WhereIf(municipalityList.Count > 0, x => insRealObjService.GetAll().Any(y => y.Inspection.Id == x.InspectionId && municipalityList.Contains(y.RealityObject.Municipality.Id)));
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(docGjiInspectorService);
                Container.Release(decisionService);
                Container.Release(insRealObjService);
                Container.Release(serviceViewDecision);
            }
        }

		/// <summary>
		/// Получить информацию о приказе
		/// </summary>
		/// <param name="documentId">Идентификатор документа</param>
		/// <returns>Результат выполнения запроса</returns>
        public DecisionInfo GetInfo(long documentId)
        {
            var serviceDocInspector = Container.Resolve<IDocumentGjiInspectorService>();
            var serviceDecision = Container.Resolve<IDomainService<Decision>>();
            var serviceChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var baseName = string.Empty;
                var planName = string.Empty;

                // Сначала пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var dataInspectors =
                    serviceDocInspector.GetInspectorsByDocumentId(documentId)
                        .Select(x => new {InspectorId = x.Inspector.Id, x.Inspector.Fio})
                        .ToList();

                foreach (var item in dataInspectors)
                {
                    if (!string.IsNullOrEmpty(inspectorNames))
                    {
                        inspectorNames += ", ";
                    }

                    inspectorNames += item.Fio;

                    if (!string.IsNullOrEmpty(inspectorIds))
                    {
                        inspectorIds += ", ";
                    }

                    inspectorIds += item.InspectorId.ToString();
                }

                if (documentId > 0)
                {
                    #region Выставляем Доп свойства

                    // Получаем распоряжение
                    var decision = serviceDecision.GetAll().FirstOrDefault(x => x.Id == documentId);

                    if (decision.Inspection != null)
                    {
                        switch (decision.Inspection.TypeBase)
                        {
                            case TypeBase.PlanJuridicalPerson:
                                {
                                    GetInfoJurPerson(ref baseName, ref planName, decision.Inspection.Id);
                                }
                                break;

                            case TypeBase.DisposalHead:
                                {
                                    GetInfoDispHead(ref baseName, ref planName, decision.Inspection.Id);
                                }
                                break;

                            case TypeBase.ProsecutorsClaim:
                                {
                                    GetInfoProsClaim(ref baseName, ref planName, decision.Inspection.Id);
                                }
                                break;

                            case TypeBase.Inspection:
                                {
                                    GetInfoInsCheck(ref baseName, ref planName, decision.Inspection.Id);
                                }
                                break;

                            case TypeBase.CitizenStatement:
                                {
                                    // если распоряжение создано на основе обращения граждан
                                    GetInfoCitizenStatement(ref baseName, ref planName, decision.Inspection.Id);
                                }

                                break;

                            case TypeBase.ActivityTsj:
                                baseName = "Проверка деятельности ТСЖ";
                                break;

                            case TypeBase.HeatingSeason:
                                baseName = "Подготовка к отопительному сезону";
                                break;

                            case TypeBase.PlanAction:
                                {
                                    GetInfoPlanAction(ref baseName, ref planName, decision.Inspection.Id);
                                }
                                break;

							case TypeBase.LicenseApplicants:
								{
									GetInfoLicenseApplicants(ref baseName, ref planName, decision.Inspection.Id);
								}
								break;
						}

                        if (decision.TypeDisposal == TypeDisposalGji.DocumentGji)
                        {
                            baseName = "Проверка исполнения предписаний";
                            planName = string.Empty;

                            var data = serviceChildren.GetAll()
                                .Where(x => x.Children.Id == decision.Id)
                                .Select(x => new
                                {
                                    x.Parent.DocumentDate,
                                    x.Parent.DocumentNumber,
                                    x.Parent.TypeDocumentGji
                                })
                                .ToList();

                            foreach (var item in data)
                            {
                                var docName = Utils.GetDocumentName(item.TypeDocumentGji);

                                if (!string.IsNullOrEmpty(planName)) planName += ", ";

                                planName += string.Format(
                                    "{0} №{1} от {2}",
                                    docName,
                                    item.DocumentNumber,
                                    item.DocumentDate.ToDateTime().ToShortDateString());
                            }
                        }
                        else if (decision.TypeDisposal == TypeDisposalGji.Base || decision.TypeDisposal == TypeDisposalGji.Licensing)
                        {
                            if (serviceChildren.GetAll().Any(x => x.Children.Id == documentId))
                            {
                                baseName = "Документарная проверка";
                                planName = string.Empty;

                                var data =
                                    serviceChildren.GetAll()
                                        .Where(x => x.Children.Id == decision.Id)
                                        .Select(x => new
                                        {
                                            x.Parent.DocumentDate,
                                            x.Parent.DocumentNumber,
                                            x.Parent.TypeDocumentGji
                                        })
                                        .ToList();

                                foreach (var item in data)
                                {
                                    var docName = Utils.GetDocumentName(item.TypeDocumentGji);

                                    if (!string.IsNullOrEmpty(planName)) planName += ", ";

                                    planName += string.Format(
                                        "{0} №{1} от {2}",
                                        docName,
                                        item.DocumentNumber,
                                        item.DocumentDate.ToDateTime().ToShortDateString());
                                }
                            }
                        }
                    }

                    #endregion
                }

                return new DecisionInfo(inspectorNames, inspectorIds, baseName, planName);
            }
            catch (ValidationException e)
            {
                throw e;
            }
            finally
            {
                Container.Release(serviceDocInspector);
                Container.Release(serviceDecision);
                Container.Release(serviceChildren);
            }
        }

        public IDomainService<DecisionProvidedDoc> DecisionProvidedDocDomain { get; set; }
        public IDomainService<ProvidedDocGji> ProvidedDocGjiDomain { get; set; }

        public IDataResult AddProvidedDocs(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var ids = baseParams.Params.GetAs<string>("providedDocIds").ToLongArray();
            var result = this.AddProvidedDocs(documentId, ids);

            return result;
        }

        private IDataResult AddProvidedDocs(long documentId, long[] ids)
        {
            try
            {
                var existingIds = new List<long>();

                var dictProvDocs = this.ProvidedDocGjiDomain.GetAll()
                    .Where(x => ids.Contains(x.Id))
                    .Select(x => new { x.Id, x.Name })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.Name).First());

                existingIds.AddRange(this.DecisionProvidedDocDomain.GetAll()
                    .Where(x => x.Decision.Id == documentId)
                    .Select(x => x.ProvidedDoc.Id)
                    .Distinct()
                    .ToList());


                foreach (var newId in ids.Distinct())
                {
                    if (existingIds.Contains(newId))
                        continue;

                    var newObj = new DecisionProvidedDoc
                    {
                        Decision = new Decision { Id = documentId },
                        Description = (dictProvDocs.ContainsKey(newId) ? dictProvDocs[newId] : string.Empty),
                        ProvidedDoc = this.ProvidedDocGjiDomain.Load(newId),
                    };

                    this.DecisionProvidedDocDomain.Save(newObj);
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        protected virtual void GetInfoCitizenStatement(ref string baseName, ref string planName, long inspectionId)
        {
            // распоряжение создано на основе обращения граждан,
            // поле planName = "Обращение № Номер ГЖИ"

            var serviceAppealCits = Container.Resolve<IDomainService<InspectionAppealCits>>();
            try
            {
                baseName = "Обращение граждан";

                // Получаем из основания наименование плана
                var baseStatement = serviceAppealCits
                    .GetAll()
                        .Where(x => x.Inspection.Id == inspectionId)
                        .Select(x => x.AppealCits.DocumentNumber + " (" + x.AppealCits.NumberGji + ")")
                        .AggregateWithSeparator(", ");

                if (!string.IsNullOrWhiteSpace(baseStatement))
                {
                    planName = string.Format("Обращение № {0}", baseStatement);
                }
            }
            finally
            {
                Container.Release(serviceAppealCits);
            }
        }

		/// <summary>
		/// Добавить предоставляемые документы автоматически (для Сахи)
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual IDataResult AutoAddProvidedDocuments(BaseParams baseParams)
        {
            return null;
        }

        protected virtual void GetInfoPlanAction(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBasePlanAction = Container.Resolve<IDomainService<BasePlanAction>>();

            try
            {
                baseName = "Проверка по плану мероприятий";

                // Получаем из Проверки юр лиц Наименование Плана
                planName = serviceBasePlanAction.GetAll()
                                    .Where(x => x.Id == inspectionId)
                                    .Select(x => x.Plan.Name)
                                    .FirstOrDefault();
            }
            finally
            {
                Container.Release(serviceBasePlanAction);
            }

        }

        protected virtual void GetInfoInsCheck(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseInsCheck = Container.Resolve<IDomainService<BaseInsCheck>>();

            try
            {
                baseName = "План инспекционных проверок";

                // Получаем из Проверки юр лиц Наименование Плана
                planName = serviceBaseInsCheck.GetAll()
                                    .Where(x => x.Id == inspectionId)
                                    .Select(x => x.Plan.Name)
                                    .FirstOrDefault();
            }
            finally
            {
                Container.Release(serviceBaseInsCheck);
            }

        }

        protected virtual void GetInfoProsClaim(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseProsClaim = Container.Resolve<IDomainService<BaseProsClaim>>();

            try
            {
                baseName = "Требование прокуратуры";

                // Получаем из проверкок по распоряжению руководства значения документа
                var dispHead = serviceBaseProsClaim.GetAll()
                                        .Where(x => x.Id == inspectionId)
                                        .Select(x => new
                                        {
                                            x.DocumentName,
                                            x.DocumentNumber,
                                            x.DocumentDate
                                        })
                                        .FirstOrDefault();

                if (dispHead != null)
                {
                    planName = string.Format("{0} №{1} от {2}", dispHead.DocumentName,
                                             dispHead.DocumentNumber,
                                             dispHead.DocumentDate !=null ? dispHead.DocumentDate.ToDateTime().ToShortDateString() : "Не заполнено");
                }
            }
            finally
            {
                Container.Release(serviceBaseProsClaim);
            }

        }

        protected virtual void GetInfoDispHead(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseDispHead = Container.Resolve<IDomainService<BaseDispHead>>();

            try
            {
                baseName = "Поручение руководства";

                // Получаем из проверкок по распоряжению руководства значения документа
                var dispHead = serviceBaseDispHead.GetAll()
                                        .Where(x => x.Id == inspectionId)
                                        .Select(x => new
                                        {
                                            x.DocumentName,
                                            x.DocumentNumber,
                                            x.DocumentDate
                                        })
                                        .FirstOrDefault();

                if (dispHead != null)
                {
                    planName = string.Format("{0} №{1} от {2}", dispHead.DocumentName,
                                             dispHead.DocumentNumber,
                                             dispHead.DocumentDate.ToDateTime().ToShortDateString());
                }
            }
            finally
            {
                Container.Release(serviceBaseDispHead);
            }

        }

        protected virtual void GetInfoJurPerson(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseJurPerson = Container.Resolve<IDomainService<BaseJurPerson>>();

            try
            {
                baseName = "Плановая проверка юр.лица";

                // Получаем из Проверки юр лиц Наименование Плана
                planName = serviceBaseJurPerson.GetAll()
                                    .Where(x => x.Id == inspectionId)
                                    .Select(x => x.Plan.Name)
                                    .FirstOrDefault();
            }
            finally
            {
                Container.Release(serviceBaseJurPerson);
            }
        }

		protected virtual void GetInfoLicenseApplicants(ref string baseName, ref string planName, long inspectionId)
		{
			var serviceBaseLicenseApplicants = Container.Resolve<IDomainService<BaseLicenseApplicants>>();

			try
			{
				baseName = "Проверка соискателей лицензии";

				var request = serviceBaseLicenseApplicants.GetAll()
					.FirstOrDefault(x => x.Id == inspectionId);

				if (request != null && request.ManOrgLicenseRequest != null)
				{
					planName = "Обращение за выдачей лицензии № {0}".FormatUsing(request.ManOrgLicenseRequest.RegisterNumber);
				}
			}
			finally
			{
				Container.Release(serviceBaseLicenseApplicants);
			}
		}
	}
}