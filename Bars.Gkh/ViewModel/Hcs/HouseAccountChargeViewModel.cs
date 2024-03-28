namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;
    using Entities.Hcs;
    
    public class HouseAccountChargeViewModel : BaseViewModel<HouseAccountCharge>
    {
        public override IDataResult List(IDomainService<HouseAccountCharge> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var accountId = baseParams.Params.GetAs<long>("accountId");

            var data = domain.GetAll()
                .Where(x => x.Account.Id == accountId)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Service,
                    x.Tariff,
                    x.Expense,
                    x.CompleteCalc,
                    x.Underdelivery,
                    x.Charged,
                    x.Recalc,
                    x.InnerBalance,
                    x.Changed,
                    x.Payment,
                    x.ChargedPayment,
                    x.OuterBalance,
                    x.DateCharging.Year,
                    x.DateCharging.Month,
                    x.Supplier,
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