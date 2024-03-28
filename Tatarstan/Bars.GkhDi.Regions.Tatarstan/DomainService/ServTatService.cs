namespace Bars.GkhDi.Regions.Tatarstan.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;
    using Bars.GkhDi.Regions.Tatarstan.Entities;

    public class ServTatService : ServService
    {
        public IRepository<WorkRepairDetailTat> WorkRepairDetailTatRepository { get; set; }

        public IRepository<WorkRepairListTat> WorkRepairListTatRepository { get; set; }

        protected Dictionary<long, List<WorkRepairDetailTat>> DictWorkRepairDetailTat = new Dictionary<long, List<WorkRepairDetailTat>>(); // int - baseServiceId

        protected Dictionary<long, List<WorkRepairListTat>> DictWorkRepairListTat = new Dictionary<long, List<WorkRepairListTat>>(); // int - baseServiceId

        protected List<WorkRepairDetailTat> ListSaveWorkRepairDetailTat = new List<WorkRepairDetailTat>();

        protected List<WorkRepairListTat> ListSaveWorkRepairListTat = new List<WorkRepairListTat>();

        protected override void InitWorkRepairList(List<long> periodIds, InitParams initParams, long manorgId, List<long> realityObjIds = null)
        {
            if (initParams.All || initParams.WorkRepairList)
        {
                DictWorkRepairListTat = WorkRepairListTatRepository
                .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id).ToDictionary(x => x.Key, y => y.ToList());
            }
            }

        protected override void InitWorkRepairDetail(List<long> periodIds, ServService.InitParams initParams, long manorgId, List<long> realityObjIds = null)
            {
            if (initParams.All || initParams.WorkRepairDetail)
            {
                DictWorkRepairDetailTat = WorkRepairDetailTatRepository
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }
            }

        protected override void CopyWorkRepairList(RepairService newService, long oldServiceId)
        {
            var planWorkServiceRepair = this.CreatePlanWorkServiceRepair(newService, oldServiceId);

            if (!DictWorkRepairListTat.ContainsKey(oldServiceId))
                {
                return;
            }

            foreach (var workRl in DictWorkRepairListTat[oldServiceId])
            {
                var workRepairList = new WorkRepairListTat
            {
                    Id = 0,
                    BaseService = newService,
                    GroupWorkPpr = workRl.GroupWorkPpr,
                    PlannedCost = workRl.PlannedCost,
                    FactCost = workRl.FactCost,
                    DateStart = workRl.DateStart,
                    DateEnd = workRl.DateEnd,
                    InfoAboutExec = workRl.InfoAboutExec,
                    ReasonRejection = workRl.ReasonRejection,
                };

                ListSaveWorkRepairListTat.Add(workRepairList);

                if (planWorkServiceRepair != null)
                { 
                    // Создаем ППР для плана на основе ППР услуги
                    var planWorkServiceRepairWork = new PlanWorkServiceRepairWorks
                    {
                        PlanWorkServiceRepair = planWorkServiceRepair,
                        WorkRepairList = workRepairList,
                        Cost = workRepairList.PlannedCost,
                        FactCost = workRepairList.FactCost,
                        DateStart = workRepairList.DateStart,
                        DateEnd = workRepairList.DateEnd,
                        ReasonRejection = workRepairList.ReasonRejection,
                        DataComplete = workRepairList.InfoAboutExec
                };

                    // Если в копируемом периоде были ППР плана, то перезатираем значения выше
                    if (DictPlanWorkServiceRepairWorks.ContainsKey(oldServiceId)
                        && DictPlanWorkServiceRepairWorks[oldServiceId].ContainsKey(workRepairList.GroupWorkPpr.Id))
        {
                        var planWork = DictPlanWorkServiceRepairWorks[oldServiceId][workRepairList.GroupWorkPpr.Id];

                        planWorkServiceRepairWork.Cost = planWork.Cost;
                        planWorkServiceRepairWork.FactCost = planWork.FactCost;
                        planWorkServiceRepairWork.DateStart = planWork.DateStart;
                        planWorkServiceRepairWork.DateEnd = planWork.DateEnd;
                        planWorkServiceRepairWork.DateComplete = planWork.DateComplete;
                        planWorkServiceRepairWork.DataComplete = planWork.DataComplete;
                        planWorkServiceRepairWork.PeriodicityTemplateService = planWork.PeriodicityTemplateService;
                        planWorkServiceRepairWork.ReasonRejection = planWork.ReasonRejection;
                            }
                            
                    this.ListSavePlanWorkServiceRepairWorks.Add(planWorkServiceRepairWork);
                        }
                    }
                    }

        protected override void CopyWorkRepairDetail(BaseService newService, long oldServiceId)
                    {
            if (!DictWorkRepairDetailTat.ContainsKey(oldServiceId))
                        {
                return;
                        }

            foreach (var workRd in DictWorkRepairDetailTat[oldServiceId])
                    {
                ListSaveWorkRepairDetailTat.Add(new WorkRepairDetailTat
                        {
                    Id = 0,
                    BaseService = newService,
                    WorkPpr = workRd.WorkPpr,
                    UnitMeasure = workRd.UnitMeasure,
                    PlannedVolume = workRd.PlannedVolume,
                    FactVolume = workRd.FactVolume
                });
                        }
                    }

        protected override void SaveWorkRepairList()
                    {
            this.InTransaction(ListSaveWorkRepairListTat, WorkRepairListTatRepository);

            DictWorkRepairListTat.Clear();
            this.ListSaveWorkRepairListTat.Clear();
                        }

        protected override void SaveWorkRepairDetail()
                    {
            this.InTransaction(ListSaveWorkRepairDetailTat, WorkRepairDetailTatRepository);

            DictWorkRepairDetailTat.Clear();
            this.ListSaveWorkRepairDetailTat.Clear();
                        }

        protected override TypeOfProvisionServiceDi GetTypeOfProvisionService(RepairService oldService, long newPeriodId)
            {
            if (oldService.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedWithoutMo)
        {
                var oldPeriodLess2013 = oldService.DisclosureInfoRealityObj.PeriodDi.DateStart == null
                    || oldService.DisclosureInfoRealityObj.PeriodDi.DateStart.Value.Year < 2013;

                var newPeriodNotLess2013 = DictPeriodDi.ContainsKey(newPeriodId) 
                    && DictPeriodDi[newPeriodId].DateStart.HasValue
                    && DictPeriodDi[newPeriodId].DateStart.Value.Year >= 2013;

                if (oldPeriodLess2013 && newPeriodNotLess2013)
        {
                    return TypeOfProvisionServiceDi.ServiceProvidedMo;
                }
            }

            return oldService.TypeOfProvisionService;
        }
    }
}
