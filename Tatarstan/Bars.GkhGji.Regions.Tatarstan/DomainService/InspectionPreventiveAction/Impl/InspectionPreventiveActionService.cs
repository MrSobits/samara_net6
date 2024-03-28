namespace Bars.GkhGji.Regions.Tatarstan.DomainService.InspectionPreventiveAction.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для <see cref="InspectionPreventiveAction"/>
    /// </summary>
    public class InspectionPreventiveActionService : IInspectionPreventiveActionService
    {
        private readonly IWindsorContainer container;

        public InspectionPreventiveActionService(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public IDataResult ListPreventiveAction(BaseParams baseParams)
        {
            var documentGjiDomain = this.container.ResolveDomain<DocumentGji>();
            var inspectionGjiDomain = this.container.ResolveDomain<InspectionGji>();
            var motivatedPresentationActionIsolatedDomain = this.container.ResolveDomain<MotivatedPresentation>();
            var visitSheetDomain = this.container.ResolveDomain<VisitSheet>();
            var preventiveActionDomain = this.container.ResolveDomain<PreventiveAction>();

            using (this.container.Using(documentGjiDomain, inspectionGjiDomain,
                motivatedPresentationActionIsolatedDomain, visitSheetDomain, preventiveActionDomain))
            {
                return motivatedPresentationActionIsolatedDomain.GetAll()
                    .Join(inspectionGjiDomain.GetAll(),
                        x => x.Inspection.Id,
                        y => y.Id,
                        (x, y) => x)
                    .Join(documentGjiDomain.GetAll(),
                        x => new { x.Inspection.Id, TypeDocumentGji = TypeDocumentGji.VisitSheet },
                        y => new { y.Inspection.Id, y.TypeDocumentGji },
                        (x, y) => new { VisitSheet = y })
                    .Join(inspectionGjiDomain.GetAll(),
                        x => x.VisitSheet.Inspection.Id,
                        y => y.Id,
                        (x, y) => new { x.VisitSheet })
                    .Join(documentGjiDomain.GetAll(),
                        x => new { x.VisitSheet.Inspection.Id, TypeDocumentGji = TypeDocumentGji.PreventiveActionTask },
                        y => new { y.Inspection.Id, y.TypeDocumentGji },
                        (x, y) => new { x.VisitSheet })
                    .Join(inspectionGjiDomain.GetAll(),
                        x => x.VisitSheet.Inspection.Id,
                        y => y.Id,
                        (x, y) => new { x.VisitSheet })
                    .Join(documentGjiDomain.GetAll(),
                        x => new { x.VisitSheet.Inspection.Id, TypeDocumentGji = TypeDocumentGji.PreventiveAction },
                        y => new { y.Inspection.Id, y.TypeDocumentGji },
                        (x, y) => new { x.VisitSheet, PreventiveAction = y })
                    .Join(preventiveActionDomain.GetAll(),
                        x => x.PreventiveAction.Id,
                        y => y.Id,
                        (x, y) => new { x.VisitSheet, PreventiveAction = y })
                    .Where(x => x.PreventiveAction.ActionType == PreventiveActionType.Visit)
                    .Select(x => new
                    {
                        InspectionId = x.PreventiveAction.Inspection.Id,
                        PreventiveActionDocumentDate = x.PreventiveAction.DocumentDate,
                        PreventiveActionFormatted = $"{x.PreventiveAction.DocumentNumber} " +
                            $"{(x.PreventiveAction.DocumentDate.HasValue ? $"{x.PreventiveAction.DocumentDate.Value:dd.MM.yyyy}" : null)}",
                        VisitSheetNumber = x.VisitSheet.DocumentNumber,
                        VisitSheetDocumentDate = x.VisitSheet.DocumentDate,
                        Contragent = x.PreventiveAction.ControlledOrganization.Name,
                        TypeJurPerson = x.PreventiveAction.ControlledPersonType,
                        TypeObject = TypeDocObject.Legal
                    })
                    .ToListDataResult(baseParams.GetLoadParam());
            }
        }
    }
}