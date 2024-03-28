namespace Bars.GkhGji.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Селектор Предписание проверки
    /// </summary>
    public class PreceptAuditSelectorService : BaseProxySelectorService<PreceptAuditProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, PreceptAuditProxy> GetCache()
        {
            var violationRepos = this.Container.ResolveRepository<PrescriptionViol>();
            var documentGjiChildrenRepos = this.Container.ResolveRepository<DocumentGjiChildren>();

            using (this.Container.Using(violationRepos, documentGjiChildrenRepos))
            {
                var inspectionQuery = this.FilterService.GetFiltredQuery<ViewFormatDataExportInspection>();

                var violationDateDict = violationRepos.GetAll()
                    .Where(x => inspectionQuery.Any(y => x.Document == y.ActCheck))
                    .Select(x => new
                    {
                        x.Document.Id,
                        x.DatePlanRemoval,
                        x.DateFactRemoval
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => new { x.DateFactRemoval, x.DatePlanRemoval })
                    .ToDictionary(x => x.Key, x => x.ToList());

                return documentGjiChildrenRepos.GetAll()
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .Where(x => inspectionQuery.Any(y => x.Parent == y.ActCheck))
                    .Select(x => new
                    {
                        x.Children.Id,
                        ActCheckId = x.Parent.Id,
                        InspectionId = x.Parent.Inspection.Id,
                        x.Children.DocumentNumber,
                        x.Children.DocumentDate
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var dateFactRemoval = violationDateDict.Get(x.ActCheckId);

                        return new PreceptAuditProxy
                        {
                            Id = x.Id,
                            AuditId = x.InspectionId,
                            State = 1, // Действует
                            DocumentNumber = x.DocumentNumber,
                            DocumentDate = x.DocumentDate,
                            PlanExecutionDate = dateFactRemoval?.Select(z => z.DatePlanRemoval).Where(y => y.HasValue).SafeMax(y => y),
                            Info = null, // 7.  Краткая информация
                            ExecutionState = (dateFactRemoval?.Select(z => z.DateFactRemoval).All(y => y.HasValue) ?? false) ? 1 : 2,
                            ExecutionDate = dateFactRemoval?.Select(z => z.DateFactRemoval).Where(y => y.HasValue).SafeMax(y => y)
                        };
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}