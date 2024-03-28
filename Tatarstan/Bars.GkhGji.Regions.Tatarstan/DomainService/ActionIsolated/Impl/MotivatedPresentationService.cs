namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Windsor;

    public class MotivatedPresentationService : IMotivatedPresentationService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult GetInspectionInfoList(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            var motivatedPresentationActionIsolatedService = this.Container.Resolve<IDomainService<MotivatedPresentation>>();
            var disposalActionIsolatedService = this.Container.Resolve<IDomainService<Disposal>>();
            var inspectionActionIsolatedService = this.Container.Resolve<IDomainService<InspectionActionIsolated>>();
            var inspectionGjiRealityObjectService = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            using (this.Container.Using(motivatedPresentationActionIsolatedService,
                inspectionActionIsolatedService, disposalActionIsolatedService, inspectionGjiRealityObjectService))
            {
                var motivatedPresentation = motivatedPresentationActionIsolatedService.Get(documentId);

                var inspectionList = inspectionActionIsolatedService
                    .GetAll()
                    .Where(x => x.ActionIsolated.Id == motivatedPresentation.Inspection.Id);

                var disposalList = disposalActionIsolatedService
                    .GetAll()
                    .Where(x => x.Stage.TypeStage == TypeStage.Disposal &&
                        inspectionList.Any(y => y.Id == x.Stage.Inspection.Id))
                    .ToList();

                return inspectionGjiRealityObjectService
                    .GetAll()
                    .Where(x => inspectionList.Any(y => y.Id == x.Inspection.Id))
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var disposal = disposalList
                            .FirstOrDefault(y => y.Stage.Inspection.Id == x.Inspection.Id);

                        return new
                        {
                            disposal?.DateStart,
                            disposal?.DateEnd,
                            Address = x.RealityObject.Address ?? "",
                            InspectionNumber = x.Inspection.InspectionNumber ?? "",
                            disposal?.TimeVisitStart,
                            disposal?.TimeVisitEnd,
                            InspectionId = x.Inspection.Id
                        };
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }

        /// <inheritdoc />
        public IDataResult GetViolationInfoList(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            var violationService = this.Container
                .Resolve<IDomainService<MotivatedPresentationViolation>>();

            using (this.Container.Using(violationService))
            {
                return violationService
                    .GetAll()
                    .Where(x => x.MotivatedPresentationRealityObject.MotivatedPresentation.Id == documentId)
                    .AsEnumerable()
                    .GroupBy(x => x.MotivatedPresentationRealityObject.RealityObject)
                    .Select(x => new
                    {
                        x.Key.Address,
                        Violations = string.Join(", ", x.Select(y => y.Violation.Name))
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }

        /// <inheritdoc />
        public IDataResult GetNewInspectionBasementInfo(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var parentDocumentType = baseParams.Params.GetAs<TypeDocumentGji>("parentDocumentType");

            var motivatedPresentationActionIsolatedDomain = this.Container.ResolveDomain<MotivatedPresentation>();

            using (this.Container.Using(motivatedPresentationActionIsolatedDomain))
            {
                var motivatedPresentation = motivatedPresentationActionIsolatedDomain.Get(documentId);

                if (motivatedPresentation.IsNull())
                {
                    return new BaseDataResult(false, "Не удалось найти мотивированное представление");
                }

                switch (parentDocumentType)
                {
                    case TypeDocumentGji.ActActionIsolated:
                        var actActionIsolatedDomain = this.Container.ResolveDomain<ActActionIsolated>();
                        var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();

                        using (this.Container.Using(actActionIsolatedDomain, taskActionIsolatedDomain))
                        {
                            var actActionIsolated = actActionIsolatedDomain.FirstOrDefault(x => x.Inspection.Id == motivatedPresentation.Inspection.Id);
                            var taskActionIsolated = taskActionIsolatedDomain.FirstOrDefault(x => x.Inspection.Id == motivatedPresentation.Inspection.Id);

                            return this.AllParentDocumentExistsCheck(actActionIsolated, taskActionIsolated) ?? new BaseDataResult(new
                            {
                                ActionIsolated = taskActionIsolated.Inspection.Id,
                                NumberAndDocumentDate = $"{actActionIsolated.DocumentNumber} {actActionIsolated.DocumentDate?.ToString("dd.MM.yyyy")}",
                                taskActionIsolated.TypeObject,
                                taskActionIsolated.TypeJurPerson,
                                JurPerson = taskActionIsolated.Contragent?.Name ?? string.Empty,
                                taskActionIsolated.PersonName
                            });
                        }
                    case TypeDocumentGji.VisitSheet:
                        var preventiveActionDomain = this.Container.ResolveDomain<PreventiveAction>();

                        using (this.Container.Using(preventiveActionDomain))
                        {
                            var preventiveAction = preventiveActionDomain.FirstOrDefault(x => x.Inspection.Id == motivatedPresentation.Inspection.Id);

                            return this.AllParentDocumentExistsCheck(preventiveAction) ?? new BaseDataResult(new
                            {
                                PreventiveAction = preventiveAction.Inspection.Id,
                                NumberAndDocumentDate = $"{preventiveAction.DocumentNumber} {preventiveAction.DocumentDate?.ToString("dd.MM.yyyy")}",
                                TypeObject = TypeDocObject.Legal,
                                TypeJurPerson = preventiveAction.ControlledPersonType,
                                JurPerson = preventiveAction.ControlledOrganization?.Name ?? string.Empty
                            });
                        }
                    default:
                        return new BaseDataResult(false, "Не удалось определить родительский документ мотивированного представления");
                }
            }
        }

        /// <summary>
        /// Проверить наличие всех родительских документов
        /// </summary>
        private BaseDataResult AllParentDocumentExistsCheck(params DocumentGji[] documents) =>
            documents.Any(x => x.IsNull())
                ? new BaseDataResult(false, "Не удалось найти один из родительских документов мотивированного представления")
                : null;

        /// <inheritdoc />
        public IDataResult ListForRegistry(BaseParams baseParams)
        {
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObject = baseParams.Params.GetAsId("realityObjectId");
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            var mpDomain = this.Container.Resolve<IDomainService<MotivatedPresentation>>();
            var mpRealityObjectDomain = this.Container.Resolve<IDomainService<MotivatedPresentationRealityObject>>();
            var docInspectorDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var docChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(mpDomain, mpRealityObjectDomain, docInspectorDomain, docChildrenDomain))
            {
                var mpFiltered = mpDomain.GetAll()
                    .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd);

                var mpRoDict = mpRealityObjectDomain.GetAll()
                    .Where(x => mpFiltered.Any(y => y == x.MotivatedPresentation))
                    .WhereIf(realityObject != 0, x => x.RealityObject.Id == realityObject)
                    .Select(x => new
                    {
                        x.MotivatedPresentation.Id,
                        x.RealityObject.Municipality,
                        x.RealityObject.Address
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => new
                    {
                        MunicipalityNames = string.Join(", ", x.Select(y => y.Municipality?.Name).Distinct()),
                        Addresses = string.Join(", ", x.Select(y => y.Address)),
                        RealityObjectCount = x.Distinct().Count()
                    });

                var docInspectorsDict = docInspectorDomain.GetAll()
                    .WhereIfElse(realityObject != 0,
                        x => mpRoDict.Keys.Contains(x.DocumentGji.Id),
                        x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.MotivatedPresentation
                            && mpFiltered.Any(y => y == x.DocumentGji))
                    .Select(x => new
                    {
                        x.DocumentGji.Id,
                        InspectorId = x.Inspector.Id,
                        x.Inspector.Fio
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => new
                    {
                        InspectorIds = y.Select(x => x.InspectorId),
                        Fio = y.Select(x => x.Fio).AggregateWithSeparator(", ")
                    });

                // Если у текущего пользователя указан инспектор, то фильтруем по нему
                var operatorInspector = userManager.GetActiveOperator()?.Inspector;
                var docFilteredByInspectorIds = operatorInspector != null
                    ? docInspectorsDict
                        .Where(x => x.Value.InspectorIds.Contains(operatorInspector.Id))
                        .Select(x => x.Key)
                        .ToList()
                        : null;

                return mpFiltered
                    .WhereIf(realityObject != 0, x => mpRoDict.Keys.Contains(x.Id))
                    .WhereIf(operatorInspector != null, x => docFilteredByInspectorIds.Contains(x.Id))
                    .Join(docChildrenDomain.GetAll(),
                    x => x.Id,
                    y => y.Children.Id,
                    (x, y) => new
                    {
                        x.Id,
                        x.State,
                        InspectionId = y.Parent.Inspection.Id,
                        y.Parent.Inspection.TypeBase,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.DocumentDate
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var mpRo = mpRoDict.Get(x.Id);

                        return new
                        {
                            x.Id,
                            x.State,
                            x.TypeBase,
                            x.InspectionId,
                            mpRo?.MunicipalityNames,
                            InspectorNames = docInspectorsDict.Get(x.Id)?.Fio,
                            RoAddresses = mpRo?.Addresses,
                            x.DocumentNumber,
                            x.DocumentDate,
                            mpRo?.RealityObjectCount,
                            TypeDocumentGji = TypeDocumentGji.MotivatedPresentation
                        };
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container, usePersistentObjectOrdering: true, usePaging: !isExport);
            }
        }
    }
}