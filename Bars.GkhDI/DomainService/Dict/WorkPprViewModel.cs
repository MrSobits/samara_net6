namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;

    using Entities;

    public class WorkPprViewModel : BaseViewModel<WorkPpr>
    {
        public override IDataResult List(IDomainService<WorkPpr> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var groupWorkPprId = baseParams.Params.GetAs<long>("groupWorkPprId");

            var data = domainService.GetAll()
                .WhereIf(groupWorkPprId > 0, x => x.GroupWorkPpr.Id == groupWorkPprId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GroupWorkPprName = x.GroupWorkPpr.Name,
                    Measure = x.Measure.Name
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}