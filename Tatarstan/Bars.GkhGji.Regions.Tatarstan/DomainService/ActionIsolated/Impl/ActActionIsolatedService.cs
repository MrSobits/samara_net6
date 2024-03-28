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
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Castle.Windsor;

    /// <inheritdoc />
    public class ActActionIsolatedService : IActActionIsolatedService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult SaveRealityObjects(BaseParams baseParams)
        {
            var roIds = baseParams.Params.GetAs<long[]>("roIds");
            var actId = baseParams.Params.GetAsId("actId");
            var actCheckRealityObjectDomain = this.Container.ResolveDomain<ActCheckRealityObject>();
            
            try
            {
                using (this.Container.Using(actCheckRealityObjectDomain))
                {
                    var act = new ActActionIsolated() { Id = actId };

                    roIds.ForEach(roId =>
                    {
                        actCheckRealityObjectDomain.Save(new ActCheckRealityObject()
                        {
                            ActCheck = act,
                            RealityObject = new RealityObject() { Id = roId }
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, ex.Message);
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult GetRealityObjectsList(BaseParams baseParams)
        {
            var actId = baseParams.Params.GetAsId("documentId");
            var loadParams = baseParams.GetLoadParam();
            var documentGjiChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var taskActionIsolatedRealityObjectDomain = this.Container.ResolveDomain<TaskActionIsolatedRealityObject>();
            var actCheckRealityObjectDomain = this.Container.ResolveDomain<ActCheckRealityObject>();

            using (this.Container.Using(documentGjiChildrenDomain, taskActionIsolatedRealityObjectDomain, actCheckRealityObjectDomain))
            {
                var taskId = (documentGjiChildrenDomain
                    .FirstOrDefault(x => x.Children.Id == actId &&
                        x.Parent.TypeDocumentGji == TypeDocumentGji.TaskActionIsolated)?.Parent?.Id) ?? 0;

                if (taskId > 0)
                {
                    var alreadyBindedRealityObjects = actCheckRealityObjectDomain
                        .GetAll()
                        .Where(x => x.ActCheck.Id == actId)
                        .Select(x => x.RealityObject.Id)
                        .ToArray();
                    
                    return taskActionIsolatedRealityObjectDomain
                        .GetAll()
                        .Where(x => x.Task.Id == taskId)
                        .Select(x => new
                        {
                            x.RealityObject.Id,
                            Municipality = x.RealityObject.Municipality.Name,
                            x.RealityObject.Address
                        })
                        .WhereIf(alreadyBindedRealityObjects.Any(), x => !alreadyBindedRealityObjects.Contains(x.Id))
                        .ToListDataResult(loadParams, this.Container);
                }
            }

            return new ListDataResult();
        }

        /// <inheritdoc />
        public IDataResult GetRealityObjectsForDefinition(BaseParams baseParams)
        {
            var actId = baseParams.Params.GetAsId("documentId");
            var loadParams = baseParams.GetLoadParam();
            var defenitionDomain = this.Container.ResolveDomain<ActActionIsolatedDefinition>();
            var actCheckRealityObjectDomain = this.Container.ResolveDomain<ActCheckRealityObject>();

            using (this.Container.Using(defenitionDomain, actCheckRealityObjectDomain))
            {
                var alreadyBindedRO = defenitionDomain.FirstOrDefault(x => x.Act.Id == actId)?.RealityObject;
                
                return actCheckRealityObjectDomain
                    .GetAll()
                    .Where(x => x.ActCheck.Id == actId)
                    .WhereIf(alreadyBindedRO != null, x => x.RealityObject.Id != alreadyBindedRO.Id)
                    .Select(x => new
                    {
                        x.RealityObject.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.RealityObject.Address
                    })
                    .ToListDataResult(loadParams, this.Container);
            }
        }

        /// <inheritdoc />
        public IDataResult ListForRegistry(BaseParams baseParams)
        {
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObject = baseParams.Params.GetAsId("realityObjectId");
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            var actActionIsolatedDomain = this.Container.Resolve<IDomainService<ActActionIsolated>>();
            var actCheckDomain = this.Container.Resolve<IDomainService<ActCheck>>();
            var docChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var taskActionIsolatedDomain = this.Container.Resolve<IDomainService<TaskActionIsolated>>();
            var docInspectorDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var actCheckRealityObjectDomain = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var serviceViolations = this.Container.Resolve<IDomainService<ActCheckViolation>>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(actActionIsolatedDomain, actCheckDomain, docChildrenDomain,
                taskActionIsolatedDomain, docInspectorDomain, actCheckRealityObjectDomain, serviceViolations))
            {
                var actActionFiltered = actActionIsolatedDomain.GetAll()
                    .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd);

                var actCheckRoDict = actCheckRealityObjectDomain.GetAll()
                    .Where(x => actActionFiltered.Any(y => y == x.ActCheck))
                    .WhereIf(realityObject != 0, x => x.RealityObject.Id == realityObject)
                    .Select(x => new
                    {
                        x.ActCheck.Id,
                        x.RealityObject.Municipality,
                        x.RealityObject.Address,
                        x.HaveViolation
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => new
                    {
                        MunicipalityIds = string.Join(", ", x.Select(y => y.Municipality?.Id)),
                        MunicipalityNames = string.Join(", ", x.Select(y => y.Municipality?.Name)),
                        Addresses = string.Join(", ", x.Select(y => y.Address)),
                        RealityObjectCount = x.Distinct().Count(),
                        DocumentCount = x.Count(),
                        HaveViolation = x.Any(y => y.HaveViolation == Gkh.Enums.YesNoNotSet.Yes)
                    });

                var docInspectorsDict = docInspectorDomain.GetAll()
                    .WhereIfElse(realityObject != 0,
                        x => actCheckRoDict.Keys.Contains(x.DocumentGji.Id),
                        x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.ActActionIsolated
                            && actActionFiltered.Any(y => y == x.DocumentGji))
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
                var operInspector = userManager.GetActiveOperator()?.Inspector;
                var docFilteredByInsspectorIds = operInspector != null
                    ? docInspectorsDict
                        .Where(x => x.Value.InspectorIds.Contains(operInspector.Id))
                        .Select(x => x.Key)
                        .ToList()
                        : null;

                return actActionIsolatedDomain.GetAll()
                    .WhereIfElse(realityObject != 0,
                        x => actCheckRoDict.Keys.Contains(x.Id),
                        x => actActionFiltered.Any(y => y == x))
                    .WhereIf(operInspector != null, x => docFilteredByInsspectorIds.Contains(x.Id))
                    .Join(actCheckDomain.GetAll(),
                    a => a,
                    b => b,
                    (a, b) => new
                    {
                        ActActionIsolatedId = a.Id,
                        ActCheckId = b.Id,
                        InspectionId = b.Inspection.Id,
                        b.Inspection.TypeBase,
                        b.State,
                        b.DocumentNumber,
                        b.DocumentNum,
                        b.DocumentDate
                    })
                    .Join(docChildrenDomain.GetAll(),
                    a => new { Id = a.ActCheckId, TypeDocument = TypeDocumentGji.TaskActionIsolated },
                    b => new { b.Children.Id, TypeDocument = b.Parent.TypeDocumentGji },
                    (a, b) => new
                    {
                        ParentId = b.Parent.Id,
                        a.ActActionIsolatedId,
                        a.ActCheckId,
                        a.InspectionId,
                        a.TypeBase,
                        a.State,
                        a.DocumentNumber,
                        a.DocumentNum,
                        a.DocumentDate
                    })
                    .Join(taskActionIsolatedDomain.GetAll(),
                    a => a.ParentId,
                    b => b.Id,
                    (a, b) => new
                    {
                        a.ActActionIsolatedId,
                        a.ActCheckId,
                        a.InspectionId,
                        a.TypeBase,
                        a.State,
                        TypeBaseAction = b.TypeBase,
                        ContragentName = b.Contragent.Name,
                        b.PersonName,
                        a.DocumentNumber,
                        a.DocumentNum,
                        a.DocumentDate
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var actCheckRo = actCheckRoDict.Get(x.ActCheckId);

                        return new
                        {
                            Id = x.ActActionIsolatedId,
                            x.InspectionId,
                            x.State,
                            x.TypeBase,
                            actCheckRo?.MunicipalityNames,
                            x.TypeBaseAction,
                            InspectorNames = docInspectorsDict.Get(x.ActActionIsolatedId)?.Fio,
                            RoAddresses = actCheckRo?.Addresses,
                            x.DocumentNumber,
                            x.DocumentNum,
                            x.DocumentDate,
                            x.ContragentName,
                            x.PersonName,
                            actCheckRo?.RealityObjectCount,
                            actCheckRo?.HaveViolation,
                            actCheckRo?.DocumentCount,
                            TypeDocumentGji = TypeDocumentGji.ActActionIsolated
                        };
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container, true, !isExport);
            }
        }
    }
}