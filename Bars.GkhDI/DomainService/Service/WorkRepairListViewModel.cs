﻿namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class WorkRepairListViewModel : BaseViewModel<WorkRepairList>
    {
        public override IDataResult List(IDomainService<WorkRepairList> domainService, BaseParams baseParams)
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
                    x.PlannedVolume,
                    x.FactCost,
                    x.FactVolume,
                    x.DateStart,
                    x.DateEnd
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}