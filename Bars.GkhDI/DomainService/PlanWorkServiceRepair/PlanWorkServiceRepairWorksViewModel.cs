namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;
    using Entities;

    public class PlanWorkServiceRepairWorksViewModel : BaseViewModel<PlanWorkServiceRepairWorks>
    {
        public override IDataResult List(IDomainService<PlanWorkServiceRepairWorks> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var planWorkServiceRepairId = baseParams.Params.GetAs<long>("planWorkServiceRepairId");

            var data = domainService.GetAll()
                .Where(x => x.PlanWorkServiceRepair.Id == planWorkServiceRepairId)
                .Select(x => new
                    {
                        x.Id,
                        x.DataComplete,
                        WorkRepairListName = x.WorkRepairList.GroupWorkPpr.Name,
                        PeriodicityTemplateService = x.PeriodicityTemplateService.Name,
                        x.DateComplete,
                        x.DateStart,
                        x.DateEnd,
                        x.Cost,
                        x.FactCost,
                        x.ReasonRejection 
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<PlanWorkServiceRepairWorks> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAs<long>("id"));

            return new BaseDataResult(new
                {
                    obj.Id,
                    obj.DataComplete,
                    obj.PeriodicityTemplateService,
                    obj.DateComplete,
                    obj.DateStart,
                    obj.DateEnd,
                    obj.Cost,
                    obj.FactCost,
                    obj.ReasonRejection,
                    WorkRepairListName = obj.WorkRepairList != null && obj.WorkRepairList.GroupWorkPpr != null ? obj.WorkRepairList.GroupWorkPpr.Name : ""
                });
        }
    }
}