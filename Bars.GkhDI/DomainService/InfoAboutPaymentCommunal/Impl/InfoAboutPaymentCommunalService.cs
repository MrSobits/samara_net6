namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class InfoAboutPaymentCommunalService : IInfoAboutPaymentCommunalService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult SaveInfoAboutPaymentCommunal(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
                var records = baseParams.Params["records"]
                    .As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>().ReadClass<InfoAboutPaymentCommunal>())
                    .ToList();

                var service = this.Container.Resolve<IDomainService<InfoAboutPaymentCommunal>>();

                var existingInfoAboutPaymentCommunalList = service.GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                        .AsEnumerable()
                        .GroupBy(x => x.BaseService.Id)
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                foreach (var rec in records)
                {
                    InfoAboutPaymentCommunal existingInfoAboutPaymentCommunal = null;

                    if (existingInfoAboutPaymentCommunalList.ContainsKey(rec.Id))
                        existingInfoAboutPaymentCommunal = existingInfoAboutPaymentCommunalList[rec.Id];

                    if (existingInfoAboutPaymentCommunal != null)
                    {
                        existingInfoAboutPaymentCommunal.CounterValuePeriodStart = rec.CounterValuePeriodStart;
                        existingInfoAboutPaymentCommunal.CounterValuePeriodEnd = rec.CounterValuePeriodEnd;
                        existingInfoAboutPaymentCommunal.Accrual = rec.Accrual;
                        existingInfoAboutPaymentCommunal.Payed = rec.Payed;
                        existingInfoAboutPaymentCommunal.Debt = rec.Debt;
                        existingInfoAboutPaymentCommunal.TotalConsumption = rec.TotalConsumption;
                        existingInfoAboutPaymentCommunal.AccrualByProvider = rec.AccrualByProvider;
                        existingInfoAboutPaymentCommunal.PayedToProvider = rec.PayedToProvider;
                        existingInfoAboutPaymentCommunal.DebtToProvider = rec.DebtToProvider;
                        existingInfoAboutPaymentCommunal.ReceivedPenaltySum = rec.ReceivedPenaltySum;

                        service.Update(existingInfoAboutPaymentCommunal);
                    }
                    else
                    {
                        var newInfoAboutPaymentCommunal = new InfoAboutPaymentCommunal
                        {
                            DisclosureInfoRealityObj = new DisclosureInfoRealityObj { Id = disclosureInfoRealityObjId },
                            BaseService = new BaseService { Id = rec.Id },
                            CounterValuePeriodStart = rec.CounterValuePeriodStart,
                            CounterValuePeriodEnd = rec.CounterValuePeriodEnd,
                            Accrual = rec.Accrual,
                            Payed = rec.Payed,
                            Debt = rec.Debt,
                            TotalConsumption = rec.TotalConsumption,
                            AccrualByProvider = rec.AccrualByProvider,
                            PayedToProvider = rec.PayedToProvider,
                            DebtToProvider = rec.DebtToProvider,
                            ReceivedPenaltySum = rec.ReceivedPenaltySum
                        };

                        service.Save(newInfoAboutPaymentCommunal);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}
