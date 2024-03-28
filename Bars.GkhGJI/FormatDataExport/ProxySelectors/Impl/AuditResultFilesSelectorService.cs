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
    /// Селектор актов проверки
    /// </summary>
    public class AuditResultFilesSelectorService : BaseProxySelectorService<AuditResultFilesProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, AuditResultFilesProxy> GetCache()
        {
            var annexRepository = this.Container.ResolveRepository<ActCheckAnnex>();

            using (this.Container.Using(annexRepository))
            {
                var inspectionQuery = this.FilterService.GetFiltredQuery<ViewFormatDataExportInspection>();

                return annexRepository.GetAll()
                    .WhereNotNull(x => x.File)
                    .Where(x => inspectionQuery.Any(y => y.ActCheck == x.ActCheck))
                    .Select(x => new AuditResultFilesProxy
                    {
                        File = x.File,
                        AuditId = x.ActCheck.Inspection.Id,
                        Type = 1
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}