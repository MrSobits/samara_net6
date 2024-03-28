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
    /// Сервис получения <see cref="DictMeasureProxy"/>
    /// </summary>
    public class DictMeasureSelectorService : BaseProxySelectorService<DictMeasureProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DictMeasureProxy> GetCache()
        {
            return this.GetProxies(false)
                .ToDictionary(x => x.Id);
        }

        protected override ICollection<DictMeasureProxy> GetAdditionalCache()
        {
            return this.GetProxies(true);
        }

        private ICollection<DictMeasureProxy> GetProxies(bool isFiltred)
        {
            var unitMeasureRepository = this.Container.Resolve<IRepository<UnitMeasure>>();

            using (this.Container.Using(unitMeasureRepository))
            {
                return unitMeasureRepository.GetAll()
                    .WhereIfContainsBulked(isFiltred, x => x.Id, this.AdditionalIds)
                    .Select(x => new DictMeasureProxy
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        OkeiCode = x.OkeiCode
                    })
                    .ToList();
            }
        }
    }
}