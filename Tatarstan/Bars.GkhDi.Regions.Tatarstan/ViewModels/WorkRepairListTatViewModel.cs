namespace Bars.GkhDi.Regions.Tatarstan.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Regions.Tatarstan.Entities;

    public class WorkRepairListTatViewModel : BaseViewModel<WorkRepairListTat>
    {
        public override IDataResult List(IDomainService<WorkRepairListTat> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");

            var data = domainService.GetAll()
                .Where(x => x.BaseService.Id == baseServiceId)
                .Select(x => new
                {
                    x.Id,
                    x.GroupWorkPpr.Name,
                    GroupWorkPpr = x.GroupWorkPpr.Id,
                    x.PlannedCost,
                    x.FactCost,
                    x.DateStart,
                    x.DateEnd,
                    x.InfoAboutExec,
                    x.ReasonRejection
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}