namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class AccrualsAccountOperationViewModel : BaseViewModel<AccrualsAccountOperation>
    {
        public override IDataResult List(IDomainService<AccrualsAccountOperation> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var accrualsAccountId = baseParams.Params.GetAs<long>("accrualsAccountId");

            var data = domainService.GetAll()
                .Where(x => x.Account.Id == accrualsAccountId)
                .Select(x => new
                {
                    x.Id,
                    x.Account,
                    x.AccrualDate,
                    x.ClosingBalance,
                    x.OpeningBalance,
                    x.TotalCredit,
                    x.TotalDebit
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}