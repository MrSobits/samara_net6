namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class WorkRepairTechServViewModel : BaseViewModel<WorkRepairTechServ>
    {
        public override IDataResult List(IDomainService<WorkRepairTechServ> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var showOnlyGroups = baseParams.Params.GetAs("showOnlyGroups", false);
            var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");

            if (showOnlyGroups)
            {
                var groupsdata = domainService.GetAll()
                .Where(x => x.BaseService.Id == baseServiceId)
                .GroupBy(x => x.WorkTo.GroupWorkTo.Name)
                .Select(x => new { GroupName = x.Key })
                .Order(loadParams)
                .Paging(loadParams)
                .ToList();

                return new ListDataResult(groupsdata, groupsdata.Count);
            }

            var data = domainService.GetAll()
                .Where(x => x.BaseService.Id == baseServiceId)
                .Select(x => new
                {
                    x.Id,
                    x.WorkTo.Name,
                    GroupName = x.WorkTo.GroupWorkTo.Name
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}