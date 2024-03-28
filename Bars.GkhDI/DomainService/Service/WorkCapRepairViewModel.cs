namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class WorkCapRepairViewModel : BaseViewModel<WorkCapRepair>
    {
        public override IDataResult List(IDomainService<WorkCapRepair> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");

            var data = domainService.GetAll()
                .Where(x => x.BaseService.Id == baseServiceId)
                .Select(x => new
                {
                    x.Id,
                    x.Work.Name,
                    WorkId = x.Work.Id,
                    x.PlannedCost,
                    x.PlannedVolume,
                    x.FactedCost,
                    x.FactedVolume
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}