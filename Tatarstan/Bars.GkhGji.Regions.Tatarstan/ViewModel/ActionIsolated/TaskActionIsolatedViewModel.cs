namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    public class TaskActionIsolatedViewModel : BaseViewModel<TaskActionIsolated>
    {
        #region Dependency Injection
        private readonly IDomainService<DocumentGjiInspector> documentGjiInspectorDomain;
        private readonly IDomainService<TaskActionIsolatedRealityObject> taskRealityObjectDomain;
        
        public TaskActionIsolatedViewModel(IDomainService<DocumentGjiInspector> documentGjiInspectorDomain,
            IDomainService<TaskActionIsolatedRealityObject> taskRealityObjectDomain)
        {
            this.documentGjiInspectorDomain = documentGjiInspectorDomain;
            this.taskRealityObjectDomain = taskRealityObjectDomain;
        }
        #endregion
        
        // <inheritdoc />
        public override IDataResult Get(IDomainService<TaskActionIsolated> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var entity = domainService.Get(id);
            if (entity == null)
            {
                return base.Get(domainService, baseParams);
            }
            
            var taskActionIsolatedKnmActionDomain = this.Container.ResolveDomain<TaskActionIsolatedKnmAction>();

            using (this.Container.Using(taskActionIsolatedKnmActionDomain))
            {
                var inspectors = this.documentGjiInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == entity.Id)
                    .Select(x => x.Inspector)
                    .ToList();

                var plannedActions = taskActionIsolatedKnmActionDomain.GetAll()
                    .Where(x => x.MainEntity.Id == entity.Id)
                    .Select(x => x.KnmAction)
                    .ToList();

                var result = new
                {
                    entity.Id,
                    InspectionId = entity.Inspection?.Id,
                    entity.Inspection,
                    entity.Stage,
                    entity.State,
                    entity.DocumentDate,
                    entity.DocumentNumber,
                    entity.DocumentNum,
                    entity.DocumentSubNum,
                    entity.Municipality,
                    entity.ZonalInspection,
                    entity.PersonName,
                    entity.Inn,
                    entity.Contragent,
                    entity.AppealCits,
                    PlannedActions = string.Join(", ", plannedActions.Select(x => x.ActCheckActionType?.GetDisplayName())),
                    PlannedActionIds = string.Join(", ", plannedActions.Select(x => x.Id)),
                    entity.ControlType,
                    entity.DateStart,
                    entity.TimeStart,
                    entity.KindAction,
                    entity.PlanAction,
                    entity.TypeBase,
                    entity.TypeJurPerson,
                    entity.TypeObject,
                    entity.IssuedTask,
                    entity.ResponsibleExecution,
                    Inspectors = string.Join(", ", inspectors.Select(x => x.Fio)),
                    InspectorIds = string.Join(", ", inspectors.Select(x => x.Id)),
                    entity.BaseDocumentDate,
                    entity.BaseDocumentFile,
                    entity.BaseDocumentName,
                    entity.BaseDocumentNumber
                };

                return new BaseDataResult(result);
            }
        }

        // <inheritdoc />
        public override IDataResult List(IDomainService<TaskActionIsolated> domainService, BaseParams baseParams)
        {
            var plan = baseParams.Params.GetAsId("planId");
            var kind = baseParams.Params.GetAs<KindAction?>("kindAction");
            var realityObject = baseParams.Params.GetAsId("realityObjectId");
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var isClosed = baseParams.Params.GetAs<bool>("isClosed");
            var isExport = baseParams.Params.GetAs<bool>("isExport");
            var loadParams = baseParams.GetLoadParam();

            var taskIds = new HashSet<long>();
            if (realityObject != default(long))
            {
                taskIds = this.taskRealityObjectDomain.GetAll()
                    .Where(x => x.RealityObject.Id == realityObject)
                    .Select(x => x.Task.Id)
                    .Distinct()
                    .AsEnumerable()
                    .ToHashSet();
            }
            
            var inspectorsDict = this.documentGjiInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.TaskActionIsolated)
                .Select(x => new
                {
                    x.DocumentGji.Id,
                    x.Inspector.Fio
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Fio).AggregateWithSeparator(", "));
            
            var data = domainService.GetAll()
                .WhereIf(taskIds.Any(), x => taskIds.Contains(x.Id))
                .WhereIf(isClosed, x => x.State != null && x.State.FinalState)
                .WhereIf(plan != default(long), x => x.TypeBase == TypeBaseAction.Plan && x.PlanAction != null && x.PlanAction.Id == plan)
                .WhereIf(dateStart != DateTime.MinValue, x => x.DateStart >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DateStart <= dateEnd)
                .WhereIf(kind != null, x => x.KindAction == kind)
                .Select(x => new
                {
                    x.Id,
                    InspectionId = x.Inspection != null ? x.Inspection.Id : 0,
                    x.State,
                    x.DocumentNumber,
                    x.KindAction,
                    Municipality = x.Municipality != null ? x.Municipality.Name : string.Empty,
                    x.DateStart,
                    x.TypeBase,
                    IsPlanDone = x.TypeBase == TypeBaseAction.Plan,
                    x.TypeObject,
                    x.PersonName,
                    Contragent = x.Contragent != null ? x.Contragent.Name : string.Empty
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.InspectionId,
                    x.State,
                    x.DocumentNumber,
                    x.KindAction,
                    x.Municipality,
                    x.DateStart,
                    x.TypeBase,
                    x.IsPlanDone,
                    x.TypeObject,
                    x.PersonName,
                    x.Contragent,
                    Inspectors = inspectorsDict.Get(x.Id)
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            var orderField = loadParams.Order.FirstOrDefault(x => x.Name == "State");

            data = orderField != null
                ? orderField.Asc
                    ? data.OrderBy(x => x.State.Code)
                    : data.OrderByDescending(x => x.State.Code)
                : data.Order(loadParams);

            return new ListDataResult(isExport ? data.ToList() : data.Paging(loadParams).ToList(), totalCount);
        }

        /// <summary>
        /// Получить список для реестра документов ГЖИ
        /// </summary>
        public IDataResult ListForDocumentRegistry(IDomainService<TaskActionIsolated> domain, BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("realityObjectId");
            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var isExport = baseParams.Params.GetAs<bool>("isExport");
            
            var documentLinkDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var actCheckRoDomain = this.Container.ResolveDomain<ActCheckRealityObject>();

            using (this.Container.Using(documentLinkDomain, actCheckRoDomain))
            {
                var doneDict = documentLinkDomain.GetAll()
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActActionIsolated)
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.TaskActionIsolated)
                    .Join(actCheckRoDomain.GetAll(),
                        x => x.Children.Id,
                        y => y.ActCheck.Id,
                        (link, actCheckRo) => new
                        {
                            link.Parent.Id,
                            actCheckRo.HaveViolation
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.HaveViolation)
                    .ToDictionary(x => x.Key,
                        x => x.All(y => y != YesNoNotSet.NotSet));

                return domain.GetAll()
                    .SelectMany(x => this.taskRealityObjectDomain.GetAll()
                            .Where(y => y.Task.Id == x.Id)
                            .DefaultIfEmpty(),
                        (task, link) => new
                        {
                            Task = task,
                            link.RealityObject
                        })
                    .WhereIf(roId != default(long), x => x.RealityObject != null && x.RealityObject.Id == roId)
                    .WhereIf(dateStart.HasValue, x => x.Task.DateStart >= dateStart)
                    .WhereIf(dateEnd.HasValue, x => x.Task.DateStart <= dateEnd)
                    .SelectMany(x => this.documentGjiInspectorDomain.GetAll()
                            .Where(y => y.DocumentGji.Id == x.Task.Id)
                            .DefaultIfEmpty(),
                        (set, docInspector) => new
                        {
                            set.Task.Id,
                            set.Task.State,
                            set.Task.Inspection.TypeBase,
                            Municipality = set.Task.Municipality != null ? set.Task.Municipality.Name : null,
                            TypeBaseAction = set.Task.TypeBase,
                            set.Task.KindAction,
                            ControlType = set.Task.ControlType != null ? set.Task.ControlType.Name : null,
                            set.Task.DocumentNumber,
                            set.Task.DocumentDate,
                            JurPerson = set.Task.Contragent != null ? set.Task.Contragent.Name : null,
                            PhysicalPerson = set.Task.PersonName,
                            set.Task.DateStart,
                            RealityObject = set.RealityObject != null ? set.RealityObject.Address : null,
                            Inspector = docInspector != null ? docInspector.Inspector.Fio : null,
                            TypeDocumentGji = set.Task.TypeDocumentGji,
                            InspectionId = set.Task.Inspection.Id
                        })
                    .AsEnumerable()
                    .GroupBy(x => new
                        {
                            x.Id,
                            x.State,
                            x.Municipality,
                            x.TypeBase,
                            x.KindAction,
                            x.ControlType,
                            x.DocumentNumber,
                            x.DocumentDate,
                            x.JurPerson,
                            x.PhysicalPerson,
                            x.DateStart,
                            x.TypeBaseAction,
                            x.TypeDocumentGji,
                            x.InspectionId
                        },
                        (x, y) => new
                        {
                            x.Id,
                            x.State,
                            x.Municipality,
                            x.TypeBase,
                            x.KindAction,
                            x.ControlType,
                            x.DocumentNumber,
                            x.DocumentDate,
                            x.JurPerson,
                            x.PhysicalPerson,
                            x.DateStart,
                            x.TypeBaseAction,
                            x.TypeDocumentGji,
                            x.InspectionId,
                            Address = y.DistinctBy(z => z.RealityObject).AggregateWithSeparator(z => z.RealityObject, ", "),
                            CountOfHouses = y.DistinctBy(z => z.RealityObject).Count(z => !string.IsNullOrEmpty(z.RealityObject)),
                            Inspectors = y.DistinctBy(z => z.Inspector).AggregateWithSeparator(z => z.Inspector, ", "),
                            Done = doneDict.Get(x.Id)
                        })
                    .ToListDataResult(baseParams.GetLoadParam(), usePersistentObjectOrdering: true, usePaging: !isExport);
            }
        }
    }
}