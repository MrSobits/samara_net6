namespace Bars.GkhGji.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Селектор распоряжений проверок
    /// </summary>
    public class AuditFilesSelectorService : BaseProxySelectorService<AuditFilesProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, AuditFilesProxy> GetCache()
        {
            var annexRepository = this.Container.ResolveRepository<DisposalAnnex>();

            using (this.Container.Using(annexRepository))
            {
                var inspectionQuery = this.FilterService.GetFiltredQuery<ViewFormatDataExportInspection>();

                return annexRepository.GetAll()
                    .WhereNotNull(x => x.File)
                    .Where(x => inspectionQuery.Any(y => y.Disposal == x.Disposal))
                    .Select(x => new AuditFilesProxy
                    {
                        File = x.File,
                        AuditId = x.Disposal.Inspection.Id,
                        Type = 1
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}