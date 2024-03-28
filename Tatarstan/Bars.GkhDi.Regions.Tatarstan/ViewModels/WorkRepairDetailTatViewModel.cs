namespace Bars.GkhDi.Regions.Tatarstan.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Regions.Tatarstan.Entities;

    public class WorkRepairDetailTatViewModel : BaseViewModel<WorkRepairDetailTat>
    {
        public override IDataResult List(IDomainService<WorkRepairDetailTat> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");
            var groupWorkPprId = baseParams.Params.GetAs<long>("groupWorkPprId");

            var data = domainService.GetAll()
                .Where(x => x.BaseService.Id == baseServiceId && x.WorkPpr.GroupWorkPpr.Id == groupWorkPprId)
                .Select(x => new
                {
                    x.Id,
                    x.WorkPpr.Name,
                    UnitMeasure = (long?)x.UnitMeasure.Id,
                    x.FactVolume,
                    x.PlannedVolume
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}