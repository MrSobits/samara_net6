using System.Linq;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.ViewModel
{
    using Bars.B4;

    public class BuildContractClwViolViewModel : BaseViewModel<BuildContractClwViol>
    {
        public override IDataResult List(IDomainService<BuildContractClwViol> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var builderId = loadParams.Filter.GetAs("builderId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ClaimWork.Id == builderId)
                .Select(x => new
                {
                    x.Id,
                    ClaimWork = x.ClaimWork.Id,
                    Violation = x.Violation.Name,
                    x.Note
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}