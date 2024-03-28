namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using Domain;
    using Domain.Extensions;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Overhaul.Domain;
    using Domain.Repository;
    using Entities;
    using Entities.Dict;
    using Castle.Windsor;
    using B4;
    using B4.IoC;

    using Bars.Gkh.Repositories.ChargePeriod;

    public class RealityObjectSubsidyService : IRealityObjectSubsidyService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetPlanSubsidyOperations(BaseParams baseParams)
        {
            var subsidyAccountDomain = Container.ResolveDomain<RealityObjectSubsidyAccount>();
            using (Container.Using(subsidyAccountDomain))
            {
                var loadParams = baseParams.GetLoadParam().ReplaceOrder("DateString", "Date");
                var accountId = loadParams.Filter.GetAsId("accId");
                var account = subsidyAccountDomain.Get(accountId);
                if (account == null)
                {
                    return new ListDataResult();
                }

                var result = GetListPlanSubsidyOperations(account);

                var data = result
                    .AsQueryable()
                    .Filter(loadParams, Container)
                    .Order(loadParams);

                return new ListDataResult(data.Paging(loadParams), data.Count());
            }           
        }

        public List<PlanSubsidyOperationProxy> GetListPlanSubsidyOperations(RealityObjectSubsidyAccount account)
        {
            var chargePeriodRepository = Container.Resolve<IChargePeriodRepository>();
            var federalStandardFeeCrDomain = Container.ResolveDomain<FederalStandardFeeCr>();
            var paysizeRepo = Container.Resolve<IPaysizeRepository>();

            var result = new List<PlanSubsidyOperationProxy>();

            try
            {
                var area = account.RealityObject.AreaLivingNotLivingMkd.ToDecimal();

                var federalStandardFeeCr = federalStandardFeeCrDomain.GetAll()
                    .Where(x => !x.DateEnd.HasValue || x.DateEnd > account.DateOpen)
                    .ToList();

                var startPeriodDate = new DateTime(account.DateOpen.Year, account.DateOpen.Month, 1);
                var endPeriodDate = startPeriodDate.AddMonths(1).AddDays(-1);
                var endDate = chargePeriodRepository.GetCurrentPeriod().GetEndDate();

                while (startPeriodDate < endDate)
                {
                    var tariffFeeList = new List<TariffFeeProxy>();
                    var dateString = "{0} {1}".FormatUsing(startPeriodDate.ToString("MMMM"), startPeriodDate.Year);

                    for (var date = startPeriodDate; date <= endPeriodDate; date = date.AddDays(1))
                    {
                        var currentTariff =
                            paysizeRepo.GetRoPaysizeByType(account.RealityObject, date)
                            ?? paysizeRepo.GetRoPaysizeByMu(account.RealityObject, date)
                            ?? 0m;

                        var currentFee = federalStandardFeeCr
                            .Where(x => x.DateStart <= date)
                            .FirstOrDefault(x => !x.DateEnd.HasValue || x.DateEnd >= date)
                            .Return(x => x.Value);

                        tariffFeeList.Add(new TariffFeeProxy
                        {
                            Fee = currentFee,
                            Tariff = currentTariff
                        });
                    }

                    tariffFeeList
                        .GroupBy(x => new {x.Fee, x.Tariff})
                        .ForEach(x => result.Add(new PlanSubsidyOperationProxy
                        {
                            DateString = dateString,
                            FederalStandardFee = x.Key.Fee,
                            Tariff = x.Key.Tariff,
                            Area = area,
                            Days = x.Count(),
                            Sum = (x.Key.Fee - x.Key.Tariff)*area*(x.Count().ToDecimal()/tariffFeeList.Count)
                        }));

                    startPeriodDate = startPeriodDate.AddMonths(1);
                    endPeriodDate = startPeriodDate.AddMonths(1).AddDays(-1);
                }

                return result;
            }
            finally
            {
                Container.Release(paysizeRepo);
                Container.Release(federalStandardFeeCrDomain);
                Container.Release(chargePeriodRepository);
            }
        }

        private class TariffFeeProxy
        {
            public decimal Tariff { get; set; }
            public decimal Fee { get; set; }
        }
    }
}