namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using Bars.B4.Utils;
    using Entities.Hcs;

    public class HouseOverallBalanceViewModel : BaseViewModel<HouseOverallBalance>
    {
        public override IDataResult List(IDomainService<HouseOverallBalance> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var realtyObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var date = baseParams.Params.GetAs<DateTime>("date");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == realtyObjectId)
                .WhereIf(date != DateTime.MinValue, 
                            x => x.DateCharging.Month == date.Month
                                && x.DateCharging.Year == date.Year)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject,
                    x.DateCharging,
                    x.InnerBalance,
                    x.MonthCharge,
                    x.Payment,
                    x.Paid,
                    x.OuterBalance,
                    x.Service,
                    x.CorrectionCoef,
                    x.HouseExpense,
                    x.AccountsExpense,
                    x.DateCharging.Year,
                    x.DateCharging.Month,
                    Date = x.DateCharging.ToString("MM.yyyy")
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data
                    .Order(loadParams)
                    .OrderByDescending(x => x.Year)
                    .ThenByDescending(x => x.Month)
                    .AsEnumerable(), data.Count());
        }
    }
}