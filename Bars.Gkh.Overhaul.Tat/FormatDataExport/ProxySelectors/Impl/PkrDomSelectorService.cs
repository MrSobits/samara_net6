namespace Bars.Gkh.Overhaul.Tat.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Селектор Дома ПКР (pkrdom.csv)
    /// </summary>
    public class PkrDomSelectorService : BaseProxySelectorService<PkrDomProxy>
    {
        /// <inheritdoc />
        protected override ICollection<PkrDomProxy> GetAdditionalCache()
        {
            var objectCrRepository = this.Container.ResolveRepository<ObjectCr>();
            var recordRepository = this.Container.ResolveRepository<VersionRecord>();

            using (this.Container.Using(objectCrRepository, recordRepository))
            {
                var programIds = this.AdditionalIds.Select(UniqueIdTool.GetHiDwordId).ToList();

                return this.GetProxies(objectCrRepository.GetAll().WhereContainsBulked(x => x.GetId(), this.AdditionalIds),
                    recordRepository.GetAll().WhereContainsBulked(x => x.ProgramVersion.GetId(), programIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, PkrDomProxy> GetCache()
        {
            var recordRepository = this.Container.ResolveRepository<VersionRecord>();
            var programVersionRepository = this.Container.ResolveRepository<ProgramVersion>();

            using (this.Container.Using(recordRepository, programVersionRepository))
            {
                var municipalityIds = this.FilterService.ProgramVersionFilter.Filter.GetAs<long[]>("municipalityId");
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                var programVersionQuery = this.FilterService.ProgramVersionIds.Any()
                    ? programVersionRepository.GetAll()
                        .WhereContainsBulked(x => x.Id, this.FilterService.ProgramVersionIds)
                    : programVersionRepository.GetAll()
                        .WhereIfContainsBulked(municipalityIds.Any(), x => x.Municipality.Id, municipalityIds)
                        .Filter(this.FilterService.ProgramVersionFilter, this.Container);

                var publishedProgramQuery = recordRepository.GetAll()
                    .Where(x => programVersionQuery.Any(y => y == x.ProgramVersion));

                return this.GetProxies(objectCrQuery, publishedProgramQuery)
                    .ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<PkrDomProxy> GetProxies(IQueryable<ObjectCr> objectCrQuery, IQueryable<VersionRecord> recordQuery)
        {
            var objectCrData = objectCrQuery
                .Select(x => new PkrDomProxy
                {
                    Id = x.Id,
                    HouseId = x.RealityObject.GetId(),
                    ProgramId = x.ProgramCr.GetId(),
                    Oktmo = x.RealityObject.Municipality.Oktmo
                })
                .ToList();

            return recordQuery
                .Where(x => x.ProgramVersion.IsMain)
                .Select(x => new
                {
                    HouseId = x.RealityObject.GetId(),
                    ProgramId = x.ProgramVersion.GetId(),
                    x.ProgramVersion.Municipality.Oktmo
                })
                .AsEnumerable()
                .Select(x =>new PkrDomProxy
                {
                    Id = UniqueIdTool.GetId(x.ProgramId, x.HouseId),
                    HouseId = x.HouseId,
                    ProgramId = x.ProgramId,
                    Oktmo = x.Oktmo
                })
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .Union(objectCrData)
                .ToList();
        }
    }
}