namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    public class RegOpPersAccMuViewModel : BaseViewModel<RegOpPersAccMunicipality>
    {
        public override IDataResult List(IDomainService<RegOpPersAccMunicipality> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var regOperatorId = loadParams.Filter.GetAs<long>("regOpId");

            var data = domainService.GetAll()
                .Where(x => x.RegOperator.Id == regOperatorId)
                .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name,
                        x.OwnerFio,
                        x.PaidContributions,
                        x.CreditContributions,
                        x.CreditPenalty,
                        x.PaidPenalty,
                        x.SubsidySumLocalBud,
                        x.SubsidySumSubjBud,
                        x.SubsidySumFedBud,
                        x.SumAdopt,
                        x.RepaySumAdopt,
                        x.PersAccountNum
                    })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}