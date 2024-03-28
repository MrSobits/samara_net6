namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class WorkRepairDetailViewModel : BaseViewModel<WorkRepairDetail>
    {
        public override IDataResult List(IDomainService<WorkRepairDetail> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");

            var groupWorkPprId = baseParams.Params.GetAs<long>("groupWorkPprId");

            var data = domainService.GetAll()
                .Where(x => x.BaseService.Id == baseServiceId && x.WorkPpr.GroupWorkPpr.Id == groupWorkPprId)
                .Select(x => new
                {
                    x.Id,
                    x.WorkPpr.Name
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}