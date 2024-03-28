namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class SpecialAccountOperationViewModel : BaseViewModel<SpecialAccountOperation>
    {
        public override IDataResult List(IDomainService<SpecialAccountOperation> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var specialAccountId = baseParams.Params.GetAs<long>("specialAccountId");

            var data = domainService.GetAll()
                .Where(x => x.Account.Id == specialAccountId)
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