namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.InspectionPreventiveAction
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
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionPreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Модель представления для <see cref="InspectionPreventiveAction"/>
    /// </summary>
    public class InspectionPreventiveActionViewModel : BaseViewModel<InspectionPreventiveAction>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<InspectionPreventiveAction> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var entity = domainService.Get(id);

            var preventiveActionDomain = this.Container.ResolveDomain<PreventiveAction>();
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();

            using (this.Container.Using(preventiveActionDomain, inspectionRiskDomain))
            {
                var preventiveAction = preventiveActionDomain
                    .FirstOrDefault(x => x.Inspection.Id == entity.PreventiveAction.Id);

                var risk = inspectionRiskDomain.GetAll()
                    .FirstOrDefault(x => x.Inspection.Id == id && !x.EndDate.HasValue);

                var result = new
                {
                    entity.Id,
                    entity.State,
                    TypeObject = TypeDocObject.Legal,
                    TypeJurPerson = preventiveAction?.ControlledPersonType,
                    JurPerson = preventiveAction?.ControlledOrganization?.Name ?? string.Empty,
                    entity.InspectionNumber,
                    entity.CheckDate,
                    entity.TypeForm,
                    PreventiveAction = $"{preventiveAction?.DocumentNumber} {preventiveAction?.DocumentDate?.ToString("dd.MM.yyyy")}",
                    risk?.RiskCategory,
                    RiskCategoryStartDate = risk?.StartDate
                };

                return new BaseDataResult(result);
            }
        }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<InspectionPreventiveAction> domainService, BaseParams baseParams)
        {
            var realityObject = baseParams.Params.GetAsId("realityObjectId");
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var isClosed = baseParams.Params.GetAs<bool>("isClosed");
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            var documentDomain = this.Container.ResolveDomain<DocumentGji>();
            var preventiveActionDomain = this.Container.ResolveDomain<PreventiveAction>();
            var inspectionRoDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var documentInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();

            using (this.Container.Using(documentDomain, preventiveActionDomain, inspectionRoDomain, documentInspectorDomain))
            {
                var filteredInspections = domainService.GetAll()
                    .WhereIf(isClosed, x => x.State.FinalState)
                    .WhereIf(dateStart != DateTime.MinValue, x => x.CheckDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.CheckDate <= dateEnd);

                var roDict = inspectionRoDomain.GetAll()
                    .WhereIf(realityObject != default(long), x => x.RealityObject.Id == realityObject)
                    .Where(x => filteredInspections.Any(y => y.Id == x.Inspection.Id))
                    .GroupBy(x => x.Inspection.Id)
                    .ToDictionary(x => x.Key,
                        x => new
                        {
                            Municipality = x.First().RealityObject.Municipality.Name,
                            Address = x.AggregateWithSeparator(y => y.RealityObject.Address, "<br>")
                        });

                var inspectorDict = (from doc in documentDomain.GetAll()
                            .Where(x => x.TypeDocumentGji == TypeDocumentGji.Decision)
                            .Where(x => filteredInspections.Any(y => y.Id == x.Inspection.Id))
                        join docInspector in documentInspectorDomain.GetAll() on doc.Id equals docInspector.DocumentGji.Id
                        group docInspector.Inspector.Fio by doc.Inspection.Id)
                    .ToDictionary(x => x.Key, x => x.AggregateWithSeparator(y => y, ", "));

                return (from action in preventiveActionDomain.GetAll()
                    join document in documentDomain.GetAll() 
                        on action.Id equals document.Id
                    join inspection in domainService.GetAll() 
                        on document.Inspection.Id equals inspection.PreventiveAction.Id
                    select new
                    {
                        inspection.Id,
                        inspection.State,
                        inspection.InspectionNumber,
                        inspection.CheckDate,
                        inspection.TypeForm,
                        JurPerson = action.ControlledOrganization.Name,
                        TypeJurPerson = action.ControlledPersonType
                    })
                    .WhereIf(realityObject != default(long), x => roDict.Keys.Contains(x.Id))
                    .WhereIf(isClosed, x => x.State.FinalState)
                    .WhereIf(dateStart != DateTime.MinValue, x => x.CheckDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.CheckDate <= dateEnd)
                    .AsEnumerable()
                    .Select(x => new
                        {
                            x.Id,
                            x.State,
                            x.InspectionNumber,
                            x.CheckDate,
                            x.TypeForm,
                            x.JurPerson,
                            x.TypeJurPerson,
                            roDict.Get(x.Id)?.Municipality,
                            roDict.Get(x.Id)?.Address,
                            Inspectors = inspectorDict.Get(x.Id),
                            TypeObject = TypeDocObject.Legal
                        })
                    .ToListDataResult(this.GetLoadParam(baseParams), this.Container, usePaging: !isExport);
            }
        }
    }
}