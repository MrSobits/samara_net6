namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService.Contracts;
    using Gkh.Authentification;
    using Entities;
    using Enums;
    using Castle.Windsor;

    using NHibernate.Linq;

    using Utils = Bars.GkhGji.Utils.Utils;

    /// <summary>
    /// Сервис для <see cref="Disposal"/>
    /// </summary>
    public class DisposalService : IDisposalService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить информацию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var info = this.GetInfo(documentId);
                return new BaseDataResult(new { inspectorNames = info.InspectorNames, inspectorIds = info.InspectorIds, baseName = info.BaseName, planName = info.PlanName });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }

        public virtual IDataResult ListControlType(BaseParams baseParams)
        {
            return new List<ControlType>
                {
                    ControlType.HousingSupervision,
                    ControlType.LicensedControl
                }.Select(x => new
                {
                    Id = (int)x,
                    Name = x.GetDisplayName()
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult ListView(BaseParams baseParams)
        {
            var docGjiChildrenServ = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                /*
                 * В качестве фильтров приходят следующие параметры
                 * dateStart - Необходимо получить документы больше даты начала
                 * dateEnd - Необходимо получить документы меньше даты окончания
                 * realityObjectId - Необходимо получить документы по дому
                */

                var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
                var realityObjectId = baseParams.Params.GetAsId("realityObjectId");

                var data = this.GetViewList()
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                    .AsEnumerable()
                    .LeftJoin(docGjiChildrenServ.GetAll().Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActSurvey),
                        x => x.Id, y => y.Parent.Id, (x, y) => new
                    {
                        x.Id,
                        x.State,
                        x.DateEnd,
                        x.DateStart,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.TypeBase,
                        KindCheck = x.KindCheckName,
                        x.ContragentName,
                        MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                        MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                        x.IsActCheckExist,
                        x.RealityObjectCount,
                        x.TypeSurveyNames,
                        x.InspectorNames,
                        x.InspectionId,
                        x.TypeDocumentGji,
                        x.TypeAgreementProsecutor,
                        ControlType = x.ControlType != null ? x.ControlType.GetDisplayName() : "",
                        HasActSurvey = y != null,
                        LicenseNumber = x.License != null && x.License.State.FinalState &&
                            (x.License.DateTermination == null || x.License.DateTermination > DateTime.Today)
                                ? x.License.LicNum.ToString()
                                : ""
                    })
                    .AsQueryable()
                    .Filter(loadParam, this.Container);

                var totalCount = data.Count();

                var orderField = loadParam.Order.FirstOrDefault(x => x.Name == "State");

                data = orderField != null
                    ? orderField.Asc
                        ? data.OrderBy(x => x.State.Code)
                        : data.OrderByDescending(x => x.State.Code)
                    : data.Order(loadParam);

                return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(docGjiChildrenServ);
            }
        }

        /// <summary>
        /// Получить список пустых инспекций
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ListNullInspection(BaseParams baseParams)
        {
            var serviceDisposal = this.Container.Resolve<IDomainService<Disposal>>();

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


                var data = serviceDisposal.GetAll()
                    .Where(x => x.TypeDisposal == TypeDisposalGji.NullInspection)
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .Select(x => new
                        {
                            x.Id,
                            x.State,
                            IssuedDisposal = x.IssuedDisposal.Fio,
                            ResponsibleExecution = x.ResponsibleExecution.Fio,
                            x.DocumentNum,
                            x.DocumentNumber,
                            x.DocumentDate
                        })
                    .Filter(loadParam, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(serviceDisposal);
            }
        }

        /// <inheritdoc />
        public IDataResult ListForExport(BaseParams baseParams)
        {
            var docGjiRepos = this.Container.Resolve<IViewInspectionRepository>();
            using (this.Container.Using(docGjiRepos))
            {
                var startDate = baseParams.Params.GetAs("DisposalStartDate", DateTime.MinValue);
                var endDate = baseParams.Params.GetAs("DisposalEndDate", DateTime.MinValue);
                var auditType = baseParams.Params.GetAs<AuditType>("AuditType");

                return docGjiRepos.GetAllDto()
                    .WhereIf(startDate.IsValid(), x => x.DocumentDate >= startDate)
                    .WhereIf(endDate.IsValid(), x => x.DocumentDate <= endDate)
                    .WhereIf(auditType == AuditType.Planned, x => x.IsPlanned)
                    .WhereIf(auditType == AuditType.NotPlanned, x => !x.IsPlanned)
                    .ToListDataResult(baseParams.GetLoadParam());
            }
        }

        /// <summary>
        /// Получить список из вью
        /// </summary>
        /// <returns>Модифицированных запрос</returns>
        public virtual IQueryable<ViewDisposal> GetViewList()
        {
            return this.GetViewListWithDocType<ViewDisposal>(TypeDocumentGji.Disposal);
        }

        /// <summary>
        /// Получить список из вью для документа с типом
        /// </summary>
        /// <param name="typeDocumentGji">Тип документа</param>
        /// <returns>Модифицированных запрос</returns>
        protected IQueryable<T> GetViewListWithDocType<T>(TypeDocumentGji typeDocumentGji) where T : ViewDisposal
        {
            if (typeDocumentGji == default)
            {
                typeDocumentGji = TypeDocumentGji.Disposal;
            }
            
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var docGjiInspectorService = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var disposalRepository = this.Container.Resolve<IRepository<Disposal>>();
            var insRealObjService = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var serviceViewDisposal = this.Container.Resolve<IDomainService<T>>();

            try
            {
                var inspectorList = userManager.GetInspectorIds();
                var municipalityList = userManager.GetMunicipalityIds();

                return serviceViewDisposal.GetAll()
                    .WhereIf(inspectorList.Count > 0, x => docGjiInspectorService.GetAll().Any(y => x.Id == y.DocumentGji.Id && inspectorList.Contains(y.Inspector.Id) && y.DocumentGji.TypeDocumentGji == typeDocumentGji)
                        || disposalRepository.GetAll().Any(y => x.TypeDocumentGji == typeDocumentGji && x.Id == y.Id && (inspectorList.Contains(y.IssuedDisposal.Id) || inspectorList.Contains(y.ResponsibleExecution.Id))))
                    .WhereIf(municipalityList.Count > 0, x => insRealObjService.GetAll().Any(y => y.Inspection.Id == x.InspectionId && municipalityList.Contains(y.RealityObject.Municipality.Id)));
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(docGjiInspectorService);
                this.Container.Release(disposalRepository);
                this.Container.Release(insRealObjService);
                this.Container.Release(serviceViewDisposal);
            }
        }

        /// <summary>
        /// Получить информацию о приказе
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual DisposalInfo GetInfo(long documentId)
        {
            var serviceDocInspector = this.Container.Resolve<IDocumentGjiInspectorService>();
            var serviceDisposal = this.Container.ResolveRepository<Entities.Disposal>();
            var serviceChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

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
                    var disposal = serviceDisposal.GetAll()
                        .Fetch(x => x.Inspection)
                        .FirstOrDefault(x => x.Id == documentId);

                    if (disposal.Inspection != null)
                    {
                        switch (disposal.Inspection.TypeBase)
                        {
                            case TypeBase.PlanJuridicalPerson:
                                {
                                    this.GetInfoJurPerson(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

                            case TypeBase.DisposalHead:
                                {
                                    this.GetInfoDispHead(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

                            case TypeBase.ProsecutorsClaim:
                                {
                                    this.GetInfoProsClaim(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

                            case TypeBase.Inspection:
                                {
                                    this.GetInfoInsCheck(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

                            case TypeBase.CitizenStatement:
                                {
                                    // если распоряжение создано на основе обращения граждан
                                    this.GetInfoCitizenStatement(ref baseName, ref planName, disposal.Inspection.Id, (disposal.Inspection as BaseStatement)?.RequestType);
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
                                    this.GetInfoPlanAction(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

                            case TypeBase.LicenseApplicants:
                                {
                                    this.GetInfoLicenseApplicants(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

                            case TypeBase.GjiWarning:
                                baseName = this.GetInfoWarningDoc(disposal.Inspection.Id);
                                break;
                            
                            case TypeBase.InspectionPreventiveAction:
                                baseName = "Профилактическое мероприятие";
                                planName = $"№{disposal.Inspection.InspectionNumber} {disposal.Inspection.CheckDate?.ToShortDateString()}";
                                break;

                            default:
                                {
                                    this.GetInspectionInfo(ref baseName, ref planName, disposal.Inspection.Id, disposal.Inspection.TypeBase);
                                } 
                                break;
                        }

                        if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
                        {
                            baseName = "Проверка исполнения предписаний";
                            planName = string.Empty;

                            var data = serviceChildren.GetAll()
                                .Where(x => x.Children.Id == disposal.Id)
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
                        else if (disposal.TypeDisposal == TypeDisposalGji.Base || disposal.TypeDisposal == TypeDisposalGji.Licensing)
                        {
                            if (serviceChildren.GetAll().Any(x => x.Children.Id == documentId))
                            {
                                baseName = "Документарная проверка";
                                planName = string.Empty;

                                var data =
                                    serviceChildren.GetAll()
                                        .Where(x => x.Children.Id == disposal.Id)
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

                return new DisposalInfo(inspectorNames, inspectorIds, baseName, planName);
            }
            catch (ValidationException e)
            {
                throw e;
            }
            finally
            {
                this.Container.Release(serviceDocInspector);
                this.Container.Release(serviceDisposal);
                this.Container.Release(serviceChildren);
            }
        }

        protected virtual void GetInfoCitizenStatement(ref string baseName, ref string planName, long inspectionId, BaseStatementRequestType? requestType)
        {
            var inspectionAppCitsDomain = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
            var inspectionDocDomain = this.Container.Resolve<IDomainService<InspectionGjiDocumentGji>>();

            baseName = requestType.HasValue && requestType.Value != 0 ? requestType.GetDisplayName() : "Обращение граждан";

            try
            {
                string docNumbers;
                if (requestType == BaseStatementRequestType.MotivationConclusion)
                {
                    docNumbers = inspectionDocDomain.GetAll()
                        .Where(x => x.Inspection.Id == inspectionId)
                        .Select(x => x.Document.DocumentNumber)
                        .AggregateWithSeparator(", ");

                    if (!string.IsNullOrWhiteSpace(docNumbers))
                    {
                        planName = $"{requestType.GetDisplayName()} № ({docNumbers})";
                    }
                }
                else
                {
                    docNumbers = inspectionAppCitsDomain.GetAll()
                        .Where(x => x.Inspection.Id == inspectionId)
                        .Select(x => x.AppealCits.NumberGji)
                        .AggregateWithSeparator(", ");

                    if (!string.IsNullOrWhiteSpace(docNumbers))
                    {
                        planName = $"Обращение № ({docNumbers})";
                    }
                }
            }
            finally
            {
                this.Container.Release(inspectionAppCitsDomain);
                this.Container.Release(inspectionDocDomain);
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
            var serviceBasePlanAction = this.Container.Resolve<IDomainService<BasePlanAction>>();

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
                this.Container.Release(serviceBasePlanAction);
            }

        }

        protected virtual string GetInfoWarningDoc(long inspectionId)
        {
            var service = this.Container.Resolve<IDomainService<WarningInspection>>();

            try
            {
                return  service.GetAll()
                    .Where(x => x.Id == inspectionId)
                    .Select(x => x.InspectionBasis.GetDescriptionName())
                    .FirstOrDefault();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        protected virtual void GetInfoInsCheck(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseInsCheck = this.Container.Resolve<IDomainService<BaseInsCheck>>();

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
                this.Container.Release(serviceBaseInsCheck);
            }

        }

        protected virtual void GetInfoProsClaim(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseProsClaim = this.Container.Resolve<IDomainService<BaseProsClaim>>();

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
                this.Container.Release(serviceBaseProsClaim);
            }

        }

        protected virtual void GetInfoDispHead(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseDispHead = this.Container.Resolve<IDomainService<BaseDispHead>>();

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
                this.Container.Release(serviceBaseDispHead);
            }

        }

        protected virtual void GetInfoJurPerson(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseJurPerson = this.Container.Resolve<IDomainService<BaseJurPerson>>();

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
                this.Container.Release(serviceBaseJurPerson);
            }
        }

        protected virtual void GetInfoLicenseApplicants(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseLicenseApplicants = this.Container.Resolve<IDomainService<BaseLicenseApplicants>>();

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
                this.Container.Release(serviceBaseLicenseApplicants);
            }
        }

        /// <summary>
        /// Получить Основание обследования и Документ основание из проверки (переопределить в других модулях)
        /// </summary>
        protected virtual void GetInspectionInfo(ref string baseName, ref string planName, long inspectionId, TypeBase typeBase)
        {
        }
    }
}