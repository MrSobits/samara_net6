namespace Bars.Gkh.Regions.Tatarstan.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="DuServiceProxy"/>
    /// </summary>
    public class DuServiceSelectorService : BaseProxySelectorService<DuServiceProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DuServiceProxy> GetCache()
        {
            var duIds = this.ProxySelectorFactory.GetSelector<DuProxy>().ProxyListIdCache;

            var serviceRepository = this.Container.ResolveRepository<ManOrgAgrContractService>();

            using (this.Container.Using(serviceRepository))
            {
                return serviceRepository.GetAll()
                    .WhereContainsBulked(x => x.Contract.Id, duIds, 5000)
                    .Select(x => new
                    {
                        x.Id,
                        ContractId = (long?)x.Contract.Id,
                        ServiceId = (long?)x.Service.ExportId,
                        x.PaymentAmount
                    })
                    .AsEnumerable()
                    .Select(x => new DuServiceProxy
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