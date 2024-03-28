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
    /// Селектор <see cref="AuditPlaceProxy"/>
    /// </summary>
    public class AuditPlaceSelectorService : BaseProxySelectorService<AuditPlaceProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, AuditPlaceProxy> GetCache()
        {
            var actCheckRoRepos = this.Container.ResolveRepository<ActCheckRealityObject>();
            using (this.Container.Using(actCheckRoRepos))
            {
                var inspectionQuery = this.FilterService.GetFiltredQuery<ViewFormatDataExportInspection>();

                return actCheckRoRepos.GetAll()
                    .WhereNotNull(x => x.RealityObject)
                    .Where(x => inspectionQuery.Any(y => x.ActCheck == y.ActCheck))
                    .Select(x => new
                    {
                        x.Id,
                        InspectionId = x.ActCheck.Inspection.Id,
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .Select(x => new AuditPlaceProxy
                    {
                        Id = x.Id,
                        AuditId = x.InspectionId,
                        HouseId = x.RoId
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}