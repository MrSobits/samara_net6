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
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Селектор Протоколы проверки
    /// </summary>
    public class ProtocolAuditSelectorService : BaseProxySelectorService<ProtocolAuditProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, ProtocolAuditProxy> GetCache()
        {
            var protocolViolationRepos = this.Container.ResolveRepository<ProtocolViolation>();
            var documentGjiChildrenRepos = this.Container.ResolveRepository<DocumentGjiChildren>();

            using (this.Container.Using(protocolViolationRepos, documentGjiChildrenRepos))
            {
                var inspectionQuery = this.FilterService.GetFiltredQuery<ViewFormatDataExportInspection>();

                var violationDateDict = protocolViolationRepos.GetAll()
                    .Where(x => inspectionQuery.Any(y => y.Inspection == x.Document.Inspection))
                    .Select(x => new
                    {
                        x.Document.Id,
                        x.DateFactRemoval
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.DateFactRemoval)
                    .ToDictionary(x => x.Key, x => x.ToList());

                return documentGjiChildrenRepos.GetAll()
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Where(x => inspectionQuery.Any(y => y.ActCheck == x.Parent))
                    .Select(x => new
                    {
                        x.Children.Id,
                        InspectionId = x.Parent.Inspection.Id,
                        x.Children.DocumentNumber,
                        x.Children.DocumentDate
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var dateFactRemoval = violationDateDict.Get(x.Id);

                        return new ProtocolAuditProxy
                        {
                            Id = x.Id,
                            AuditId = x.InspectionId,
                            State = 1, // Действует
                            DocumentNumber = x.DocumentNumber,
                            DocumentDate = x.DocumentDate,
                            Info = null, // 6.  Краткая информация
                            ExecutionState = (dateFactRemoval?.All(y => y.HasValue) ?? false) ? 1 : 2,
                            ExecutionDate = dateFactRemoval?.Where(y => y.HasValue).SafeMax(y => y)
                        };
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}