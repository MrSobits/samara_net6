namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;
    using Entities.Reminder;
    using System.Linq.Expressions;
    using B4.DataAccess;
    using B4.Modules.States;

    public class ReminderService : Bars.GkhGji.DomainService.ReminderService
    {
        /// <summary>
        /// Репозитория статуса 
        /// </summary>
        public IRepository<State> StateRepo { get; set; }

        /// <summary>
        /// Провайдер статуса 
        /// </summary>
        public IStateProvider StateProvider { get; set; }

        /// <summary>
        /// Фильтрация
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<ChelyabinskReminder> GetQueryable(BaseParams baseParams, IDomainService<ChelyabinskReminder> service)
        {
            var query = service.GetAll();

            var dopFilter = baseParams.Params.GetAs("dopFilter", 10);
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var isStatement = baseParams.Params.GetAs("isStatement", true);
            var isDisposal = baseParams.Params.GetAs("isDisposal", true);
            var isPrescription = baseParams.Params.GetAs("isPrescription", true);
            var isBaseInspection = baseParams.Params.GetAs("isBaseInspection", true);
            var isActCheck = baseParams.Params.GetAs("isActCheck", true);
            var isNoticeOfInspection = baseParams.Params.GetAs("isNoticeOfInspection", true);

            var answersInWork = this.GetInWorkAnswerQuery();
            switch (dopFilter)
            {
                case 20:
                    {
                        query = query
                            .Where(x => x.TypeReminder != TypeReminder.BaseInspection)
                            .Where(x => x.CheckDate.HasValue)
                            .Where(x => DateTime.Now.Date < x.CheckDate)
                            .Where(x => DateTime.Now.AddDays(5) >= x.CheckDate.Value);
                    }
                    break;

                case 30:
                    {
                        query = query
                            .Where(x => x.TypeReminder != TypeReminder.BaseInspection)
                            .Where(x => ((x.CheckDate.HasValue && DateTime.Now.Date >= x.CheckDate) || !x.CheckDate.HasValue) && !answersInWork
                                .Where(y => y.AppealCits == x.AppealCits)
                                .Any(y => y.Executor.Id == x.AppealCitsExecutant.Controller.Id
                                    || y.Executor.Id == x.AppealCitsExecutant.Executant.Id));
                    }
                    break;
                case 40:
                    {
                        query = query
                            .Where(x => x.TypeReminder == TypeReminder.BaseInspection);
                    }
                    break;

            }

            return query.WhereIf(dateStart != DateTime.MinValue, x => x.CheckDate >= dateStart)
                     .WhereIf(dateEnd != DateTime.MinValue, x => x.CheckDate <= dateEnd)
                     .WhereIf(!isStatement, x => x.TypeReminder != TypeReminder.Statement)
                     .WhereIf(!isDisposal, x => x.TypeReminder != TypeReminder.Disposal)
                     .WhereIf(!isPrescription, x => x.TypeReminder != TypeReminder.Prescription)
                     .WhereIf(!isBaseInspection, x => x.TypeReminder != TypeReminder.BaseInspection)
                     .WhereIf(!isActCheck, x => x.TypeReminder != TypeReminder.ActCheck)
                     .WhereIf(!isNoticeOfInspection, x => x.TypeReminder != TypeReminder.NoticeOfInspection);
        }

        /// <summary>
        /// Тип напоминания
        /// </summary>
        public override TypeReminder[] ReminderTypes()
        {
            // Вообщем по умолчанию регистрируются только такие типы 
            // в слуаче если в регионе нобходимы другие, то тогда заменяем реализацию
            return new TypeReminder[]
                {
                    TypeReminder.Statement,
                    TypeReminder.BaseInspection,
                    TypeReminder.Disposal,
                    TypeReminder.Prescription,
                    TypeReminder.ActCheck,
                    TypeReminder.NoticeOfInspection
                };
        }

        /// <summary>
        /// Список напоминалок в Виджете для Инспектора
        /// </summary>
        public override IDataResult ListWidgetInspector(BaseParams baseParams)
        {
            var servReminder = this.Container.Resolve<IDomainService<ChelyabinskReminder>>();
            var servInsSub = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorId = activeOperator.Inspector?.Id ?? 0;

                var answersInWork = this.GetInWorkAnswerQuery();

                var result = servReminder.GetAll()
                    .Where(x => x.Actuality)
                    .WhereIf(inspectorId > 0, x => x.AppealCitsExecutant.Author.Id == inspectorId || x.AppealCitsExecutant.Controller.Id == inspectorId || x.AppealCitsExecutant.Executant.Id == inspectorId)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.TypeReminder,
                            CheckDate = x.AppealCits.ExtensTime ?? x.CheckDate,
                            Num = x.Num ?? "",
                            AppealId = x.AppealCits != null ? x.AppealCits.Id : 0,
                            AppealNum = x.AppealCits != null ? x.AppealCits.DocumentNumber : "",
                            AppealState = x.AppealCits != null ? x.AppealCits.State.Name : "",
                            AppealStateCode = x.AppealCits != null ? x.AppealCits.State.Code : "",
                            AppealNumGji = x.AppealCits != null ? x.AppealCits.NumberGji : "",
                            AppealCorr = x.AppealCits != null ? x.AppealCits.Correspondent : "",
                            AppealCorrAddress = x.AppealCits != null ? x.AppealCits.CorrespondentAddress : "",
                            AppealDescription = x.AppealCits != null ? x.AppealCits.Description : "",
                            ContragentName = x.Contragent != null ? x.Contragent.Name : "",
                            x.CategoryReminder,
                            HasAppealCitizensInWorkState = answersInWork
                                                            .Where(y => y.AppealCits == x.AppealCits)
                                                            .Any(y => y.Executor.Id == x.AppealCitsExecutant.Controller.Id 
                                                                        || y.Executor.Id == x.AppealCitsExecutant.Executant.Id)
                        })
                    .OrderBy(x => x.CheckDate)
                    .AsEnumerable()
                    .GroupBy(x => x.AppealId)
                    .Select(x => new
                    {
                        x.First().Id,
                        x.First().TypeReminder,
                        CheckDate = x.First().CheckDate.HasValue ? x.First().CheckDate.ToDateTime() : DateTime.MinValue,
                        x.First().AppealState,
                        Num = x.First().TypeReminder == TypeReminder.Statement ? (!string.IsNullOrEmpty(x.First().AppealNum) ? x.First().AppealNum : x.First().AppealNumGji) : x.First().Num,
                        NumText = x.First().TypeReminder == TypeReminder.Statement ? "Номер обращения" : "№ ГЖИ",
                        x.First().AppealCorr,
                        x.First().AppealCorrAddress,
                        AppealDescription = x.First().AppealDescription.Length > 80 ? x.First().AppealDescription.Substring(0, 80).Trim() + "..." : x.First().AppealDescription,
                        x.First().ContragentName,
                        ColorTypeReminder = this.GetColorTypeReminder(x.First().TypeReminder),
                        x.First().CategoryReminder,
                        Color = x.First().HasAppealCitizensInWorkState
                            ? "green" : x.First().AppealStateCode == "СОПР2"? "green"
                            : DateTime.Now >= x.First().CheckDate.GetValueOrDefault()
                                ? "red"
                                : x.First().CheckDate.GetValueOrDefault().AddDays(-5) <= DateTime.Now
                                    ? "yellow"
                                    : "green"
                    }).Take(4)
                    .ToList();

                return new ListDataResult(result, result.Count);
            }
            catch (ValidationException exc)
            {
                return new ListDataResult { Success = false, Message = exc.Message };
            }
            catch (Exception exc)
            {
                return new ListDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(servReminder);
                this.Container.Release(servInsSub);
            }
        }

        /// <summary>
        /// Список Напоминалок руководителя в Панели Руководителя
        /// </summary>
        public override IDataResult ListReminderOfHead(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var servInsSub = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = this.Container.Resolve<IDomainService<ChelyabinskReminder>>();

            try
            {
                var entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(AppealCits));
                var openState = this.StateRepo.GetAll().FirstOrDefault(x => x.Name == "В работе" && x.TypeId == entityInfo.TypeId);
                var loadParams = baseParams.GetLoadParam();
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null)
                {

                    throw new ValidationException("Не найден оператор");
                }

                if (activeOperator.Inspector == null)
                {
                    throw new ValidationException("Не назначен инспектор у оператора");
                }

                var inspectorIds = servInsSub.GetAll()
                    .Where(x => x.SignedInspector.Id == activeOperator.Inspector.Id)
                    .Select(x => x.Inspector.Id);

                // Параметры с виджетов
                var inspectorId = baseParams.Params.GetAs<long>("inspectorId");
                var colorType = baseParams.Params.GetAs("colorType", string.Empty);
                var typeReminder = baseParams.Params.GetAs("typeReminder", TypeReminder.BaseInspection);
                var isTypeReminder = baseParams.Params.GetAs("isTypeReminder", false);

                Expression<Func<ChelyabinskReminder, bool>> func;
                switch (colorType)
                {
                    case "red":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && x.AppealCits != null &&
                        (x.AppealCits.ExtensTime == null && DateTime.Now >= x.CheckDate.GetValueOrDefault()
                        || x.AppealCits.ExtensTime != null && DateTime.Now >= x.AppealCits.ExtensTime.GetValueOrDefault());
                        break;
                    case "yellow":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && x.AppealCits != null &&
                        (x.AppealCits.ExtensTime == null && x.CheckDate.GetValueOrDefault() > DateTime.Now && DateTime.Now.AddDays(5) > x.CheckDate.GetValueOrDefault()
                        || x.AppealCits.ExtensTime != null && x.AppealCits.ExtensTime.GetValueOrDefault() > DateTime.Now && DateTime.Now.AddDays(5) > x.AppealCits.ExtensTime.GetValueOrDefault());
                        break;
                    case "green":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && x.AppealCits != null &&
                        (x.AppealCits.ExtensTime == null && x.CheckDate.GetValueOrDefault() > DateTime.Now.AddDays(5)
                        || x.AppealCits.ExtensTime != null && x.AppealCits.ExtensTime.GetValueOrDefault() > DateTime.Now.AddDays(5));
                        break;
                    case "white":
                        func = x => x.TypeReminder == TypeReminder.BaseInspection;
                        break;
                    default:
                        func = x => true;
                        break;
                }
                var answersInWork = this.GetInWorkAnswerQuery();

                var data = this.GetQueryable(baseParams, servReminder)
                    .Where(x => x.Actuality)
                   // .Where(x => inspectorIds.Any(y => y == x.AppealCitsExecutant.Executant.Id))
                    .Where(x => inspectorIds.Any(y => y == x.Inspector.Id))
                    .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                    .Where(x => x.AppealCits == null ||  (x.AppealCits.State.Id == openState.Id))
                  //  .WhereIf(inspectorId > 0, x => x.AppealCitsExecutant.Executant.Id == inspectorId)
                    .WhereIf(inspectorId > 0, x => x.Inspector.Id == inspectorId)
                    .WhereIf(!string.IsNullOrEmpty(colorType), func)
                    .WhereIf(isTypeReminder, x => x.TypeReminder == typeReminder)
                    .Select(
                        x => new
                        {
                            x.Id,
                            InspectionGji = x.InspectionGji != null ? new { x.InspectionGji.Id, x.InspectionGji.TypeBase } : null,
                            DocumentGji = x.DocumentGji != null ? new { x.DocumentGji.Id, x.DocumentGji.TypeDocumentGji } : null,
                            AppealCits = x.AppealCits != null ? x.AppealCits.Id : 0,
                            Contragent = x.Contragent.Name,
                            x.Actuality,
                            x.TypeReminder,
                            x.CategoryReminder,
                            x.Num,
                            x.CheckDate,
                            Inspector = x.AppealCitsExecutant.Executant.Fio,
                            HasAppealCitizensInWorkState = answersInWork
                                .Where(y => y.AppealCits == x.AppealCits)
                                  .Any(y => y.Executor.Id == x.Inspector.Id
                                    || y.Executor.Id == x.Inspector.Id)
                                //.Any(y => y.Executor.Id == x.AppealCitsExecutant.Controller.Id
                                //    || y.Executor.Id == x.AppealCitsExecutant.Executant.Id)
                        })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.CheckDate)
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(servInsSub);
                this.Container.Release(servReminder);
            }
        }

        /// <summary>
        /// Список напиоминалок в виджете руководителя для Списка "Состояние Задач"
        /// </summary>
        public override IDataResult ListTaskState(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var servInsSub = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = this.Container.Resolve<IDomainService<ChelyabinskReminder>>();

            try
            {
                var entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(AppealCits));
                var openState = this.StateRepo.GetAll().FirstOrDefault(x => x.Name == "В работе" && x.TypeId == entityInfo.TypeId);
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorIds = servInsSub.GetAll()
                    .Where(x => x.SignedInspector.Id == activeOperator.Inspector.Id)
                    .Select(x => x.Inspector.Id);


                var inspectorId = activeOperator.Inspector?.Id ?? 0;
                var answersInWork = this.GetInWorkAnswerQuery();

                var data = servReminder.GetAll()
                    .Where(x => x.Actuality)
                    .WhereIf(
                        inspectorId > 0,
                        x =>
                            x.AppealCitsExecutant.Author.Id == inspectorId || x.AppealCitsExecutant.Controller.Id == inspectorId
                                || x.AppealCitsExecutant.Executant.Id == inspectorId)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.TypeReminder,
                            CheckDate = x.AppealCits.ExtensTime ?? x.CheckDate,
                            AppealId = x.AppealCits.Id,
                            HasAppealCitizensInWorkState =
                                x.TypeReminder == TypeReminder.Statement && answersInWork
                                    .Where(y => y.Executor.Id == x.AppealCitsExecutant.Controller.Id || y.Executor.Id == x.AppealCitsExecutant.Executant.Id)
                                    .Any(y => y.AppealCits == x.AppealCits)
                        })
                    .OrderBy(x => x.CheckDate)
                    .AsEnumerable()
                    .GroupBy(x => x.AppealId)
                    .Select(
                        x => new
                        {
                            x.First().Id,
                            x.First().CheckDate,
                            x.First().TypeReminder,
                            x.First().HasAppealCitizensInWorkState

                        }).GroupBy(x => x.TypeReminder)
                    .ToDictionary(
                        x => x.Key,
                        y => new
                        {
                            CountRed = y.Count(
                                x =>
                                    x.TypeReminder != TypeReminder.BaseInspection
                                        && ((x.CheckDate.HasValue && DateTime.Now.Date >= x.CheckDate)
                                            || !x.CheckDate.HasValue) && !x.HasAppealCitizensInWorkState),
                            CountYellow = y.Count(
                                x =>
                                    x.TypeReminder != TypeReminder.BaseInspection
                                        && (x.CheckDate.HasValue
                                            && DateTime.Now.Date < x.CheckDate
                                            && DateTime.Now.AddDays(5).Date >= x.CheckDate)),
                            CountGreen = y.Count(
                                x =>
                                    x.TypeReminder != TypeReminder.BaseInspection
                                        && (x.CheckDate.HasValue
                                            && DateTime.Now.AddDays(5) < x.CheckDate.Value || x.HasAppealCitizensInWorkState)),
                            CountWhite = y.Count(x => x.TypeReminder == TypeReminder.BaseInspection)
                        })
                    .Select(x => new
                    {
                        TypeReminder = x.Key,
                        CountRed = x.Value.CountRed,
                        CountYellow = x.Value.CountYellow,
                        CountGreen = x.Value.CountGreen,
                        CountWhite = x.Value.CountWhite
                    })
                    .ToList();

                var totalCount = data.Count;

                return new ListDataResult(data, totalCount);
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(servInsSub);
                this.Container.Release(servReminder);
            }
        }

        /// <summary>
        /// Список напоминалок в виджете руководителя для Списка "Контроль Задач"
        /// </summary>
        public override IDataResult ListTaskControl(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var servInsSub = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = this.Container.Resolve<IDomainService<ChelyabinskReminder>>();
            var entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(AppealCits));

            var openState = this.StateRepo.GetAll().FirstOrDefault(x => x.Name == "В работе" && x.TypeId == entityInfo.TypeId);

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

                var answersInWork = this.GetInWorkAnswerQuery();

                var data =
                    servReminder.GetAll()
                        .Where(x => x.Actuality)
                        .Where(x=> x.Inspector != null)
                       .Where(x => x.AppealCits == null || (x.AppealCits.State.Id == openState.Id))
                        .Where(x => inspectorIds.Any(y => y == x.Inspector.Id))
                        .Select(x => new
                        {
                            //InspectorId = x.AppealCitsExecutant.Executant.Id,
                            //InspectorFio = x.AppealCitsExecutant.Executant.Fio,
                            InspectorId = x.Inspector.Id,
                            InspectorFio = x.Inspector.Fio,
                            x.TypeReminder,
                            ExtensTime = x.AppealCits == null? x.AppealCits.ExtensTime.GetValueOrDefault(): x.CheckDate.GetValueOrDefault(),
                            CheckDate = x.CheckDate.GetValueOrDefault(),
                            HasAppealCitizensInWorkState =
                                x.TypeReminder == TypeReminder.Statement && answersInWork
                                    .Where(y => x.AppealCitsExecutant != null && y.Executor.Id == x.AppealCitsExecutant.Controller.Id || y.Executor.Id == x.AppealCitsExecutant.Executant.Id)
                                    .Any(y => y.AppealCits == x.AppealCits)
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.InspectorId,
                            x.InspectorFio,
                            Color = x.TypeReminder != TypeReminder.BaseInspection && x.TypeReminder != TypeReminder.Protocol
                                    ? ((x.ExtensTime == DateTime.MinValue && DateTime.Now.Date >= x.CheckDate)
                                    || x.ExtensTime != DateTime.MinValue && DateTime.Now.Date >= x.ExtensTime) 
                                    && !x.HasAppealCitizensInWorkState
                                        ? "red"
                                        : ((x.ExtensTime == DateTime.MinValue && x.CheckDate < DateTime.Now.AddDays(5))
                                        || x.ExtensTime != DateTime.MinValue && x.ExtensTime < DateTime.Now.AddDays(5))
                                        && !x.HasAppealCitizensInWorkState
                                            ? "yellow"
                                            : "green"
                                    : "white"
                        })
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
            catch (Exception exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(servInsSub);
                this.Container.Release(servReminder);
            }
        }

        public override IDataResult ListReminderOfInspector(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var servReminder = this.Container.Resolve<IDomainService<ChelyabinskReminder>>();
            var entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(AppealCits));

            var openState = this.StateRepo.GetAll().FirstOrDefault(x => x.Name == "В работе" && x.TypeId == entityInfo.TypeId);

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorId = activeOperator != null && activeOperator.Inspector != null
                                    ? activeOperator.Inspector.Id
                                    : 0;

                var answersInWork = this.GetInWorkAnswerQuery();

                var data = this.GetQueryable(baseParams, servReminder)
                    .Where(x => x.Actuality)
                    .Where(x => x.AppealCits == null || (x.AppealCits.State.Id == openState.Id))
                    .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                    //.WhereIf(
                    //    inspectorId > 0,
                    //    x =>
                    //        x.AppealCitsExecutant.Author.Id == inspectorId || x.AppealCitsExecutant.Controller.Id == inspectorId
                    //            || x.AppealCitsExecutant.Executant.Id == inspectorId)
                                     .WhereIf(
                        inspectorId > 0,
                        x => x.Inspector.Id == inspectorId || (x.AppealCitsExecutant != null &&
                            x.AppealCitsExecutant.Author.Id == inspectorId || x.AppealCitsExecutant.Controller.Id == inspectorId
                                || x.AppealCitsExecutant.Executant.Id == inspectorId))
                    .Select(
                        x => new
                        {
                            x.Id,
                            InspectionGji = x.InspectionGji != null ? new {x.InspectionGji.Id, x.InspectionGji.TypeBase} : null,
                            DocumentGji = x.DocumentGji != null ? new {x.DocumentGji.Id, x.DocumentGji.TypeDocumentGji} : null,
                            AppealCits = x.AppealCits != null ? x.AppealCits.Id : 0,
                            AppealState = x.AppealCits != null ? x.AppealCits.State.Name : "",
                            AppealStateCode = x.AppealCits != null ? x.AppealCits.State.Code : "",
                            Contragent = x.Contragent.Name,
                            x.Actuality,
                            x.TypeReminder,
                            x.CategoryReminder,
                            Num = (x.Num != null && x.Num != "")
                                ? x.Num
                                : x.AppealCits != null ? x.AppealCits.DocumentNumber : "",
                            x.CheckDate,
                            DateFrom = x.DocumentGji != null ? x.DocumentGji.DocumentDate : x.AppealCits.DateFrom,
                            CheckingInspector =
                                x.TypeReminder == TypeReminder.Statement && x.AppealCitsExecutant.Controller != null
                                    ? x.AppealCitsExecutant.Controller.Fio
                                    : x.TypeReminder == TypeReminder.ActCheck || x.TypeReminder == TypeReminder.Disposal ? x.Inspector.Fio 
                                        : null,
                            HasAppealCitizensInWorkState =
                                x.TypeReminder == TypeReminder.Statement && answersInWork
                                    .Where(y => y.Executor.Id == x.AppealCitsExecutant.Controller.Id
                                        || y.Executor.Id == x.AppealCitsExecutant.Executant.Id)
                                    .Any(y => y.AppealCits == x.AppealCits)
                        })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.CheckDate)
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(servReminder);
            }
        }

        public override IDataResult GetInfo(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceInspSubscrip = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = this.Container.Resolve<IDomainService<ChelyabinskReminder>>();
            var entityInfo = this.StateProvider.GetStatefulEntityInfo(typeof(AppealCits));

            var openState = this.StateRepo.GetAll().FirstOrDefault(x => x.Name == "В работе" && x.TypeId == entityInfo.TypeId);

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
                var inspectorId = activeOperator.Inspector?.Id ?? 0;

                var inspectorIds =
                    serviceInspSubscrip.GetAll()
                        .Where(x => x.SignedInspector.Id == inspectorId)
                        .Select(x => x.Inspector.Id);

                var answersInWork = this.GetInWorkAnswerQuery();

                var data = this.GetQueryable(baseParams, servReminder)
                    .Where(x => x.Actuality)
                    .Where(x => x.AppealCits.State.Id == openState.Id)
                    .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                    .WhereIf(isHead, x => inspectorIds.Any(y => y == x.AppealCitsExecutant.Executant.Id))
                    .WhereIf(!isHead, x => x.AppealCitsExecutant.Author.Id == inspectorId || x.AppealCitsExecutant.Controller.Id == inspectorId || x.AppealCitsExecutant.Executant.Id == inspectorId)
                    .Select(x => new
                    {
                        x.Id,
                        x.CheckDate,
                        x.TypeReminder,
                        HasAppealCitizensInWorkState =
                            x.TypeReminder == TypeReminder.Statement && answersInWork
                                .Where(y => y.Executor.Id == x.AppealCitsExecutant.Controller.Id
                                        || y.Executor.Id == x.AppealCitsExecutant.Executant.Id)
                                .Any(y => y.AppealCits == x.AppealCits)
                    })
                    .Filter(loadParams, this.Container)
                    .AsEnumerable()
                    .GroupBy(x => 1, (key, value) => new
                    {
                        allTask = value.Count(),
                        comeUpToTerm = value.Count(x => x.TypeReminder != TypeReminder.BaseInspection && x.CheckDate.HasValue && DateTime.Now.Date < x.CheckDate && DateTime.Now.AddDays(5) >= x.CheckDate.Value),
                        expired = value.Count(x => x.TypeReminder != TypeReminder.BaseInspection && ((x.CheckDate.HasValue && DateTime.Now.Date >= x.CheckDate) || !x.CheckDate.HasValue) && !x.HasAppealCitizensInWorkState),
                        unformed = value.Count(x => x.TypeReminder == TypeReminder.BaseInspection)
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

                return new BaseDataResult(new
                {
                    AllTask = 0,
                    ComeUpToTerm = 0,
                    Expired = 0,
                    Unformed = 0
                });
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = exc.Message,
                    Data = new
                    {
                        AllTask = 0,
                        ComeUpToTerm = 0,
                        Expired = 0,
                        Unformed = 0
                    }
                };
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(serviceInspSubscrip);
                this.Container.Release(servReminder);
            }
        }
    }
}