namespace Bars.Gkh.Repair.ViewModel
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Entities.RepairControlDate;

    public class RepairWorkViewModel : BaseViewModel<RepairWork>
    {
        public IDomainService<RepairObject> RepairObjectDomainService { get; set; }

        public IDomainService<RepairControlDate> RepairControlDateDomainService { get; set; }

        public override IDataResult List(IDomainService<RepairWork> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var repairObjectId = baseParams.Params.GetAs("repairObjectId", 0L);

            if (repairObjectId == 0)
            {
                return new ListDataResult(new List<object>(), 0);
            }

            var repairProgramId = RepairObjectDomainService.Get(repairObjectId).RepairProgram.Id;

            var dictControlDate = RepairControlDateDomainService.GetAll()
                                            .Where(x => x.RepairProgram.Id == repairProgramId)
                                            .Select(x => new { x.Work.Id, x.Date })
                                            .ToDictionary(x => x.Id, x => x.Date);

            var data = domainService.GetAll()
                .Where(x => x.RepairObject.Id == repairObjectId)
                .Select(x => new
                {
                    x.Id,
                    x.Work.TypeWork,
                    WorkId = x.Work.Id,
                    WorkName = x.Work.Name,
                    x.Volume,
                    x.Sum,
                    x.VolumeOfCompletion,
                    x.PercentOfCompletion,
                    x.CostSum,
                    UnitMeasure = x.Work.UnitMeasure.ShortName,
                    BuilderName = x.Builder,
                    x.DateStart,
                    x.DateEnd,
                    x.AdditionalDate
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.TypeWork,
                    x.WorkName,
                    x.Volume,
                    x.Sum,
                    x.VolumeOfCompletion,
                    x.PercentOfCompletion,
                    x.CostSum,
                    x.UnitMeasure,
                    x.BuilderName,
                    x.DateStart,
                    x.DateEnd,
                    x.AdditionalDate,
                    ControlDate = dictControlDate.ContainsKey(x.WorkId) ? dictControlDate[x.WorkId] : null
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<RepairWork> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", 0L);
            var obj = domainService.Get(id);

            return new BaseDataResult(new
            {
                obj.Id,
                obj.RepairObject,
                obj.Work,
                UnitMeasure = obj.Work.UnitMeasure.ShortName,
                TypeWork = obj.Work.TypeWork.GetEnumMeta().Display,
                obj.Volume,
                obj.Sum,
                obj.DateStart,
                obj.DateEnd,
                obj.VolumeOfCompletion,
                obj.PercentOfCompletion,
                obj.CostSum,
                obj.Builder
            });
        }
    }
}