namespace Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Hmao.Entities;
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
           var recordRepository = this.Container.ResolveRepository<PublishedProgramRecord>();

            using (this.Container.Using(recordRepository))
            {
                var programIds = this.AdditionalIds.Select(UniqueIdTool.GetHiDwordId).ToList();

                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                return this.GetProxies(objectCrQuery.WhereContainsBulked(x => x.GetId(), this.AdditionalIds),
                    recordRepository.GetAll().WhereContainsBulked(x => x.PublishedProgram.ProgramVersion.GetId(), programIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, PkrDomProxy> GetCache()
        {
            var recordRepository = this.Container.ResolveRepository<PublishedProgramRecord>();
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
                    .Where(x => programVersionQuery.Any(y => y == x.PublishedProgram.ProgramVersion 
                        && x.PublishedProgram.ProgramVersion.IsMain));
                
                return this.GetProxies(objectCrQuery, publishedProgramQuery)
                    .ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<PkrDomProxy> GetProxies(IQueryable<ObjectCr> objectCrQuery, IQueryable<PublishedProgramRecord> publishedProgramRecord)
        {
            var objectCrData = objectCrQuery
                .Select(x => new PkrDomProxy
                {
                    Id = x.Id,
                    HouseId = x.RealityObject.GetId(),
                    ProgramId = x.ProgramCr.GetId(),
                    Oktmo = x.RealityObject.Municipality.Oktmo,
                })
                .ToList();

            return publishedProgramRecord
                .Select(s => new
                {
                    Id = s.GetId(),
                    ProgramVersionId = 1,
                    HouseId = s.RealityObject.GetId(),
                    Oktmo = s.PublishedProgram.ProgramVersion.Municipality.Oktmo
                })
                .AsEnumerable()
                .Select(s => new PkrDomProxy
                {
                    Id = UniqueIdTool.GetId(s.ProgramVersionId, s.HouseId),
                    HouseId = s.HouseId,
                    ProgramId = s.ProgramVersionId,
                    Oktmo = s.Oktmo
                })
                .GroupBy(x => x.Id)
                .Select(x => x.First())
                .Union(objectCrData)
                .ToList();
        }
    }
}