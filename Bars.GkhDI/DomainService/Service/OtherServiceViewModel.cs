namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities.Service;

    using Entities;

    public class OtherServiceViewModel : BaseViewModel<OtherService>
    {
        public override IDataResult Get(IDomainService<OtherService> domainService, BaseParams baseParams)
        {
            //метод переопределен для корректного отображения поставщика в окне редактирования прочих услуг
            var otherService = domainService.Get(baseParams.Params.GetAsId());

            if (otherService != null)
            {
                var providerOtherServiceDomain = this.Container.ResolveDomain<ProviderOtherService>();
                using (this.Container.Using(providerOtherServiceDomain))
                {
                    var hashSet = new HashSet<long> { otherService.Id };
                    var providersHash = OtherServiceViewModel.GetProvidersDictionary(providerOtherServiceDomain, hashSet);
                    otherService.Provider = providersHash.ContainsKey(otherService.Id) ? providersHash[otherService.Id] : otherService.Provider;
                }
            }

            return new BaseDataResult(otherService);
        }

        public override IDataResult List(IDomainService<OtherService> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");

            var providerOtherServiceDomain = this.Container.ResolveDomain<ProviderOtherService>();
            var tariffOtherServiceDomain = this.Container.ResolveDomain<TariffForConsumersOtherService>();
            using (this.Container.Using(providerOtherServiceDomain, tariffOtherServiceDomain))
            {
                
                var data = domainService.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId && x.TemplateOtherService != null)
                    .Select(x => new
                    {
                        x.Id,
                        x.TemplateOtherService.Name,
                        x.Code,
                        x.UnitMeasure,
                        UnitMeasureName = x.UnitMeasure.Name
                    })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                data = data.Order(loadParams).Paging(loadParams);

                var dataList = data.ToList();
                var otherServiceIdHash = dataList.Select(x => x.Id).ToHashSet();

                var tariffsHash = OtherServiceViewModel.GetTariffsDictionary(tariffOtherServiceDomain, otherServiceIdHash);
                var providersHash = OtherServiceViewModel.GetProvidersDictionary(providerOtherServiceDomain, otherServiceIdHash);

                var result = dataList
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Code,
                        x.UnitMeasure,
                        x.UnitMeasureName,
                        Tariff = tariffsHash.ContainsKey(x.Id) ? tariffsHash[x.Id] : null,
                        Provider = providersHash.ContainsKey(x.Id) ? providersHash[x.Id] : null
                    }).ToList();

                return new ListDataResult(result, totalCount);
            }
        }

        /// <summary>
        /// Возвращает словарь тарифов.
        /// </summary>
        private static Dictionary<long, decimal?> GetTariffsDictionary(IDomainService<TariffForConsumersOtherService> tariffOtherServiceDomain, HashSet<long> otherServiceIdHash)=>
            tariffOtherServiceDomain.GetAll()
                .Where(x => otherServiceIdHash.Contains(x.OtherService.Id))
                .Select(x => new
                {
                    x.Id,
                    OtherServiceId = x.OtherService.Id,
                    x.DateStart,
                    x.Cost
                })
                .AsEnumerable()
                .GroupBy(x => x.OtherServiceId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.DateStart).ThenByDescending(y => y.Id).First().Cost);

        /// <summary>
        /// Возвращает словарь поставщиков.
        /// </summary>
        private static Dictionary<long, string> GetProvidersDictionary(IDomainService<ProviderOtherService> providerOtherServiceDomain, HashSet<long> otherServiceIdHash) =>
            providerOtherServiceDomain.GetAll().Where(x => otherServiceIdHash.Contains(x.OtherService.Id))
                .Select(x => new
                {
                    x.OtherService,
                    x.DateStartContract,
                    x.Provider,
                    x.ProviderName
                }).AsEnumerable()
                .GroupBy(x => x.OtherService.Id).ToDictionary(x => x.Key,
                    x => x.OrderByDescending(y => y.DateStartContract).First().Provider?.Name ?? x.OrderByDescending(y => y.DateStartContract).First().ProviderName);
    }
}