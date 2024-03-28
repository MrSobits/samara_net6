using Bars.Gkh.DataResult;

namespace Bars.GkhCr.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class BuildContractTypeWorkViewModel : BaseViewModel<BuildContractTypeWork>
    {
        public override IDataResult List(IDomainService<BuildContractTypeWork> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var buildContractId = loadParams.Filter.GetAs<long>("buildContractId");

            var data = domainService.GetAll()
                .Where(x => x.BuildContract.Id == buildContractId)
                .Select(x => new
                {
                    x.Id,
                    TypeWork = x.TypeWork.Work.Name,
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