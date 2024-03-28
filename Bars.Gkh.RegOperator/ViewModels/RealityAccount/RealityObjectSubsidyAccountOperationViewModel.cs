namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Domain;

    public class RealityObjectSubsidyAccountOperationViewModel : BaseViewModel<RealityObjectSubsidyAccountOperation>
    {
        public override IDataResult List(IDomainService<RealityObjectSubsidyAccountOperation> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var accountId = loadParams.Filter.GetAsId("accId");

            var data =
                domainService.GetAll()
                    .Where(x => x.Account.Id == accountId)
                    .Select(x => new
                    {
                        x.Id,
                        AccId = x.Account.Id,
                        x.OperationType,
                        x.Date,
                        x.OperationSum
                    })
                    .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}