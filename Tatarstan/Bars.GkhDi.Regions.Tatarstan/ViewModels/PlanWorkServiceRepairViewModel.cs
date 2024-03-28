namespace Bars.GkhDi.Regions.Tatarstan.ViewModels
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    using System.Linq;

    public class PlanWorkServiceRepairViewModel : GkhDi.DomainService.PlanWorkServiceRepairViewModel
    {
        public IDomainService<RepairService> RepairServiceService { get; set; }

        public override IDataResult Get(IDomainService<PlanWorkServiceRepair> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params["id"].To<long>();
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            var repairServiceData = RepairServiceService.GetAll().FirstOrDefault(x => x.Id == obj.BaseService.Id);

            return new BaseDataResult(new
            {
                obj.Id,
                obj.DisclosureInfoRealityObj,
                obj.BaseService,
                Name =
                    obj.BaseService != null && obj.BaseService.TemplateService != null
                        ? obj.BaseService.TemplateService.Name
                        : string.Empty,
                SumWorkTo = repairServiceData != null ? repairServiceData.SumWorkTo : null,
                SumFact = repairServiceData != null ? repairServiceData.SumFact : null,
                DateStart = repairServiceData != null ? repairServiceData.DateStart : null,
                DateEnd = repairServiceData != null ? repairServiceData.DateEnd : null,
                ProgressInfo = repairServiceData != null ? repairServiceData.ProgressInfo : null,
                RejectCause = repairServiceData != null ? repairServiceData.RejectCause : null
            });
        }
    }
}