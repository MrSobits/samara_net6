namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class SpecialAccountViewModel : BaseViewModel<SpecialAccount>
    {
        public override IDataResult List(IDomainService<SpecialAccount> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var roId = baseParams.Params.GetAs<long>("roId");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    CreditOrganization = x.CreditOrganization.Name,
                    AccountOwner = x.AccountOwner.Name,
                    x.Number,
                    x.OpenDate,
                    x.CloseDate,
                    x.TotalIncome,
                    x.TotalOut
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}