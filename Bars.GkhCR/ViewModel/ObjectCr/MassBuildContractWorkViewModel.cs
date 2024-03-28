using Bars.Gkh.DataResult;

namespace Bars.GkhCr.ViewModel
{
    using System.Linq;
    using B4;
    using Bars.Gkh.Domain;
    using Entities;

    public class MassBuildContractWorkViewModel : BaseViewModel<MassBuildContractWork>
    {
        public override IDataResult List(IDomainService<MassBuildContractWork> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var buildContractId = baseParams.Params.GetAsId("buildContractId");

            var data = domainService.GetAll()
                .Where(x => x.MassBuildContract.Id == buildContractId)
                .Select(x => new
                {
                    x.Id,
                    Work = x.Work.Name,
                    x.Sum
                })
                .Filter(loadParams, this.Container);

            var summary = data.Sum(x => x.Sum);
            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListSummaryResult(data.ToList(), totalCount, new { Sum = summary });
        }
    }
}