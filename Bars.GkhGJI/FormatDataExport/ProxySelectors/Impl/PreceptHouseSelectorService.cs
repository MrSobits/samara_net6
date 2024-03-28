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
    /// Селектор Дома в предписании
    /// </summary>
    public class PreceptHouseSelectorService : BaseProxySelectorService<PreceptHouseProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, PreceptHouseProxy> GetCache()
        {
            var violationRepos = this.Container.ResolveRepository<PrescriptionViol>();
            using (this.Container.Using(violationRepos))
            {
                var prescriptionQuery = this.FilterService.GetFiltredQuery<Prescription>();

                return violationRepos.GetAll()
                    .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.Prescription)
                    .WhereNotNull(x => x.InspectionViolation.RealityObject)
                    .Where(x => prescriptionQuery.Any(y => y == x.Document))
                    .Select(x => new PreceptHouseProxy
                    {
                        PreceptId = x.Document.Id,
                        DomId = x.InspectionViolation.RealityObject.Id
                    })
                    .AsEnumerable()
                    .DistinctBy(x => x.Id)
                    .ToDictionary(x => x.Id);
            }
        }
    }
}