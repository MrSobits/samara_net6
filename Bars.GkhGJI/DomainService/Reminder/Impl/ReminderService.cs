namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repositories;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.ConfigSections;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис напоминаний
    /// </summary>
    public class ReminderService : IReminderService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Список напоминалок в Виджете для Инспектора
        /// </summary>
        public virtual IDataResult ListWidgetInspector(BaseParams baseParams)
        {
            var servReminder = this.Container.Resolve<IDomainService<Reminder>>();
            var servInsSub = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator?.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorId = activeOperator.Inspector?.Id ?? 0;

                var data = servReminder.GetAll()
                    .Where(x => x.Actuality)
                    .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                    .WhereIf(inspectorId > 0, x => x.Inspector.Id == inspectorId)
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeReminder,
                        x.CheckDate,
                        Num = x.Num ?? "",
                        DocNum = x.AppealCits.DocumentNumber,
                        NumGji = x.AppealCits.NumberGji,
                        ContragentName = x.Contragent != null ? x.Contragent.Name : "",
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
                        CheckDate = x.CheckDate.ToDateTime(),
                        Num = x.TypeReminder == TypeReminder.Statement ? (!string.IsNullOrEmpty(x.DocNum) ? x.DocNum : x.NumGji) : x.Num,
                        NumText = x.TypeReminder == TypeReminder.Statement ? "Номер обращения" : "№ ГЖИ",
                        x.ContragentName,
                        ColorTypeReminder = this.GetColorTypeReminder(x.TypeReminder),
                        x.CategoryReminder,
                        Color = DateTime.Now >= x.CheckDate.GetValueOrDefault()
                                ? "red"
                                : x.CheckDate.GetValueOrDefault().AddDays(-5) <= DateTime.Now
                                    ? "yellow"
                                    : "green"
                    })
                    .ToList();

                return new ListDataResult(result, totalCount);
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(servReminder);
                this.Container.Release(servInsSub);
            }
        }

        /// <summary>
        /// Список напиоминалок в виджете руководителя для Списка "Состояние Задач"
        /// </summary>
        public virtual IDataResult ListTaskState(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var servInsSub = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = this.Container.Resolve<IDomainService<Reminder>>();

            try
            {
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator?.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorIds = servInsSub.GetAll()
                    .Where(x => x.SignedInspector.Id == activeOperator.Inspector.Id)
                    .Select(x => x.Inspector.Id);

                var data =
                    servReminder.GetAll()
                        .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                        .Where(x => x.Actuality)
                        .Where(x => inspectorIds.Any(y => y == x.Inspector.Id))
                        .Select(x => new
                        {
                            x.Id,
                            x.CheckDate,
                            x.TypeReminder
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.TypeReminder)
                        .ToDictionary(x => x.Key,
                            y => new
                            {
                                CountRed = y.Count(
                                    x =>
                                        x.TypeReminder != TypeReminder.BaseInspection
                                        &&
                                        ((x.CheckDate.HasValue && DateTime.Now.Date >= x.CheckDate) ||
                                         !x.CheckDate.HasValue)),
                                CountYellow = y.Count(
                                    x =>
                                        x.TypeReminder != TypeReminder.BaseInspection
                                        && x.CheckDate.HasValue
                                        && DateTime.Now.Date < x.CheckDate
                                        && DateTime.Now.AddDays(5).Date >= x.CheckDate),
                                CountGreen = y.Count(
                                    x =>
                                        x.TypeReminder != TypeReminder.BaseInspection
                                        && x.CheckDate.HasValue
                                        && DateTime.Now.AddDays(5) < x.CheckDate.Value),
                                CountWhite = y.Count(x => x.TypeReminder == TypeReminder.BaseInspection)
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
                this.Container.Release(userManager);
                this.Container.Release(servInsSub);
                this.Container.Release(servReminder);
            }
        }

        /// <summary>
        /// Список напоминалок в виджете руководителя для Списка "Контроль Задач"
        /// </summary>
        public virtual IDataResult ListTaskControl(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var servInsSub = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = this.Container.Resolve<IDomainService<Reminder>>();

            try
            {
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorIds =servInsSub.GetAll()
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
                            x.TypeReminder != TypeReminder.BaseInspection
                                ? DateTime.Now.Date >= x.CheckDate.GetValueOrDefault()
                                    ? "red"
                                    : x.CheckDate.GetValueOrDefault().AddDays(-5) < DateTime.Now
                                        ? "yellow"
                                        : "green"
                                : "white"
                        })
                        .AsEnumerable()
                        .GroupBy(
                            x => new {x.InspectorId, x.InspectorFio},
                            x => x.Color,
                            (key, value) => new {Inspector = key, Color = value})
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
                this.Container.Release(userManager);
                this.Container.Release(servInsSub);
                this.Container.Release(servReminder);
            }
        }

        /// <summary>
        /// Список напоминалок инспектора в Панели Инспектора
        /// </summary>
        public virtual IDataResult ListReminderOfInspector(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var servReminder = this.Container.Resolve<IDomainService<Reminder>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorId = activeOperator?.Inspector?.Id ?? 0;

                var data = this.GetQueryable(baseParams, servReminder)
                    .Where(x => x.Actuality)
                    .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                    .WhereIf(inspectorId > 0, x => x.Inspector.Id == inspectorId)
                    .Select(x => new
                        {
                            x.Id,
                            InspectionGji = x.InspectionGji != null ? new { x.InspectionGji.Id, x.InspectionGji.TypeBase } : null,
                            DocumentGji = x.DocumentGji != null ? new { x.DocumentGji.Id, x.DocumentGji.TypeDocumentGji } : null,
                            AppealCits = x.AppealCits != null ? x.AppealCits.Id : 0,
                            Contragent = x.Contragent.Name,
                            x.Actuality,
                            x.TypeReminder,
                            x.CategoryReminder,
							Num = (x.Num != null && x.Num != "") ? x.Num :
                                     x.AppealCits != null ? x.AppealCits.DocumentNumber : "",
                            x.CheckDate
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

        /// <summary>
        /// Список Напоминалок руководителя в Панели Руководителя
        /// </summary>
        public virtual IDataResult ListReminderOfHead(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var servInsSub = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = this.Container.Resolve<IDomainService<Reminder>>();

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
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && DateTime.Now >= x.CheckDate.GetValueOrDefault();
                        break;
                    case "yellow":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && x.CheckDate.GetValueOrDefault() > DateTime.Now && DateTime.Now.AddDays(5) > x.CheckDate.GetValueOrDefault();
                        break;
                    case "green":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && x.CheckDate.GetValueOrDefault() > DateTime.Now.AddDays(5);
                        break;
                    case "white":
                        func = x => x.TypeReminder == TypeReminder.BaseInspection;
                        break;
                    default:
                        func = x => true;
                        break;
                }

                //var cntInspectors = inspectorIds.Count();

                var data = this.GetQueryable(baseParams, servReminder)
                    .Where(x => x.Actuality)
                    .Where(x => inspectorIds.Any(y => y == x.Inspector.Id))
                    .Where(x => x.InspectionGji == null || !x.InspectionGji.State.FinalState)
                    .WhereIf(inspectorId > 0, x => x.Inspector.Id == inspectorId)
                    .WhereIf(!string.IsNullOrEmpty(colorType), func)
                    .WhereIf(isTypeReminder, x => x.TypeReminder == typeReminder)
                    .WhereIf(!string.IsNullOrEmpty(colorType), func)
                    .Select(x => new
                    {
                        x.Id,
                        InspectionGji = x.InspectionGji != null ? new {x.InspectionGji.Id, x.InspectionGji.TypeBase} : null,
                        DocumentGji = x.DocumentGji != null ? new {x.DocumentGji.Id, x.DocumentGji.TypeDocumentGji} : null,
                        AppealCits = x.AppealCits != null ? x.AppealCits.Id : 0,
                        Contragent = x.Contragent.Name,
                        x.Actuality,
                        x.TypeReminder,
                        x.CategoryReminder,
                        x.Num,
                        x.CheckDate,
                        Inspector = x.Inspector.Fio
                    })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.CheckDate)
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
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
        /// Дополнительная информация в панели Руководителяи Инспектора
        /// </summary>
        public virtual IDataResult GetInfo(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceInspSubscrip = this.Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = this.Container.Resolve<IDomainService<Reminder>>();

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
                    .Filter(loadParams, this.Container)
                    .AsEnumerable()
                    .GroupBy(x => 1, (key, value) => new
                        {
                            allTask = value.Count(),
                            comeUpToTerm = value.Count(x => x.TypeReminder != TypeReminder.BaseInspection && x.CheckDate.HasValue && DateTime.Now.Date < x.CheckDate && DateTime.Now.AddDays(5) >= x.CheckDate.Value),
                            expired = value.Count(x => x.TypeReminder != TypeReminder.BaseInspection && ((x.CheckDate.HasValue && DateTime.Now.Date >= x.CheckDate) || !x.CheckDate.HasValue)),
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

                return new BaseDataResult();
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(serviceInspSubscrip);
                this.Container.Release(servReminder);
            }
        }

        public virtual IQueryable<Reminder> GetQueryable(BaseParams baseParams, IDomainService<Reminder> service)
        {
            var query = service.GetAll();

            var dopFilter = baseParams.Params.GetAs("dopFilter", 10);
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var isStatement = baseParams.Params.GetAs("isStatement", true);
            var isDisposal = baseParams.Params.GetAs("isDisposal", true);
            var isPrescription = baseParams.Params.GetAs("isPrescription", true);
            var isBaseInspection = baseParams.Params.GetAs("isBaseInspection", true);

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
                            .Where(x => (x.CheckDate.HasValue && DateTime.Now.Date >= x.CheckDate) || !x.CheckDate.HasValue);    
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
                     .WhereIf(!isBaseInspection, x => x.TypeReminder != TypeReminder.BaseInspection);
        }

        public virtual IDataResult ListTypeReminder(BaseParams baseParams)
        {
            /*
             Поскольку в базовый енум добавляется куча типов котоыре не вовсех регионах нужны
             то тогда в этом серверном методе возвращаем типы котоыре нужны только для этого региона
            */

            var list = new List<TypeReminderProxy>();

            var disposalText = this.Container.Resolve<IDisposalText>();

            try
            {
                foreach (var type in this.ReminderTypes())
                {
                    var display = type.GetEnumMeta().Display;

                    if (type == TypeReminder.Disposal)
                    {
                        display = disposalText.SubjectiveCase;
                    }

                    list.Add(new TypeReminderProxy
                    {
                        Id = (int)type,
                        Display = display,
                        Name = type.ToString()
                    });
                }

                var total = list.Count;

                return new ListDataResult(list, total);
            }
            finally 
            {
                this.Container.Release(disposalText);
            }

        }

        /// <summary>
        /// Вернуть список ответов в работе
        /// </summary>
        /// <returns>Подзапрос</returns>
        protected IQueryable<AppealCitsAnswer> GetInWorkAnswerQuery()
        {
            var appealCits = this.Container.ResolveDomain<AppealCitsAnswer>();
            var stateRepo = this.Container.Resolve<IStateRepository>();
            var config = this.Container.GetGkhConfig<HousingInspection>();
            
            using (this.Container.Using(appealCits, stateRepo, config))
            {
                var availableStates = stateRepo.GetAllStates<AppealCitsAnswer>()
                    .Where(x => x.Name.In("В работе", "Готов ответ"))
                    .Select(x => x.Id)
                    .ToArray();

                return appealCits.GetAll()
                    .Where(x => !config.GeneralConfig.ShowStatementsWithAnswerAsOverdue)
                    .Where(x => availableStates.Contains(x.State.Id));
            }
        }

        public virtual TypeReminder[] ReminderTypes()
        {
            // Вообщем по умолчанию регистрируются только такие типы 
            // в слуаче если в регионе нобходимы другие, то тогда заменяем реализацию
            return new TypeReminder[]
                {
                    TypeReminder.Statement,
                    TypeReminder.BaseInspection, 
                    TypeReminder.Disposal, 
                    TypeReminder.Prescription
                };
        }

        protected string GetColorTypeReminder(TypeReminder type)
        {
            switch ((int)type)
            {
                case 30: { return "purple"; }
                case 40: { return "blue"; }
                default: { return "blue"; }
            }
        }

        protected class TypeReminderProxy
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Display { get; set; }
        }
    }
}