namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class AccrualsAccountOperationViewModel : BaseViewModel<AccrualsAccountOperation>
    {
        public override IDataResult List(IDomainService<AccrualsAccountOperation> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var accountId = baseParams.Params.GetAs<long>("accountId");

            var data = domainService.GetAll()
                .Where(x => x.Account.Id == accountId)
                .Select(x => new
                {
                    x.Id,
                    Account = x.Account.Id,
                    x.AccrualDate,
                    x.ClosingBalance,
                    x.OpeningBalance,
                    x.TotalOut,
                    x.TotalIncome
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}