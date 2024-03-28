namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System;
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
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    public class PreventiveActionViewModel : BaseViewModel<PreventiveAction>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<PreventiveAction> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var planId = baseParams.Params.GetAsId("Plan");
            var controlledOrganizationId = baseParams.Params.GetAsId("ControlledOrganization");
            var actionType = baseParams.Params.GetAs<PreventiveActionType?>("ActionType");
            var periodStart = baseParams.Params.GetAs("PeriodStart", DateTime.MinValue);
            var periodEnd = baseParams.Params.GetAs("PeriodEnd", DateTime.MaxValue);
            var showAllClosed = baseParams.Params.GetAs<bool>("ShowClosed");

            return domainService
                .GetAll()
                .WhereIf(showAllClosed, x => x.State.FinalState)
                .WhereIf(planId != 0, x => x.Plan != null && x.Plan.Id == planId)
                .WhereIf(controlledOrganizationId != 0, x => x.ControlledOrganization != null && x.ControlledOrganization.Id == controlledOrganizationId)
                .WhereIf(periodStart != DateTime.MinValue || periodEnd != DateTime.MaxValue, x => x.DocumentDate >= periodStart && x.DocumentDate <= periodEnd)
                .WhereIf(actionType.HasValue, x => x.ActionType == actionType)
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    Municipality = x.Municipality.Name,
                    x.DocumentNumber,
                    x.ActionType,
                    x.VisitType,
                    x.DocumentDate,
                    InspectionId = x.Inspection.Id,
                    ControlledOrganization = x.ControlledOrganization != null ? x.ControlledOrganization.Name : string.Empty
                })
                .ToListDataResult(loadParams, Container);
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<PreventiveAction> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var entity = domainService.Get(id);
            if (entity == null)
            {
                return base.Get(domainService, baseParams);
            }

            string inspectors = string.Empty;
            string inspectorsIds = string.Empty;
            IDomainService<DocumentGjiInspector> docInspectorDomain;
            using (this.Container.Using(docInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>()))
            {
                var inspector = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == entity.Id)
                    .Select(x => x.Inspector)
                    .ToList();

                inspectors = string.Join(", ", inspector.Select(x=>x.Fio));
                inspectorsIds = string.Join(", ", inspector.Select(x => x.Id));
            }
            
            var result = new
            {
                entity.Id,
                InspectionId = entity.Inspection?.Id,
                entity.Inspection,
                entity.Stage,
                entity.State,
                entity.DocumentDate,
                entity.DocumentYear,
                entity.DocumentNumber,
                entity.DocumentNum,
                entity.DocumentSubNum,
                entity.Municipality,
                Inspectors = inspectors,
                InspectorIds = inspectorsIds,
                entity.File,
                entity.FileDate,
                entity.FileName,
                entity.FileNumber,
                MunicipalityName =  entity.Municipality.Name,
                GjiContragentName = entity.ZonalInspection.ZoneName,
                entity.Head,
                entity.Plan,
                entity.ActionType,
                entity.VisitType,
                entity.FullName,
                entity.PhoneNumber,
                entity.ControlledPersonAddress,
                entity.ControlledPersonType,
                entity.ControlledOrganization,
                entity.ZonalInspection,
                entity.ControlType,
                entity.ErknmGuid,
                entity.ErknmRegistrationNumber,
                entity.ErknmRegistrationDate,
                SentToErknm = string.IsNullOrEmpty(entity.ErknmGuid) ? YesNo.No : YesNo.Yes
            };

            return new BaseDataResult(result);
        }

        /// <summary>
        /// Получить список для реестра документов ГЖИ
        /// </summary>
        public IDataResult ListForDocumentRegistry(IDomainService<PreventiveAction> domainService, BaseParams baseParams)
        {
            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            var documentGjiInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var documentGjiChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var preventiveActionTaskDomain = this.Container.ResolveDomain<PreventiveActionTask>();

            using (this.Container.Using(documentGjiInspectorDomain, documentGjiChildrenDomain, preventiveActionTaskDomain))
            {
                var query = domainService.GetAll()
                    .WhereIf(dateStart.HasValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd.HasValue, x => x.DocumentDate <= dateEnd);

                var inspectorsDict = documentGjiInspectorDomain.GetAll()
                    .Where(x => query.Any(y => y.Id == x.DocumentGji.Id))
                    .Select(x => new
                    {
                        x.DocumentGji.Id,
                        x.Inspector.Fio
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        y => y.Select(x => x.Fio).AggregateWithSeparator(", "));

                return query
                    .SelectMany(x => documentGjiChildrenDomain.GetAll()
                        .Where(y => y.Parent.Id == x.Id)
                        .Join(preventiveActionTaskDomain.GetAll(),
                            y => new { y.Children.Id, y.Children.TypeDocumentGji },
                            z => new { z.Id, TypeDocumentGji = TypeDocumentGji.PreventiveActionTask },
                            (y, z) => z)
                        .DefaultIfEmpty(),
                        (x, y) => new { Action = x, Task = y })
                    .Select(x => new
                    {
                        x.Action.Id,
                        x.Action.State,
                        x.Action.TypeDocumentGji,
                        x.Action.Inspection.TypeBase,
                        InspectionId = x.Action.Inspection.Id,
                        Municipality = x.Action.Municipality.Name,
                        ControlType = x.Action.ControlType.Name,
                        x.Action.ActionType,
                        x.Action.VisitType,
                        ControlledOrganization = x.Action.ControlledOrganization.Name,
                        x.Action.DocumentNumber,
                        x.Action.DocumentDate,
                        x.Task.ActionStartDate,
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.TypeDocumentGji,
                        x.TypeBase,
                        x.InspectionId,
                        x.Municipality,
                        x.ControlType,
                        x.ActionType,
                        x.VisitType,
                        x.ControlledOrganization,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.ActionStartDate,
                        Inspectors = inspectorsDict.Get(x.Id)
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container, true, !isExport);
            }
        }
    }
}