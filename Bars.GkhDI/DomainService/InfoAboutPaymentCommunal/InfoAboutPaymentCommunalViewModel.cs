namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using Bars.GkhDi.Enums;
    using Entities;

    public class InfoAboutPaymentCommunalViewModel : BaseViewModel<InfoAboutPaymentCommunal>
    {
        public override IDataResult List(IDomainService<InfoAboutPaymentCommunal> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");

            var serviceBase = this.Container.Resolve<IDomainService<BaseService>>();

            var dataCommunal = serviceBase
                .GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId && x.TemplateService.TypeGroupServiceDi == TypeGroupServiceDi.Communal)
                .Select(x => new
                {
                    x.Id,
                    TemplateServiceName = x.TemplateService.Name,
                    ProviderName = x.Provider.Name
                })
                .ToList();

            var dataInfoAboutPaymentCommunal = domainService.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId && x.BaseService.TemplateService.TypeGroupServiceDi == TypeGroupServiceDi.Communal)
                .Select(x => new
                {
                    x.Id,
                    BaseServiseId = x.BaseService.Id,
                    x.CounterValuePeriodStart,
                    x.CounterValuePeriodEnd,
                    x.Accrual,
                    x.Payed,
                    x.Debt,
                    x.TotalConsumption,
                    x.AccrualByProvider,
                    x.PayedToProvider,
                    x.DebtToProvider,
                    x.ReceivedPenaltySum
                })
                .Filter(loadParams, this.Container).ToList();

            var dataNewInfoAboutPaymentCommunal = new List<InfoAboutPaymentCommunal>();

            foreach (var dataCommunalItem in dataCommunal)
            {
                var infoAboutPaymentCommunal = dataInfoAboutPaymentCommunal.FirstOrDefault(x => x.BaseServiseId == dataCommunalItem.Id);
                if (infoAboutPaymentCommunal != null)
                {
                    var newInfoAboutPaymentCommunal = new InfoAboutPaymentCommunal
                    {
                        Id = dataCommunalItem.Id,
                        BaseService = serviceBase.Load(dataCommunalItem.Id),
                        CounterValuePeriodStart = infoAboutPaymentCommunal.CounterValuePeriodStart,
                        CounterValuePeriodEnd = infoAboutPaymentCommunal.CounterValuePeriodEnd,
                        Accrual = infoAboutPaymentCommunal.Accrual,
                        Payed = infoAboutPaymentCommunal.Payed,
                        Debt = infoAboutPaymentCommunal.Debt,
                        TotalConsumption = infoAboutPaymentCommunal.TotalConsumption,
                        AccrualByProvider = infoAboutPaymentCommunal.AccrualByProvider,
                        PayedToProvider = infoAboutPaymentCommunal.PayedToProvider,
                        DebtToProvider = infoAboutPaymentCommunal.DebtToProvider,
                        ReceivedPenaltySum = infoAboutPaymentCommunal.ReceivedPenaltySum
                    };
                    dataNewInfoAboutPaymentCommunal.Add(newInfoAboutPaymentCommunal);
                }
                else
                {
                    var newInfoAboutPaymentCommunal = new InfoAboutPaymentCommunal
                    {
                        Id = dataCommunalItem.Id,
                        BaseService = serviceBase.Load(dataCommunalItem.Id),
                        CounterValuePeriodStart = null,
                        CounterValuePeriodEnd = null,
                        Accrual = null,
                        Payed = null,
                        Debt = null,
                        TotalConsumption = null,
                        AccrualByProvider = null,
                        PayedToProvider = null,
                        DebtToProvider = null,
                        ReceivedPenaltySum = null
                    };
                    dataNewInfoAboutPaymentCommunal.Add(newInfoAboutPaymentCommunal);
                }
            }

            var data = dataNewInfoAboutPaymentCommunal
                .Select(x => new
                {
                    x.Id,
                    BaseServiceName = x.BaseService != null && x.BaseService.TemplateService != null ? x.BaseService.TemplateService.Name : string.Empty,
                    ProviderName = x.BaseService != null && x.BaseService.Provider != null ? x.BaseService.Provider.Name : string.Empty,
                    x.CounterValuePeriodStart,
                    x.CounterValuePeriodEnd,
                    x.Accrual,
                    x.Payed,
                    x.Debt,
                    x.TotalConsumption,
                    x.AccrualByProvider,
                    x.PayedToProvider,
                    x.DebtToProvider,
                    x.ReceivedPenaltySum
                })
                .AsQueryable();

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<InfoAboutPaymentCommunal> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var infoAboutPaymentCommunalData = domainService.GetAll().FirstOrDefault(x => x.BaseService.Id == id);

            var serviceBase = this.Container.Resolve<IDomainService<BaseService>>();
            var baseService = serviceBase.Get(id);

            if (infoAboutPaymentCommunalData != null)
            {
                return new BaseDataResult(
                    new 
                    {
                        Id = id,
                        BaseServiceName = baseService != null && baseService.TemplateService != null ? baseService.TemplateService.Name : string.Empty,
                        ProviderName = baseService != null && baseService.Provider != null ? baseService.Provider.Name : string.Empty,
                        CounterValuePeriodStart = infoAboutPaymentCommunalData.CounterValuePeriodStart,
                        CounterValuePeriodEnd = infoAboutPaymentCommunalData.CounterValuePeriodEnd,
                        Accrual = infoAboutPaymentCommunalData.Accrual,
                        Payed = infoAboutPaymentCommunalData.Payed,
                        Debt = infoAboutPaymentCommunalData.Debt,
                        TotalConsumption = infoAboutPaymentCommunalData.TotalConsumption,
                        AccrualByProvider = infoAboutPaymentCommunalData.AccrualByProvider,
                        PayedToProvider = infoAboutPaymentCommunalData.PayedToProvider,
                        DebtToProvider = infoAboutPaymentCommunalData.DebtToProvider,
                        ReceivedPenaltySum = infoAboutPaymentCommunalData.ReceivedPenaltySum
                    });
            }
            else
            {
                infoAboutPaymentCommunalData = new InfoAboutPaymentCommunal
                {
                    Id = id,
                    BaseService = baseService,
                    CounterValuePeriodStart = null,
                    CounterValuePeriodEnd = null,
                    Accrual = null,
                    Payed = null,
                    Debt = null,
                    TotalConsumption = null,
                    AccrualByProvider = null,
                    PayedToProvider = null,
                    DebtToProvider = null,
                    ReceivedPenaltySum = null
                };

                return new BaseDataResult(
                    new 
                    {
                        Id = id,
                        BaseServiceName = baseService != null && baseService.TemplateService != null ? baseService.TemplateService.Name : string.Empty,
                        ProviderName = baseService != null && baseService.Provider != null ? baseService.Provider.Name : string.Empty,
                        infoAboutPaymentCommunalData.CounterValuePeriodStart,
                        infoAboutPaymentCommunalData.CounterValuePeriodEnd,
                        infoAboutPaymentCommunalData.Accrual,
                        infoAboutPaymentCommunalData.Payed,
                        infoAboutPaymentCommunalData.Debt,
                        infoAboutPaymentCommunalData.TotalConsumption,
                        infoAboutPaymentCommunalData.AccrualByProvider,
                        infoAboutPaymentCommunalData.PayedToProvider,
                        infoAboutPaymentCommunalData.DebtToProvider,
                        infoAboutPaymentCommunalData.ReceivedPenaltySum
                    });
            }
        }
    }
}