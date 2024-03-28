namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;

    using Castle.Windsor;

    public class RegOpAccountService : IRegOpAccountService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var regopId = baseParams.Params.GetAs<long>("regopId");

            var realAccounts = Container.Resolve<IDomainService<RealAccount>>();
            var odpDecisions = Container.Resolve<IDomainService<RegOpAccountDecision>>();
            var accrualsAccountDomain = Container.Resolve<IDomainService<AccrualsAccount>>();

            // Получаем общую сумму исходящего сальдо по всем объектам ДПКР через счета начислений
            var now = DateTime.Now.Date;
            var dictAccuralsAccountLongTerm =
                accrualsAccountDomain.GetAll()
                    .Where(x => x.OpenDate <= now && (!x.CloseDate.HasValue || x.CloseDate >= now))
                    .Select(x => new { RoId = x.RealityObject.Id, BalanceOut = x.OpeningBalance })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.BalanceOut));

            var data =
                realAccounts.GetAll()
                    .Where(y => odpDecisions.GetAll()
                        .Where(x => x.RealityObject.Id == y.RealityObject.Id)
                        .Any(x => x.RegOperator.Id == regopId))
                    .Select(x => new
                    {
                        x.Id,
                        x.RealityObject.Address,
                        AccountNumber = x.Number,
                        DateOpen = x.OpenDate,
                        DateClose = x.CloseDate,
                        CreditTotal = x.TotalOut,
                        DebetTotal = x.TotalIncome,
                        BalanceIn = x.Balance,
                        BalanceOut =
                            dictAccuralsAccountLongTerm.ContainsKey(x.RealityObject.Id)
                                ? dictAccuralsAccountLongTerm[x.RealityObject.Id]
                                : 0
                    })
                    .Filter(loadParams, Container);

            var dataList = data.ToList();
            var summary = new
                {
                    CreditTotal = dataList.Sum(x => x.CreditTotal),
                    DebetTotal = dataList.Sum(x => x.DebetTotal),
                    BalanceIn = dataList.Sum(x => x.BalanceIn),
                    BalanceOut = dataList.Sum(x => x.BalanceOut)
                };

            return new ListSummaryResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count(), summary);
        }
    }
}