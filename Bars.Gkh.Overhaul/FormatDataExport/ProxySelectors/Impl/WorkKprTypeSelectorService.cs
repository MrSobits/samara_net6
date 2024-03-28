namespace Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="WorkKprTypeProxy"/>
    /// </summary>
    public class WorkKprTypeSelectorService : BaseProxySelectorService<WorkKprTypeProxy>
    {
        /// <inheritdoc />
        protected override bool CanGetFullData()
        {
            return false;
        }

        /// <inheritdoc />
        protected override ICollection<WorkKprTypeProxy> GetAdditionalCache()
        {
            var workRepository = this.Container.ResolveRepository<Work>();
            var coObjectRepository = this.Container.ResolveRepository<CommonEstateObject>();

            using (this.Container.Using(workRepository, coObjectRepository))
            {
                return this.GetProxies(workRepository.GetAll().WhereContainsBulked(x => x.ExportId, this.AdditionalIds),
                    coObjectRepository.GetAll().WhereContainsBulked(x => x.ExportId, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, WorkKprTypeProxy> GetCache()
        {
            var workRepository = this.Container.ResolveRepository<Work>();
            var coObjectRepository = this.Container.ResolveRepository<CommonEstateObject>();

            using (this.Container.Using(workRepository, coObjectRepository))
            {
                return this.GetProxies(workRepository.GetAll(), coObjectRepository.GetAll()).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<WorkKprTypeProxy> GetProxies(IQueryable<Work> workQuery, IQueryable<CommonEstateObject> coObjectQuery)
        {
            var coObjects = coObjectQuery
                .Select(x => new WorkKprTypeProxy
                {
                    Id = x.ExportId,
                    GroupCode = x.GisCode,
                    Name = x.Name
                })
                .ToList();

            return workQuery
                .Select(x => new WorkKprTypeProxy
                {
                    Id = x.ExportId,
                    GroupCode = x.GisCode,
                    Name = x.Name
                })
                .AsEnumerable()
                .Union(coObjects)
                .ToList();
        }
    }
}