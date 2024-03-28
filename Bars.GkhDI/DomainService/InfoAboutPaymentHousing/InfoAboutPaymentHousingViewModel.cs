namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.GkhDi.Enums;

    using Entities;

    public class InfoAboutPaymentHousingViewModel : BaseViewModel<InfoAboutPaymentHousing>
    {
        public override IDataResult List(IDomainService<InfoAboutPaymentHousing> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");

            var serviceBase = this.Container.Resolve<IDomainService<BaseService>>();
            var dataHousing = serviceBase
                .GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId && x.TemplateService.TypeGroupServiceDi == TypeGroupServiceDi.Housing)
                .Select(x => new
                {
                    x.Id,
                    TemplateServiceName = x.TemplateService.Name,
                    ProviderName = x.Provider.Name
                })
                .ToList();

            var dataInfoAboutPaymentHousing = domainService.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId && x.BaseService.TemplateService.TypeGroupServiceDi == TypeGroupServiceDi.Housing)
                .Select(x => new
                {
                    x.Id,
                    BaseServiseId = x.BaseService.Id,
                    x.CounterValuePeriodStart,
                    x.CounterValuePeriodEnd,
                    x.GeneralAccrual,
                    x.Collection
                })
                .Filter(loadParams, this.Container)
                .ToList();

            var dataNewInfoAboutPaymentHousing = new List<InfoAboutPaymentHousing>();

            foreach (var dataCommunalItem in dataHousing)
            {
                var infoAboutPaymentHousing = dataInfoAboutPaymentHousing.FirstOrDefault(x => x.BaseServiseId == dataCommunalItem.Id);
                if (infoAboutPaymentHousing != null)
                {
                    var newInfoAboutPaymentCommunal = new InfoAboutPaymentHousing
                    {
                        Id = dataCommunalItem.Id,
                        BaseService = serviceBase.Load(dataCommunalItem.Id),
                        CounterValuePeriodStart = infoAboutPaymentHousing.CounterValuePeriodStart,
                        CounterValuePeriodEnd = infoAboutPaymentHousing.CounterValuePeriodEnd,
                        GeneralAccrual = infoAboutPaymentHousing.GeneralAccrual,
                        Collection = infoAboutPaymentHousing.Collection
                    };
                    dataNewInfoAboutPaymentHousing.Add(newInfoAboutPaymentCommunal);
                }
                else
                {
                    var newInfoAboutPaymentCommunal = new InfoAboutPaymentHousing
                    {
                        Id = dataCommunalItem.Id,
                        BaseService = serviceBase.Load(dataCommunalItem.Id),
                        CounterValuePeriodStart = null,
                        CounterValuePeriodEnd = null,
                        GeneralAccrual = null,
                        Collection = null
                    };
                    dataNewInfoAboutPaymentHousing.Add(newInfoAboutPaymentCommunal);
                }
            }

            var data = dataNewInfoAboutPaymentHousing
                .Select(x => new
                {
                    x.Id,
                    BaseServiceName = x.BaseService != null && x.BaseService.TemplateService != null ? x.BaseService.TemplateService.Name : string.Empty,
                    ProviderName = x.BaseService != null && x.BaseService.Provider != null ? x.BaseService.Provider.Name : string.Empty,
                    x.CounterValuePeriodStart,
                    x.CounterValuePeriodEnd,
                    x.GeneralAccrual,
                    x.Collection
                })
                .AsQueryable();

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}