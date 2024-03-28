namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.InspectionActionIsolated
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;

    public class InspectionActionIsolatedViewModel : BaseViewModel<InspectionActionIsolated>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<InspectionActionIsolated> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var entity = domainService.Get(id);
            if (entity == null)
            {
                return base.Get(domainService, baseParams);
            }

            var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();
            var actActionIsolatedDomain = this.Container.ResolveDomain<ActActionIsolated>();
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();

            using (this.Container.Using(taskActionIsolatedDomain, actActionIsolatedDomain, inspectionRiskDomain))
            {
                var taskActionIsolated = taskActionIsolatedDomain.GetAll()
                    .FirstOrDefault(x => x.Inspection.Id == entity.ActionIsolated.Id);

                var actActionIsolated = actActionIsolatedDomain.GetAll()
                    .FirstOrDefault(x => x.Inspection.Id == entity.ActionIsolated.Id);

                var risk = inspectionRiskDomain.GetAll().Where(x => x.Inspection.Id == id).FirstOrDefault(x => !x.EndDate.HasValue);

                var result = new
                {
                    entity.Id,
                    entity.State,
                    ContragentId = taskActionIsolated?.Contragent?.Id,
                    taskActionIsolated?.TypeObject,
                    entity.TypeJurPerson,
                    JurPerson = taskActionIsolated?.Contragent?.Name ?? string.Empty,
                    entity.InspectionNumber,
                    entity.CheckDate,
                    entity.TypeForm,
                    ActionIsolated = $"{actActionIsolated?.DocumentNumber} {actActionIsolated?.DocumentDate?.ToString("dd.MM.yyyy")}",
                    taskActionIsolated?.PersonName,
                    taskActionIsolated?.Inn,
                    risk?.RiskCategory,
                    RiskCategoryStartDate = risk?.StartDate
                };

                return new BaseDataResult(result);
            }
        }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<InspectionActionIsolated> domainService, BaseParams baseParams)
        {
            var realityObject = baseParams.Params.GetAsId("realityObjectId");
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var isClosed = baseParams.Params.GetAs<bool>("isClosed");
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            var inspectionRobjectDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var docInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var disposalDomain = this.Container.ResolveDomain<Disposal>();
            var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();
            var documentGjiDomain = this.Container.ResolveDomain<DocumentGji>();

            using (this.Container.Using(inspectionRobjectDomain, documentGjiDomain,
                docInspectorDomain, disposalDomain, taskActionIsolatedDomain))
            {
                var inspections = domainService.GetAll()
                    .WhereIf(isClosed, x => x.State.FinalState)
                    .WhereIf(dateStart != DateTime.MinValue, x => x.CheckDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.CheckDate <= dateEnd);

                var inspRobjectsDict = inspectionRobjectDomain.GetAll()
                    .Where(x => inspections.Any(y => y.Id == x.Inspection.Id))
                    .WhereIf(realityObject != default(long), x => x.RealityObject.Id == realityObject)
                    .Select(x => new
                    {
                        x.Inspection.Id,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        x.RealityObject.Address
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        x => x.Select(y => new
                            {
                                y.MunicipalityName,
                                y.Address
                            })
                            .ToList());

                var disposals = disposalDomain.GetAll()
                    .Where(x => x.Stage.TypeStage == TypeStage.Disposal &&
                        inspections.Any(y => y.Id == x.Stage.Inspection.Id));

                var inspectorsDict = docInspectorDomain.GetAll()
                    .Where(x => disposals.Any(y => y.Id == x.DocumentGji.Id))
                    .Select(x => new
                    {
                        x.DocumentGji.Inspection.Id,
                        x.Inspector.Fio
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        x => string.Join(", ", x.Select(y => y.Fio).ToList()));

                return (from a in taskActionIsolatedDomain.GetAll()
                        join b in documentGjiDomain.GetAll() on a.Id equals b.Id
                        join c in domainService.GetAll() on b.Inspection.Id equals c.ActionIsolated.Id
                        select new
                        {
                            c.Id,
                            c.State,
                            a.TypeObject,
                            a.TypeJurPerson,
                            JurPerson = a.Contragent.Name,
                            c.InspectionNumber,
                            c.CheckDate,
                            c.TypeForm
                        })
                    .WhereIf(isClosed, x => x.State.FinalState)
                    .WhereIf(dateStart != DateTime.MinValue, x => x.CheckDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.CheckDate <= dateEnd)
                    .WhereIfContains(realityObject != default(long), x => x.Id, inspRobjectsDict.Keys)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.TypeObject,
                        x.TypeJurPerson,
                        x.JurPerson,
                        x.InspectionNumber,
                        x.CheckDate,
                        Municipality = inspRobjectsDict.ContainsKey(x.Id) ? string.Join("; ", inspRobjectsDict[x.Id].Select(y => y.MunicipalityName).Distinct()) : string.Empty,
                        x.TypeForm,
                        Inspectors = inspectorsDict.ContainsKey(x.Id) ? inspectorsDict[x.Id] : string.Empty,
                        Address = inspRobjectsDict.ContainsKey(x.Id) ? string.Join("; ", inspRobjectsDict[x.Id].Select(y => y.Address).Distinct()) : string.Empty

                    })
                    .ToListDataResult(this.GetLoadParam(baseParams), this.Container, true, !isExport);
            }
        }
    }
}