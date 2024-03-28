namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;
    using B4;

    using Entities.Hcs;

    public class HouseAccountViewModel : BaseViewModel<HouseAccount>
    {
        public override IDataResult List(IDomainService<HouseAccount> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var houseAccChargeService = Container.Resolve<IDomainService<HouseAccountCharge>>();

            var roId = baseParams.Params.GetAs<long>("realityObjectId");

            var querySum = houseAccChargeService.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Where(x => x.DateCharging.Year == DateTime.Now.Year)
                .Select(x => new
                {
                    x.Payment,
                    x.Charged,
                    Debt = x.Charged - x.Payment,
                    AccountId = x.Account.Id
                });

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    x.HouseAccountNumber,
                    x.OpenAccountDate,
                    x.CloseAccountDate,
                    x.PaymentCode,
                    x.Apartment,
                    x.Living,
                    Payment = (decimal?) querySum.Where(y => y.AccountId == x.Id).Sum(y => y.Payment),
                    Charged = (decimal?) querySum.Where(y => y.AccountId == x.Id).Sum(y => y.Charged),
                    Debt = (decimal?) querySum.Where(y => y.AccountId == x.Id).Sum(y => y.Debt)
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}