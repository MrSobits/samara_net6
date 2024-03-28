namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class RealAccountViewModel : BaseViewModel<RealAccount>
    {
        public override IDataResult List(IDomainService<RealAccount> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var roId = baseParams.Params.GetAs<long>("roId");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    AccountOwner = x.AccountOwner.Name,
                    x.Number,
                    x.OpenDate,
                    x.CloseDate,
                    x.AccountType,
                    x.OverdraftLimit,
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}