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
    /// Селектор предписаний проверок
    /// </summary>
    public class PreceptFilesSelectorService : BaseProxySelectorService<PreceptFilesProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, PreceptFilesProxy> GetCache()
        {
            var annexRepository = this.Container.ResolveRepository<PrescriptionAnnex>();
            using (this.Container.Using(annexRepository))
            {
                var prescriptionQuery = this.FilterService.GetFiltredQuery<Prescription>();

                return annexRepository.GetAll()
                    .WhereNotNull(x => x.File)
                    .Where(x => prescriptionQuery.Any(y => y == x.Prescription))
                    .Select(x => new PreceptFilesProxy
                    {
                        File = x.File,
                        DocumentGjiId = x.Prescription.Id,
                        Type = 1
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}