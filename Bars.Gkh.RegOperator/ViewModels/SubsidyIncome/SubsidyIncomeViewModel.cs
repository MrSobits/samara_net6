namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Enums;

    public class SubsidyIncomeViewModel : BaseViewModel<SubsidyIncome>
    {
        public override IDataResult List(IDomainService<SubsidyIncome> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var showConfirmed = baseParams.Params.GetAs<bool>("showConfirmed", ignoreCase: true);
            var showDeleted = baseParams.Params.GetAs<bool>("showDeleted", ignoreCase: true);
            
            var data = domainService.GetAll()
                .WhereIf(!showConfirmed, x => x.DistributeState != DistributionState.Distributed)
                .WhereIf(!showDeleted, x => x.DistributeState != DistributionState.Deleted)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToArray(), data.Count());
        }
    }
}
