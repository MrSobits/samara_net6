namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Сервис получения <see cref="UstavOuUslugaProxy"/>
    /// </summary>
    public class UstavOuUslugaSelectorService : BaseProxySelectorService<UstavOuUslugaProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, UstavOuUslugaProxy> GetCache()
        {
            return this.ProxySelectorFactory.GetSelector<UstavProxy>()
                .ExtProxyListCache
                .Select(x => new UstavOuUslugaProxy
                {
                    Id = x.Id,
                    OuId = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.TerminationDate,
                    IsServiceByThisContract = 1,
                })
                .ToDictionary(x => x.Id);
        }
    }
}