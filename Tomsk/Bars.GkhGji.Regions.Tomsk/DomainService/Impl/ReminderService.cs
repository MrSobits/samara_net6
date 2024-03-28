namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;
	using System.Linq.Expressions;
	using Bars.B4.DataAccess;
	using Bars.Gkh.Utils;

    public class ReminderService : Bars.GkhGji.DomainService.ReminderService
    {
        /// <summary>
        /// Дополнительная информация в панели Руководителяи Инспектора
        /// </summary>
        public override IDataResult GetInfo(BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var serviceInspSubscrip = Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = Container.Resolve<IDomainService<Reminder>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var activeOperator = userManager.GetActiveOperator();


                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new BaseDataResult(new
                    {
                        AllTask = 0,
                        ComeUpToTerm = 0,
                        Expired = 0,
                        Unformed = 0
                    });
                }

                var isHead = baseParams.Params.GetAs("isHead", true);
                var inspectorId = activeOperator != null && activeOperator.Inspector != null ? activeOperator.Inspector.Id : 0;

                var inspectorIds =
                    serviceInspSubscrip.GetAll()
                        .Where(x => x.SignedInspector.Id == inspectorId)
                        .Select(x => x.Inspector.Id);

                var data = this.GetQueryable(baseParams, servReminder)
                    .Where(x => x.Actuality)
                    .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                    .WhereIf(isHead, x => inspectorIds.Any(y => y == x.Inspector.Id))
                    .WhereIf(!isHead, x => x.Inspector.Id == inspectorId)
                    .Select(x => new
                    {
                        x.Id,
                        x.CheckDate,
                        x.TypeReminder
                    })
                    .Filter(loadParams, Container)
                    .AsEnumerable()
                    .GroupBy(x => 1, (key, value) => new
                    {
                        allTask = value.Count(),
                        comeUpToTerm = value.Count(
                                    x =>
                                        x.TypeReminder != TypeReminder.BaseInspection
                                        && x.TypeReminder != TypeReminder.Protocol
                                        && x.CheckDate.HasValue
                                        && DateTime.Now.Date < x.CheckDate
                                        && DateTime.Now.AddDays(5).Date >= x.CheckDate),
                        expired = value.Count(
                                     x =>
                                        x.TypeReminder != TypeReminder.BaseInspection
                                        && x.TypeReminder != TypeReminder.Protocol &&
                                        ((x.CheckDate.HasValue && DateTime.Now.Date >= x.CheckDate) ||
                                         !x.CheckDate.HasValue)),
                        unformed = value.Count(x => x.TypeReminder == TypeReminder.BaseInspection || x.TypeReminder == TypeReminder.Protocol)
                    })
                    .FirstOrDefault();

                if (data != null)
                {
                    return new BaseDataResult(new
                    {
                        AllTask = data.allTask,
                        ComeUpToTerm = data.comeUpToTerm,
                        Expired = data.expired,
                        Unformed = data.unformed
                    });
                }

                return new BaseDataResult();
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(serviceInspSubscrip);
                Container.Release(servReminder);
            }
        }

        /// <summary>
        /// Список напиоминалок в виджете руководителя для Списка "Состояние Задач"
        /// </summary>
        public override IDataResult ListTaskState(BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var servInsSub = Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = Container.Resolve<IDomainService<Reminder>>();

            try
            {
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorIds = servInsSub.GetAll()
                    .Where(x => x.SignedInspector.Id == activeOperator.Inspector.Id)
                    .Select(x => x.Inspector.Id);

                var data =
                    servReminder.GetAll()
                        .Where(x => x.Actuality)
                        .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                        .Where(x => inspectorIds.Any(y => y == x.Inspector.Id))
                        .Select(x => new { x.Id, x.CheckDate, x.TypeReminder })
                        .AsEnumerable()
                        .GroupBy(x => x.TypeReminder)
                        .ToDictionary(x => x.Key,
                            y => new
                            {
                                CountRed = y.Count(
                                    x =>
                                        x.TypeReminder != TypeReminder.BaseInspection
                                        && x.TypeReminder != TypeReminder.Protocol &&
                                        ((x.CheckDate.HasValue && DateTime.Now.Date >= x.CheckDate) ||
                                         !x.CheckDate.HasValue)),
                                CountYellow = y.Count(
                                    x =>
                                        x.TypeReminder != TypeReminder.BaseInspection
                                        && x.TypeReminder != TypeReminder.Protocol 
                                        && x.CheckDate.HasValue
                                        && DateTime.Now.Date < x.CheckDate
                                        && DateTime.Now.AddDays(5).Date >= x.CheckDate),
                                CountGreen = y.Count(
                                    x => x.TypeReminder != TypeReminder.BaseInspection &&
                                        x.TypeReminder != TypeReminder.Protocol
                                        && x.CheckDate.HasValue
                                        && DateTime.Now.AddDays(5) < x.CheckDate.Value),
                                CountWhite = y.Count(x => x.TypeReminder == TypeReminder.BaseInspection || x.TypeReminder == TypeReminder.Protocol)
                            })
                        .Select(x => new
                        {
                            TypeReminder = x.Key,
                            x.Value.CountRed,
                            x.Value.CountYellow,
                            x.Value.CountGreen,
                            x.Value.CountWhite
                        })
                        .ToList();

                var totalCount = data.Count();

                return new ListDataResult(data, totalCount);
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(servInsSub);
                Container.Release(servReminder);
            }
        }

        /// <summary>
        /// Список напоминалок в Виджете для Инспектора
        /// </summary>
        public override IDataResult ListWidgetInspector(BaseParams baseParams)
        {
            var servReminder = Container.Resolve<IDomainService<Reminder>>();
            var servInsSub = Container.Resolve<IDomainService<InspectorSubscription>>();
            var userManager = Container.Resolve<IGkhUserManager>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorId =
                    activeOperator.Inspector != null
                        ? activeOperator.Inspector.Id
                        : 0;

                var data = servReminder.GetAll()
                    .Where(x => x.Actuality)
                    .WhereIf(inspectorId > 0, x => x.Inspector.Id == inspectorId)
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeReminder,
                        x.CheckDate,
                        Num = x.Num ?? "",
                        AppealId = x.AppealCits != null ? x.AppealCits.Id: 0,
                        AppealNum =  x.AppealCits != null ? x.AppealCits.DocumentNumber : "",
                        AppealNumGji =  x.AppealCits != null ? x.AppealCits.NumberGji : "",
                        AppealCorr = x.AppealCits != null ? x.AppealCits.Correspondent : "",
                        AppealCorrAddress = x.AppealCits != null ? x.AppealCits.CorrespondentAddress : "",
                        AppealDescription = x.AppealCits != null ? x.AppealCits.Description : "",
                        ContragentName = x.Contragent != null ? x.Contragent.Name : "",
                        ColorTypeReminder = "red",
                        x.CategoryReminder
                    })
                    .OrderBy(x => x.CheckDate);

                var totalCount = data.Count();

                var result = data
                    .Paging(loadParam)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeReminder,
                        CheckDate = x.CheckDate.HasValue ? x.CheckDate.ToDateTime() : DateTime.MinValue,
                        Num = x.TypeReminder == TypeReminder.Statement ? (!string.IsNullOrEmpty(x.AppealNum) ? x.AppealNum : x.AppealNumGji) : x.Num,
                        NumText = x.TypeReminder == TypeReminder.Statement ? "Номер обращения" : "№ ГЖИ",
                        x.AppealCorr,
                        x.AppealCorrAddress,
                        AppealDescription = x.AppealDescription.Length > 80 ? x.AppealDescription.Substring(0, 80).Trim() + "..." : x.AppealDescription,
                        x.ContragentName,
                        ColorTypeReminder = GetColorTypeReminder(x.TypeReminder),
                        x.CategoryReminder,
                        Color = DateTime.Now >= x.CheckDate.GetValueOrDefault()
                            ? "red"
                            : x.CheckDate.GetValueOrDefault().AddDays(-5) <= DateTime.Now
                                ? "yellow"
                                : "green"
                    }).ToList();

                return new ListDataResult(result, totalCount);
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(servReminder);
                Container.Release(servInsSub);
            }
        }


        /// <summary>
        /// Список Напоминалок руководителя в Панели Руководителя
        /// </summary>
        public override IDataResult ListReminderOfHead(BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var servInsSub = Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = Container.Resolve<IDomainService<Reminder>>();
			var appCitsRealityObjectDomain = Container.ResolveDomain<AppealCitsRealityObject>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorIds = servInsSub.GetAll()
                    .Where(x => x.SignedInspector.Id == activeOperator.Inspector.Id)
                    .Select(x => x.Inspector.Id);

                // Параметры с виджетов
                var inspectorId = baseParams.Params.GetAs<long>("inspectorId");
                var colorType = baseParams.Params.GetAs("colorType", string.Empty);
                var typeReminder = baseParams.Params.GetAs("typeReminder", TypeReminder.BaseInspection);
                var isTypeReminder = baseParams.Params.GetAs("isTypeReminder", false);

                Expression<Func<Reminder, bool>> func;
                switch (colorType)
                {
                    case "red":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && x.TypeReminder != TypeReminder.Protocol && DateTime.Now >= x.CheckDate.GetValueOrDefault();
                        break;
                    case "yellow":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && x.TypeReminder != TypeReminder.Protocol && x.CheckDate.GetValueOrDefault() > DateTime.Now && DateTime.Now.AddDays(5) > x.CheckDate.GetValueOrDefault();
                        break;
                    case "green":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && x.TypeReminder != TypeReminder.Protocol && x.CheckDate.GetValueOrDefault() > DateTime.Now.AddDays(5);
                        break;
                    case "white":
                        func = x => x.TypeReminder == TypeReminder.BaseInspection || x.TypeReminder == TypeReminder.Protocol;
                        break;
                    default:
                        func = x => true;
                        break;
                }

                var query = this.GetQueryable(baseParams, servReminder)
                    .Where(x => x.Actuality)
                    .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                    .Where(x => inspectorIds.Any(y => y == x.Inspector.Id))
                    .WhereIf(inspectorId > 0, x => x.Inspector.Id == inspectorId)
                    .WhereIf(!string.IsNullOrEmpty(colorType), func)
                    .WhereIf(isTypeReminder, x => x.TypeReminder == typeReminder);

				var appCitIds = query.Where(x => x.AppealCits != null)
					.Select(x => x.AppealCits.Id)
					.ToArray();

				var locationProblemsByAppCitsId = appCitsRealityObjectDomain.GetAll()
					.Where(x => appCitIds.Contains(x.AppealCits.Id))
					.GroupBy(x => x.AppealCits.Id)
					.ToDictionary(x => x.Key,
						x => x.Select(y => y.RealityObject.Address).AggregateWithSeparator(", "));

	            var data = query
		            .AsEnumerable()
		            .Select(x => new
		            {
			            x.Id,
			            InspectionGji = x.InspectionGji != null ? new {x.InspectionGji.Id, x.InspectionGji.TypeBase} : null,
			            DocumentGji = x.DocumentGji != null ? new {x.DocumentGji.Id, x.DocumentGji.TypeDocumentGji} : null,
			            AppealCits = x.AppealCits != null ? x.AppealCits.Id : 0,
			            Contragent = x.Contragent != null ? x.Contragent.Name : "",
			            x.Actuality,
			            x.TypeReminder,
			            x.Num,
			            x.CheckDate,
			            Inspector = x.Inspector.Fio,
						CorrespondentAddress = x.AppealCits != null ? x.AppealCits.CorrespondentAddress : "",
						LocationProblem =
							x.AppealCits != null && locationProblemsByAppCitsId.ContainsKey(x.AppealCits.Id)
								? locationProblemsByAppCitsId[x.AppealCits.Id]
								: ""
		            })
                    .AsQueryable()
                    .Filter(loadParams, Container)
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.CheckDate);

                var totalCount = data.Count();

                data = data.Order(loadParams).Paging(loadParams);

                return new ListDataResult(data.ToList(), totalCount);
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(servInsSub);
                Container.Release(servReminder);
				Container.Release(appCitsRealityObjectDomain);
            }
        }


        /// <summary>
        /// Список напоминалок в виджете руководителя для Списка "Контроль Задач"
        /// </summary>
        public override IDataResult ListTaskControl(BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var servInsSub = Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = Container.Resolve<IDomainService<Reminder>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorIds = servInsSub.GetAll()
                              .Where(x => x.SignedInspector.Id == activeOperator.Inspector.Id)
                              .Select(x => x.Inspector.Id);

                var data =
                    servReminder.GetAll()
                        .Where(x => x.Actuality)
                        .Where(x => inspectorIds.Any(y => y == x.Inspector.Id))
                        .Select(x => new
                        {
                            InspectorId = x.Inspector.Id,
                            InspectorFio = x.Inspector.Fio,
                            Color =
                                x.TypeReminder != TypeReminder.BaseInspection && x.TypeReminder != TypeReminder.Protocol
                                    ? DateTime.Now.Date >= x.CheckDate.GetValueOrDefault()
                                        ? "red"
                                        : x.CheckDate.GetValueOrDefault().AddDays(-5) < DateTime.Now
                                            ? "yellow"
                                            : "green"
                                    : "white"
                        })
                        .AsEnumerable()
                        .GroupBy(
                            x => new { x.InspectorId, x.InspectorFio },
                            x => x.Color,
                            (key, value) => new { Inspector = key, Color = value })
                        .Select(x => new
                        {
                            x.Inspector.InspectorFio,
                            x.Inspector.InspectorId,
                            CountRed = x.Color.Count(y => y == "red"),
                            CountYellow = x.Color.Count(y => y == "yellow"),
                            CountGreen = x.Color.Count(y => y == "green"),
                            CountWhite = x.Color.Count(y => y == "white")
                        })
                        .OrderByDescending(x => x.CountRed)
                        .ThenByDescending(x => x.CountYellow)
                        .ThenByDescending(x => x.CountGreen)
                        .ThenByDescending(x => x.CountWhite)
                        .AsQueryable();

                var totalCount = data.Count();

                // Paging здесь ненужен поскольку мы в этом виджете должны видеть всех инспекторов
                return new ListDataResult(data.ToList(), totalCount);
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(servInsSub);
                Container.Release(servReminder);
            }
        }

	    public override IDataResult ListReminderOfInspector(BaseParams baseParams)
	    {
			var userManager = Container.Resolve<IGkhUserManager>();
			var servReminder = Container.Resolve<IDomainService<Reminder>>();
			var servInsSub = Container.Resolve<IDomainService<InspectorSubscription>>();
			var appCitsRealityObjectDomain = Container.ResolveDomain<AppealCitsRealityObject>();

			try
			{
				var loadParams = baseParams.GetLoadParam();
				var activeOperator = userManager.GetActiveOperator();

				if (activeOperator == null || activeOperator.Inspector == null)
				{
					return new ListDataResult(null, 0);
				}

				var inspectorId = activeOperator.Inspector.Id;
				var inspectorIds = servInsSub.GetAll()
				   .Where(x => x.SignedInspector.Id == inspectorId)
				   .Select(x => x.Inspector.Id);

				var query = this.GetQueryable(baseParams, servReminder)
					.Where(x => x.Actuality)
                    .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                    .WhereIf(inspectorId > 0, x => x.Inspector.Id == inspectorId)
                    .WhereIf(inspectorId == 0, x => inspectorIds.Any(y => y == x.Inspector.Id));

				var appCitIds = query.Where(x => x.AppealCits != null)
					.Select(x => x.AppealCits.Id)
					.ToArray();

				var locationProblemsByAppCitsId = appCitsRealityObjectDomain.GetAll()
					.Where(x => appCitIds.Contains(x.AppealCits.Id))
					.GroupBy(x => x.AppealCits.Id)
					.ToDictionary(x => x.Key,
						x => x.Select(y => y.RealityObject.Address).AggregateWithSeparator(", "));

				var data = query
					.AsEnumerable()
					.Select(x => new
					{
						x.Id,
						InspectionGji = x.InspectionGji != null ? new {x.InspectionGji.Id, x.InspectionGji.TypeBase} : null,
						DocumentGji = x.DocumentGji != null ? new {x.DocumentGji.Id, x.DocumentGji.TypeDocumentGji} : null,
						AppealCits = x.AppealCits != null ? x.AppealCits.Id : 0,
						Contragent = x.Contragent != null ? x.Contragent.Name : "",
						x.Actuality,
						x.TypeReminder,
						x.Num,
						x.CheckDate,
						CorrespondentAddress = x.AppealCits != null ? x.AppealCits.CorrespondentAddress : "",
						LocationProblem =
							x.AppealCits != null && locationProblemsByAppCitsId.ContainsKey(x.AppealCits.Id)
								? locationProblemsByAppCitsId[x.AppealCits.Id]
								: ""
					})
                    .ToList()
                    .AsQueryable()
                    .Filter(loadParams, Container);

                var totalCount = data.Count();

                data = data.Order(loadParams)
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.CheckDate)
                    .Paging(loadParams);

                return new ListDataResult(data.ToList(), totalCount);
            }
			finally
			{
				Container.Release(userManager);
				Container.Release(servReminder);
				Container.Release(appCitsRealityObjectDomain);
			}
	    }

	    public override IQueryable<Reminder> GetQueryable(BaseParams baseParams, IDomainService<Reminder> service)
        {
            var query = service.GetAll();

            var dopFilter = baseParams.Params.GetAs("dopFilter", 10);
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var isStatement = baseParams.Params.GetAs("isStatement", true);
            var isDisposal = baseParams.Params.GetAs("isDisposal", true);
            var isPrescription = baseParams.Params.GetAs("isPrescription", true);
            var isBaseInspection = baseParams.Params.GetAs("isBaseInspection", true);
            var isProtocol = baseParams.Params.GetAs("isProtocol", true);

            //20 - подходящие к сроку, 30 - просроченные, 40 - не сформировано
            switch (dopFilter)
            {
                case 20:
                    {
                        query = query
                            .Where(x => x.TypeReminder != TypeReminder.BaseInspection && x.TypeReminder != TypeReminder.Protocol)
                            .Where(x => x.CheckDate.HasValue)
                            .Where(x => DateTime.Now.Date < x.CheckDate)
                            .Where(x => DateTime.Now.AddDays(5) >= x.CheckDate.Value);
                    }
                    break;

                case 30:
                    {
                        query = query
                            .Where(x => x.TypeReminder != TypeReminder.BaseInspection && x.TypeReminder != TypeReminder.Protocol)
                            .Where(x => (x.CheckDate.HasValue && DateTime.Now.Date >= x.CheckDate) || !x.CheckDate.HasValue);
                    }
                    break;
                case 40:
                    {
                        query = query
                            .Where(x => x.TypeReminder == TypeReminder.BaseInspection || x.TypeReminder == TypeReminder.Protocol);
                    }
                    break;
            }

            return query.WhereIf(dateStart != DateTime.MinValue, x => x.CheckDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.CheckDate <= dateEnd)
                .WhereIf(!isStatement, x => x.TypeReminder != TypeReminder.Statement)
                .WhereIf(!isDisposal, x => x.TypeReminder != TypeReminder.Disposal)
                .WhereIf(!isPrescription, x => x.TypeReminder != TypeReminder.Prescription)
                .WhereIf(!isBaseInspection, x => x.TypeReminder != TypeReminder.BaseInspection)
                .WhereIf(!isProtocol, x => x.TypeReminder != TypeReminder.Protocol);
        }

        public override TypeReminder[] ReminderTypes()
        {
            return new TypeReminder[]
                {
                    TypeReminder.Statement,
                    TypeReminder.BaseInspection, 
                    TypeReminder.Disposal, 
                    TypeReminder.Prescription, 
                    TypeReminder.Protocol
                };
        }
    }
}