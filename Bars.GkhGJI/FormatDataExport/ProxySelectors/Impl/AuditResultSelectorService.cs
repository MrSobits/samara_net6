namespace Bars.GkhGji.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Селектор результатов проверок
    /// </summary>
    public class AuditResultSelectorService : BaseProxySelectorService<AuditResultProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, AuditResultProxy> GetCache()
        {
            var actCheckRealityObjectRepository = this.Container.ResolveRepository<ActCheckRealityObject>();
            var actCheckViolationRepository = this.Container.ResolveRepository<ActCheckViolation>();
            var documentGjiInspectorRepository = this.Container.ResolveRepository<DocumentGjiInspector>();
            var actCheckWitnessRepository = this.Container.ResolveRepository<ActCheckWitness>();
            var actCheckPeriodRepository = this.Container.ResolveRepository<ActCheckPeriod>();

            using (this.Container.Using(actCheckRealityObjectRepository,
                actCheckViolationRepository,
                documentGjiInspectorRepository,
                actCheckWitnessRepository,
                actCheckPeriodRepository
                ))
            {
                var inspectionQuery = this.FilterService.GetFiltredQuery<ViewFormatDataExportInspection>();

                var violationDict = actCheckViolationRepository.GetAll()
                    .WhereNotNull(x => x.ActObject)
                    .Where(x => inspectionQuery.Any(y => x.InspectionViolation.Inspection == y.Inspection))
                    .Select(x => new
                    {
                        x.InspectionViolation.Inspection.Id,
                        x.InspectionViolation.Violation.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.Name)
                    .ToDictionary(x => x.Key, x => x.AggregateWithSeparator(";"));

                var inspectorDict = documentGjiInspectorRepository.GetAll()
                    .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => inspectionQuery.Any(y => x.DocumentGji == y.ActCheck))
                    .Select(x => new
                    {
                        x.DocumentGji.Id,
                        x.Inspector.Fio,
                        x.Inspector.Code
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => $"{x.Fio}, {x.Code}")
                    .ToDictionary(x => x.Key, x => x.Distinct().AggregateWithSeparator(";"));

                var respondentDict = actCheckWitnessRepository.GetAll()
                    .Where(x => inspectionQuery.Any(y => x.ActCheck == y.ActCheck))
                    .Select(x => new
                    {
                        x.ActCheck.Id,
                        x.Fio,
                        x.Position,
                        x.IsFamiliar
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        x => x.GroupBy(y => y.IsFamiliar)
                            .ToDictionary(y => y.Key,
                                y => y.AggregateWithSeparator(z => $"{z.Fio}, {z.Position}", ";")));

                return actCheckRealityObjectRepository.GetAll()
                    .Where(x => inspectionQuery.Any(y => x.ActCheck == y.ActCheck))
                    .Select(x => new
                    {
                        x.Id,
                        ActCheckId = x.ActCheck.Id,
                        InspectionId = x.ActCheck.Inspection.Id,
                        x.HaveViolation,
                        x.Description,
                        x.RealityObject.Address,
                        x.ActCheck.AcquaintState,
                        x.ActCheck.DocumentPlaceFias.AddressName,
                        x.ActCheck.AcquaintedDate,
                        x.ActCheck.Inspection.InspectionNumber,
                        x.ActCheck.Inspection.CheckDate,
                        x.ActCheck.DocumentDate
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var inspectors = inspectorDict.Get(x.ActCheckId);
                        var responents = respondentDict.Get(x.ActCheckId);
                        return new AuditResultProxy
                        {
                            Id = x.Id,
                            AuditId = x.InspectionId,
                            State = 1,
                            DocumentKind = 1,
                            DocumentNumber = x.InspectionNumber,
                            DocumentDate = x.CheckDate,
                            Result = x.HaveViolation == YesNoNotSet.Yes ? 1 : 2,
                            Violations = x.Description,
                            ActViolations = violationDict.Get(x.InspectionId),
                            StartDate = null,
                            EndDate = null,
                            DayCount = null,
                            HourCount = null,
                            PlaceAddress = x.Address,
                            Inspectors = inspectors,
                            Respondents = responents?.Values.AggregateWithSeparator("; "),
                            CreateDocPlaceAddress = x.AddressName,
                            ReviewState = x.AcquaintState.HasValue ? (int) x.AcquaintState.Value / 10 : (int?) null,
                            RefusedRespondents = responents?.Get(false),
                            ReiviewDate = x.AcquaintedDate,
                            ConcentRespondents = responents?.Get(true)
                        };
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}