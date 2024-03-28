namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    public class WorkRepairListService : IWorkRepairListService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListSelected(BaseParams baseParams)
        {
            var realityObjId = baseParams.Params.GetAs<long>("realityObjId");
            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var disclosureInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>().Load(disclosureInfoId);

            // Получаем все группы работ ППР забитые в справочники
            var workGroupPprList = this.Container.Resolve<IDomainService<GroupWorkPpr>>().GetAll().Where(x => !x.IsNotActual).ToList();

            // Получаем все шаблонные услуги с типом КР
            var repairTemplServiceList =
                this.Container.Resolve<IDomainService<TemplateService>>().GetAll()
                    .Where(x => x.KindServiceDi == KindServiceDi.Repair)
                    .ToList();

            // Получаем все услуги с типом КР у данного дома
            var repairServiceList = this.Container.Resolve<IDomainService<RepairService>>()
                .GetAll()
                .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == realityObjId
                            && x.DisclosureInfoRealityObj.PeriodDi.Id == disclosureInfo.PeriodDi.Id)
                .ToList();

            // Получаем все работы (на основе группы ППР) услуги ремонт по данному дому
            var workRepairService = this.Container.Resolve<IDomainService<WorkRepairList>>();
            var workRepairList = workRepairService
                .GetAll()
                .Where(x => x.BaseService.DisclosureInfoRealityObj.RealityObject.Id == realityObjId
                            && x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id == disclosureInfo.PeriodDi.Id)
                .ToList();

            var data = new List<object>();
            foreach (var repairTemplService in repairTemplServiceList)
            {
                var existRepair = false;
                foreach (var repairService in repairServiceList)
                {
                    if (repairTemplService.Id == repairService.TemplateService.Id)
                    {
                        existRepair = true;
                        foreach (var workGroupPpr in
                            workGroupPprList.Where(x => x.Service != null && x.Service.Id == repairTemplService.Id))
                        {
                            var existWorkPPR = false;
                            foreach (var workRepair in workRepairList)
                            {
                                if (workRepair.GroupWorkPpr != null && workRepair.GroupWorkPpr.Id == workGroupPpr.Id && workRepair.BaseService.Id == repairService.Id)
                                {
                                    existWorkPPR = true;
                                    data.Add(
                                        new
                                        {
                                            workRepair.Id,
                                            GroupWorkPprId = workGroupPpr.Id,
                                            ServiceId = repairService.Id,
                                            ServiceName = repairService.TemplateService.Name,
                                            ProviderName =
                                                repairService.Provider != null
                                                    ? repairService.Provider.Name
                                                    : string.Empty,
                                            workGroupPpr.Name,
                                            workRepair.PlannedCost,
                                            workRepair.PlannedVolume,
                                            workRepair.FactCost,
                                            workRepair.FactVolume,
                                            workRepair.DateStart,
                                            workRepair.DateEnd,
                                            TypeColor = 0
                                        });
                                }
                            }

                            if (!existWorkPPR)
                            {
                                // Добавляем работы готовые на создание(нет их менее серые)
                                data.Add(
                                    new
                                    {
                                        GroupWorkPprId = workGroupPpr.Id,
                                        ServiceId = repairService.Id,
                                        ServiceName = repairService.TemplateService.Name,
                                        ProviderName =
                                            repairService.Provider != null
                                                ? repairService.Provider.Name
                                                : string.Empty,
                                        workGroupPpr.Name,
                                        TypeColor = 1
                                    });
                            }
                        }
                    }
                }

                if (!existRepair)
                {
                    // Добавляем серые работы по шаблонной услуге repairTemplService
                    foreach (var workGroupPpr in workGroupPprList)
                    {
                        data.Add(
                            new
                            {
                                GroupWorkPprId = workGroupPpr.Id,
                                workGroupPpr.Name,
                                ServiceName = repairTemplService.Name,
                                TypeColor = 2
                            });
                    }
                }
            }

            return new ListDataResult(data.ToList(), data.Count);
        }
        
        public IDataResult SaveWorkPpr(BaseParams baseParams)
        {
            try
            {
                var service = this.Container.Resolve<IDomainService<WorkRepairList>>();
                var realityObjId = baseParams.Params.GetAs<long>("realityObjId");
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var records = baseParams.Params.GetAs<List<WorkProxy>>("records");

                var periodId =
                    Container.Resolve<IDomainService<DisclosureInfo>>()
                             .GetAll()
                             .Where(x => x.Id == disclosureInfoId)
                             .Select(x => x.PeriodDi.Id)
                             .FirstOrDefault();

                var workRepairListDict = service.
                    GetAll().
                    Where(x => x.BaseService.DisclosureInfoRealityObj.RealityObject.Id == realityObjId && x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id == periodId).
                    ToDictionary(x => x.Id);

                foreach (var rec in records.Where(x => x.ServiceId > 0))
                {
                    if (rec.Id > 0)
                    {
                        if (!workRepairListDict.ContainsKey(rec.Id))
                        {
                            continue;
                        }

                        var workRepairList = workRepairListDict[rec.Id];

                        workRepairList.PlannedVolume = rec.PlannedVolume;
                        workRepairList.PlannedCost = rec.PlannedCost;
                        workRepairList.FactVolume = rec.FactVolume;
                        workRepairList.FactCost = rec.FactCost;
                        workRepairList.DateStart = rec.DateStart;
                        workRepairList.DateEnd = rec.DateEnd;

                        if (workRepairList.Id > 0)
                        {
                            service.Update(workRepairList);
                        }
                    }
                    else
                    {
                        service.Save(new WorkRepairList
                        {
                            Id = 0,
                            BaseService = new BaseService { Id = rec.ServiceId },
                            GroupWorkPpr = new GroupWorkPpr { Id = rec.GroupWorkPprId },
                            PlannedVolume = rec.PlannedVolume,
                            PlannedCost = rec.PlannedCost,
                            FactVolume = rec.FactVolume,
                            FactCost = rec.FactCost,
                            DateStart = rec.DateStart,
                            DateEnd = rec.DateEnd
                        });
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }


        public IDataResult HasDetailAllWorkRepair(BaseParams baseParams)
        {
            var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");

            var workRepairServ = Container.Resolve<IDomainService<WorkRepairList>>();
            var workRepairDetServ = Container.Resolve<IDomainService<WorkRepairDetail>>();

            var allCount = workRepairServ.GetAll().Where(x => x.BaseService.Id == baseServiceId).Select(x => x.GroupWorkPpr.Id).Count();

            var existDetail = workRepairDetServ
                .GetAll()
                .Where(x => x.BaseService.Id == baseServiceId && workRepairServ.GetAll().Any(y => y.BaseService.Id == baseServiceId && y.GroupWorkPpr.Id == x.WorkPpr.GroupWorkPpr.Id))
                .Select(x => x.WorkPpr.GroupWorkPpr.Id)
                .Distinct()
                .Count();

            return new BaseDataResult(allCount == existDetail);
        }

        private class WorkProxy
        {
            public long Id { get; set; }

            public long ServiceId { get; set; }

            public long GroupWorkPprId { get; set; }

            public decimal? PlannedCost { get; set; }

            public decimal? PlannedVolume { get; set; }

            public decimal? FactCost { get; set; }

            public decimal? FactVolume { get; set; }

            public DateTime? DateStart { get; set; }

            public DateTime? DateEnd { get; set; }
        }
    }
}
