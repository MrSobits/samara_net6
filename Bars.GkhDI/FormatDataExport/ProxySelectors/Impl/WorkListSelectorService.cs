namespace Bars.GkhDi.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Сервис получения <see cref="WorkListProxy"/>
    /// </summary>
    public class WorkListSelectorService : BaseProxySelectorService<WorkListProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, WorkListProxy> GetCache()
        {
            var baseServiceRepository = this.Container.ResolveRepository<BaseService>();
            var communalServiceRepository = this.Container.ResolveRepository<CommunalService>();
            var additionalServiceRepository = this.Container.ResolveRepository<AdditionalService>();
            var costItemRepository = this.Container.ResolveRepository<CostItem>();

            try
            {
                var duDict = this.ProxySelectorFactory.GetSelector<DuProxy>().ProxyListCache.Values
                    .Select(x => new
                    {
                        x.Id,
                        x.RealityObjectId
                    })
                    .GroupBy(x => x.RealityObjectId, x => x.Id)
                    .ToDictionary(x => x.Key, x => x.First());

                var uslugaDict = this.ProxySelectorFactory.GetSelector<DictUslugaProxy>().ProxyListCache.Values
                    .Where(x => x.TemplateServiceId.HasValue)
                    .Select(
                        x => new
                        {
                            TemplateServiceId = x.TemplateServiceId.Value,
                            x.Id
                        })
                    .ToDictionary(x => x.TemplateServiceId, x => x.Id);

                var housingDict = costItemRepository.GetAll()
                    .WhereContainsBulked(x => x.BaseService.DisclosureInfoRealityObj.RealityObject.Id, duDict.Keys, 5000)
                    .Select(x => new
                    {
                        x.Id,
                        BaseServiceId = x.BaseService.Id,
                        x.Cost,
                        x.Sum,
                        x.Count
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.BaseServiceId)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Id).FirstOrDefault());

                var communalDict = communalServiceRepository.GetAll()
                    .WhereContainsBulked(x => x.DisclosureInfoRealityObj.RealityObject.Id, duDict.Keys, 5000)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.PricePurchasedResources,
                            x.VolumePurchasedResources
                        })
                    .ToDictionary(x => x.Id);

                var additionallDict = additionalServiceRepository.GetAll()
                    .WhereContainsBulked(x => x.DisclosureInfoRealityObj.RealityObject.Id, duDict.Keys, 5000)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Total
                        })
                    .ToDictionary(x => x.Id);

                return baseServiceRepository.GetAll()
                    .WhereContainsBulked(x => x.DisclosureInfoRealityObj.RealityObject.Id, duDict.Keys, 5000)
                    .Select(
                        x => new
                        {
                            x.Id,
                            RoId = x.DisclosureInfoRealityObj.RealityObject.Id,
                            x.DisclosureInfoRealityObj.PeriodDi.DateStart,
                            x.DisclosureInfoRealityObj.PeriodDi.DateEnd,
                            x.TemplateService.TypeGroupServiceDi
                        })
                    .AsEnumerable()
                    .Select(
                        x =>
                        {
                            var cummnalInfo = communalDict.Get(x.Id);
                            var housingInfo = housingDict.Get(x.Id);
                            var additionalInfo = additionallDict.Get(x.Id);

                            return new WorkListProxy
                            {
                                Id = x.Id,
                                RealityObjectId = x.RoId,
                                StartDate = x.DateStart,
                                EndDate = x.DateEnd,
                                ManOrgContractId = duDict.Get(x.RoId),
                                Status = 1,
                                Cost = x.TypeGroupServiceDi == TypeGroupServiceDi.Communal ? cummnalInfo?.PricePurchasedResources 
                                    : x.TypeGroupServiceDi == TypeGroupServiceDi.Housing ? housingInfo?.Cost : null,
                                Volume = x.TypeGroupServiceDi == TypeGroupServiceDi.Communal ? cummnalInfo?.VolumePurchasedResources
                                    : x.TypeGroupServiceDi == TypeGroupServiceDi.Housing ? housingInfo?.Count : null,
                                Summary = x.TypeGroupServiceDi == TypeGroupServiceDi.Housing ? housingInfo?.Sum 
                                    : x.TypeGroupServiceDi == TypeGroupServiceDi.Other ? additionalInfo?.Total : null,
                                DictUslugaId = uslugaDict.Get(x.Id)
                            };
                        })
                    .ToDictionary(x => x.Id);
            }
            finally 
            {
                this.Container.Release(baseServiceRepository);
                this.Container.Release(costItemRepository);
                this.Container.Release(communalServiceRepository);
                this.Container.Release(additionalServiceRepository);
            }
        }
    }
}