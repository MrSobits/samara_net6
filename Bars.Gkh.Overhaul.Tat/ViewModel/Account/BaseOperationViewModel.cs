namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    using Entities;

    public class BaseOperationViewModel : BaseViewModel<BaseOperation>
    {
        public override IDataResult List(IDomainService<BaseOperation> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var bankStatId = loadParams.Filter.GetAs<long>("bankStatId");
            var accountId = baseParams.Params.GetAs<long>("accountId");

            var data = domainService.GetAll()
                .WhereIf(bankStatId > 0, x => x.BankStatement.Id == bankStatId)
                .WhereIf(accountId > 0, x => x.BankStatement.BankAccount.Id == accountId)
                .Select(x => new
                {
                    x.Id,
                    Operation = x.Operation.Name,
                    x.OperationDate,
                    x.Sum,
                    x.Receiver,
                    x.Payer,
                    x.Purpose
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}