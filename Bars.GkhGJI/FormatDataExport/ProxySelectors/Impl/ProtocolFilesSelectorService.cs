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
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Селектор протоколов проверок
    /// </summary>
    public class ProtocolFilesSelectorService : BaseProxySelectorService<ProtocolFilesProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, ProtocolFilesProxy> GetCache()
        {
            var annexRepository = this.Container.ResolveRepository<ProtocolAnnex>();
            var documentGjiChildrenRepos = this.Container.ResolveRepository<DocumentGjiChildren>();
            using (this.Container.Using(annexRepository, documentGjiChildrenRepos))
            {
                var inspectionQuery = this.FilterService.GetFiltredQuery<ViewFormatDataExportInspection>();

                var filterQuery = documentGjiChildrenRepos.GetAll()
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Where(x => inspectionQuery.Any(y => y.ActCheck == x.Parent))
                    .Select(x => x.Children);

                return annexRepository.GetAll()
                    .WhereNotNull(x => x.File)
                    .Where(x => filterQuery.Any(y => y == x.Protocol))
                    .Select(x => new ProtocolFilesProxy
                    {
                        File = x.File,
                        DocumentGjiId = x.Protocol.Id,
                        Type = 1
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}