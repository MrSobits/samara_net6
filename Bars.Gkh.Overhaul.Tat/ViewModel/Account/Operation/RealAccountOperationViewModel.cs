namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class RealAccountOperationViewModel : BaseViewModel<RealAccountOperation>
    {
        public override IDataResult List(IDomainService<RealAccountOperation> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var realAccountId = baseParams.Params.GetAs<long>("realAccountId");

            var data = domainService.GetAll()
                .Where(x => x.Account.Id == realAccountId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.OperationDate,
                    x.Payer,
                    x.Purpose,
                    x.Receiver,
                    x.Sum,
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}