namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="EntranceProxy"/>
    /// </summary>
    public class EntranceSelectorService : BaseProxySelectorService<EntranceProxy>
    {
        /// <inheritdoc />
        protected override ICollection<EntranceProxy> GetAdditionalCache()
        {
            var repository = this.Container.ResolveRepository<Entrance>();
            using (this.Container.Using(repository))
            {
                return this.GetProxies(repository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        protected override IDictionary<long, EntranceProxy> GetCache()
        {
            var repository = this.Container.ResolveRepository<Entrance>();
            using (this.Container.Using(repository))
            {
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();

                var query = repository.GetAll()
                    .Where(x => roQuery.Any(r => r.Id == x.RealityObject.Id));

                return this.GetProxies(query)
                    .ToDictionary(x => x.Id);
            }
        }

        protected ICollection<EntranceProxy> GetProxies(IQueryable<Entrance> query)
        {
            return query
                .Select(x => new EntranceProxy
                {
                    Id = x.Id,
                    HouseId = x.RealityObject.Id,
                    Number = x.Number.ToString(),
                    IsSupplierConfirmed = 1
                })
                .ToList();
        }
    }
}