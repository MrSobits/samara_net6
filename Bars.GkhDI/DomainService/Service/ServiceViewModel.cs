namespace Bars.GkhDi.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.GkhDi.Enums;

    using Entities;

    public class ServiceViewModel : BaseViewModel<BaseService>
    {
        public override IDataResult List(IDomainService<BaseService> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");

            var kindTemplateService = baseParams.Params.GetAs<long>("kindTemplateService");
            var isTemplateService = baseParams.Params.ContainsKey("isTemplateService") && baseParams.Params["isTemplateService"].ToBool();

            var servicePercentsDict = Container.Resolve<IDomainService<ServicePercent>>().GetAll()
                .Where(x => x.Service.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId && x.Code == "ServicePercent")
                .ToDictionary(x => x.Service.Id, y => y.Percent != null ? y.Percent.ToDecimal().RoundDecimal(2) + " %" : " - ");

            var data = domainService.GetAll()
                .WhereIf(isTemplateService, x => x.TemplateService.KindServiceDi == kindTemplateService.To<KindServiceDi>())
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                .AsEnumerable()
                .Select(x => new
                                 {
                                     x.Id,
                                     x.TemplateService.TypeGroupServiceDi,
                                     x.TemplateService.KindServiceDi, // нужно для обработки записи в контроллере
                                     x.TemplateService.Name,
                                     ProviderName = x.Provider != null ? x.Provider.Name : string.Empty,
                                     Tariff = x.TariffForConsumers.HasValue ? x.TariffForConsumers.Value.ToString("N") : string.Empty,
                                     TariffIsSetFor = x.TariffIsSetForDi,
                                     DateStart = x.DateStartTariff.HasValue ? x.DateStartTariff.Value.ToShortDateString() : string.Empty,
                                     percent = servicePercentsDict.ContainsKey(x.Id) ? servicePercentsDict[x.Id] : " - "
                                 })
                .AsQueryable()
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}