namespace Bars.Gkh.Repair.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Entities.RepairControlDate;

    public class PerformedRepairWorkActViewModel : BaseViewModel<PerformedRepairWorkAct>
    {
        public IDomainService<RepairObject> RepairObjectDomainService { get; set; }

        public IDomainService<RepairControlDate> RepairControlDateDomainService { get; set; }

        public override IDataResult List(IDomainService<PerformedRepairWorkAct> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var repairObjectId = baseParams.Params.GetAs("repairObjectId", 0L);

            if (repairObjectId == 0)
            {
                return new ListDataResult(new List<object>(), 0);
            }

            var data = domainService.GetAll()
                .Where(x => x.RepairWork.RepairObject.Id == repairObjectId)
                .Select(
                    x => new
                    {
                        x.Id,
                        WorkName = x.RepairWork.Work.Name,
                        x.PerformedWorkVolume,
                        x.ActSum
                    })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<PerformedRepairWorkAct> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", 0L);
            var obj = domainService.Get(id);

            return new BaseDataResult(
                new
                {
                    obj.Id,
                    ObjectAddress = obj.RepairWork.RepairObject.RealityObject.Address,
                    obj.RepairWork,
                    WorkName = obj.RepairWork.Work.Name,
                    obj.ObjectPhoto,
                    obj.ObjectPhotoDescription,
                    obj.ActDate,
                    obj.ActNumber,
                    obj.PerformedWorkVolume,
                    obj.ActSum,
                    obj.ActFile,
                    obj.ActDescription
                });
        }
    }
}