namespace Bars.GkhDi.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сервис получения <see cref="WorkProxy"/>
    /// </summary>
    public class WorkSelectorService : BaseProxySelectorService<WorkProxy>
    {
        /// <summary>
        /// Статьи затрат
        /// </summary>
        public IRepository<CostItem> CostItemRepository { get; set; }

        /// <inheritdoc />
        protected override IDictionary<long, WorkProxy> GetCache()
        {
            return this.CostItemRepository.GetAll()
                .AsEnumerable()
                .Select(x => new WorkProxy
                {
                    Id = x.Id,
                    WorkUslugaProxyId = x.BaseService.Id,
                    Cost = x.Cost,
                    Volume = x.Count,
                    Count = this.GetIntFromDecimal(x.Count),
                    Sum = x.Sum
                })
                .ToDictionary(x => x.Id);
        }

        private int? GetIntFromDecimal(decimal? value)
        {
            if (!value.HasValue)
            {
                return null;
            }

            return decimal.ToInt32(value.Value);
        }
    }
}