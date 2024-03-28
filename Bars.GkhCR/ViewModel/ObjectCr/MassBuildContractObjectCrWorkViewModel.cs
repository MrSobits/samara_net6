using Bars.Gkh.DataResult;

namespace Bars.GkhCr.ViewModel
{
    using System.Linq;
    using B4;
    using Bars.Gkh.Domain;
    using Entities;

    public class MassBuildContractObjectCrWorkViewModel : BaseViewModel<MassBuildContractObjectCrWork>
    {
        public override IDataResult List(IDomainService<MassBuildContractObjectCrWork> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var buildContracCRtId = baseParams.Params.GetAsId("buildContractCRId");

            var data = domainService.GetAll()
                .Where(x => x.MassBuildContractObjectCr.Id == buildContracCRtId)
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