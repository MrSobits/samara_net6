namespace Bars.Gkh.Regions.Tatarstan.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="UstavServiceProxy"/>
    /// </summary>
    public class UstavServiceSelectorService : BaseProxySelectorService<UstavServiceProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, UstavServiceProxy> GetCache()
        {
            var ustavIds = this.ProxySelectorFactory.GetSelector<UstavProxy>().ProxyListIdCache;

            var manOrgContractService = this.Container.ResolveRepository<ManOrgAgrContractService>();

            using (this.Container.Using(manOrgContractService))
            {
                return manOrgContractService.GetAll()
                    .WhereContainsBulked(x => x.Contract.Id, ustavIds, 5000)
                    .Select(x => new
                    {
                        x.Id,
                        ContractId = x.Contract.Id,
                        ServiceId = (long?)x.Service.ExportId,
                        x.PaymentAmount
                    })
                    .AsEnumerable()
                    .Select(x => new UstavServiceProxy
                    {
                        Id = x.Id,
                        DuId = x.ContractId,
                        ServiceId = x.ServiceId,
                        PaymentAmount = x.PaymentAmount
                    })
                    .ToDictionary(x => x.Id);
            }

        }
    }
}