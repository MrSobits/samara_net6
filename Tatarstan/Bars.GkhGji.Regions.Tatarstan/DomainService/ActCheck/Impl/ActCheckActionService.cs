namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActCheck.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;
    using Bars.GkhGji.Regions.Tatarstan.Enums;
    using Castle.Core.Internal;
    using Castle.Windsor;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Domain;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Disposal;

    public class ActCheckActionService : IActCheckActionService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddInspectors(BaseParams baseParams)
        {
            var actCheckActionInspectorService = this.Container.Resolve<IDomainService<ActCheckActionInspector>>();
            var achCheckActionService = this.Container.Resolve<IDomainService<ActCheckAction>>();

            try
            {
                var actCheckActionId = baseParams.Params.GetAsId("actCheckActionId");
                var inspectorIds = baseParams.Params.GetAs<string>("inspectorIds");
                var newActionInspectorList = new List<ActCheckActionInspector>();

                if (inspectorIds.IsNullOrEmpty() || actCheckActionId == 0)
                    return new BaseDataResult();

                var inspectorsDict = actCheckActionInspectorService
                    .GetAll()
                    .Where(x => x.ActCheckAction.Id == actCheckActionId)
                    .GroupBy(x => x.Inspector.Id)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                var actCheckAction = achCheckActionService
                    .GetAll()
                    .FirstOrDefault(x => x.Id == actCheckActionId);

                foreach (var id in inspectorIds.Split(","))
                {
                    var inspectorId = id.ToLong();

                    if (inspectorsDict.ContainsKey(inspectorId))
                    {
                        inspectorsDict.Remove(inspectorId);
                        continue;
                    }

                    if (inspectorId > 0)
                    {
                        var newObj = new ActCheckActionInspector()
                        {
                            ActCheckAction = actCheckAction,
                            Inspector = new Inspector { Id = inspectorId }
                        };

                        newActionInspectorList.Add(newObj);
                    }
                }

                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        foreach (var item in newActionInspectorList)
                        {
                            actCheckActionInspectorService.Save(item);
                        }

                        foreach (var value in inspectorsDict.Values)
                        {
                            actCheckActionInspectorService.Delete(value.Id);
                        }

                        transaction.Commit();
                        return new BaseDataResult();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                this.Container.Release(actCheckActionInspectorService);
                this.Container.Release(achCheckActionService);
            }
        }

        /// <inheritdoc />
        public IDataResult AddCarriedOutEvents(BaseParams baseParams)
        {
            var actCheckActionInspectorService = this.Container.Resolve<IDomainService<ActCheckActionCarriedOutEvent>>();
            var achCheckActionService = this.Container.Resolve<IDomainService<ActCheckAction>>();

            try
            {
                var actCheckActionId = baseParams.Params.GetAsId("actCheckActionId");
                var enumValues = baseParams.Params.GetAs<string>("enumValues");
                var newActionCarriedOutEventList = new List<ActCheckActionCarriedOutEvent>();

                if (enumValues.IsNull() || actCheckActionId == 0)
                    return new BaseDataResult();

                var eventTypesDict = actCheckActionInspectorService
                    .GetAll()
                    .Where(x => x.ActCheckAction.Id == actCheckActionId)
                    .GroupBy(x => x.EventType)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                var actCheckAction = achCheckActionService.GetAll().FirstOrDefault(x => x.Id == actCheckActionId);

                foreach (var id in enumValues.Split(","))
                {
                    var newEventType = (ActCheckActionCarriedOutEventType)id.ToInt();

                    if (eventTypesDict.ContainsKey(newEventType))
                    {
                        eventTypesDict.Remove(newEventType);
                        continue;
                    }

                    if (newEventType > 0)
                    {
                        var newObj = new ActCheckActionCarriedOutEvent()
                        {
                            ActCheckAction = actCheckAction,
                            EventType = newEventType
                        };

                        newActionCarriedOutEventList.Add(newObj);
                    }
                }

                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        foreach (var item in newActionCarriedOutEventList)
                        {
                            actCheckActionInspectorService.Save(item);
                        }

                        foreach (var value in eventTypesDict.Values)
                        {
                            actCheckActionInspectorService.Delete(value.Id);
                        }

                        transaction.Commit();
                        return new BaseDataResult();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                this.Container.Release(actCheckActionInspectorService);
                this.Container.Release(achCheckActionService);
            }
        }

        /// <inheritdoc />
        public IList GetActionTypes(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            var docChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var decisionKnmActionDomain = this.Container.ResolveDomain<DecisionKnmAction>();
            var taskActionIsolatedKnmActionDomain = this.Container.ResolveDomain<TaskActionIsolatedKnmAction>();

            using (this.Container.Using(docChildrenDomain,
                decisionKnmActionDomain, taskActionIsolatedKnmActionDomain))
            {
                var parentDocument = docChildrenDomain
                    .FirstOrDefault(x => x.Children.Id == documentId)
                    ?.Parent;

                IQueryable<KnmAction> query = null;

                switch (parentDocument?.TypeDocumentGji)
                {
                    case TypeDocumentGji.Decision:
                        query = decisionKnmActionDomain.GetAll()
                            .Where(x => x.MainEntity.Id == parentDocument.Id)
                            .Select(x => x.KnmAction);
                        break;
                    case TypeDocumentGji.TaskActionIsolated:
                        var availableTypes = new ActCheckActionType?[]
                        {
                            ActCheckActionType.Inspection,
                            ActCheckActionType.InstrumentalExamination
                        };
                        query = taskActionIsolatedKnmActionDomain.GetAll()
                            .Where(x => x.MainEntity.Id == parentDocument.Id)
                            .Where(x => availableTypes.Contains(x.KnmAction.ActCheckActionType))
                            .Select(x => x.KnmAction);
                        break;
                    default:
                        return new List<string>();
                }

                return query
                    .Where(x => x.ActCheckActionType != null)
                    .Select(x => x.ActCheckActionType)
                    .Distinct()
                    .AsEnumerable()
                    .Select(x => new
                    {
                        Display = x.Value.GetDisplayName(),
                        Value = (int)x.Value
                    })
                    .ToList();
            }
        }
    }
}