namespace Bars.Gkh.Regions.Tatarstan.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="DuOuUslugaProxy"/>
    /// </summary>
    public class DuOuUslugaSelectorService : BaseProxySelectorService<DuOuUslugaProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DuOuUslugaProxy> GetCache()
        {
            var cacheIds = this.ProxySelectorFactory.GetSelector<DuProxy>().ProxyListIdCache;

            var manOrgContractService = this.Container.ResolveRepository<ManOrgContractService>();

            using (this.Container.Using(manOrgContractService))
            {
                return manOrgContractService.GetAll()
                    .WhereContainsBulked(x => x.Contract.Id, cacheIds, 5000)
                    .Where(x => x.ServiceType == ManagementContractServiceType.Communal || x.ServiceType == ManagementContractServiceType.Additional)
                    .Select(x => new
                    {
                        x.Id,
                        ContractId = (long?)x.Contract.Id,
                        x.StartDate,
                        x.EndDate,
                        ServiceId = (long?)x.Service.ExportId
                    })
                    .AsEnumerable()
                    .Select(x => new DuOuUslugaProxy
                    {
                        Id = x.Id,
                        OuId = x.ContractId,
                        ServiceId = x.ServiceId,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        IsServiceByThisContract = 1 // всегда передавать 1
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}