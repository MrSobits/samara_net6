namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    using Entities;

    public class AccBankStatementViewModel : BaseViewModel<AccBankStatement>
    {
        public override IDataResult List(IDomainService<AccBankStatement> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var accountId = loadParams.Filter.GetAs<long>("accountId");
            var isSuspenseAcc = loadParams.Filter.GetAs<bool>("IsSuspenseAcc");

            var data = domainService.GetAll()
                .WhereIf(accountId > 0, x => x.BankAccount.Id == accountId)
                .WhereIf(isSuspenseAcc, x => x.BankAccount == null)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}