namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Report.Fucking731;

    using Castle.Windsor;

    public class WorkRepairDetailService : IWorkRepairDetailService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddWorks(BaseParams baseParams)
        {
            try
            {
                var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");
                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',');

                var service = this.Container.Resolve<IDomainService<WorkRepairDetail>>();

                // получаем у контроллера работы что бы не добавлять их повторно
                var exsistingWorkRepairDetail = service
                    .GetAll()
                    .Where(x => x.BaseService.Id == baseServiceId)
                    .Select(x => x.WorkPpr.Id)
                    .ToList();

                foreach (var id in objectIds)
                {
                    if (exsistingWorkRepairDetail.Contains(id.ToLong()))
                    {
                        continue;
                    }
                    var newId = id.ToLong();

                    var newWorkRepairDetail = new WorkRepairDetail
                    {
                        WorkPpr = new WorkPpr { Id = newId },
                        BaseService = new BaseService { Id = baseServiceId }
                    };

                    service.Save(newWorkRepairDetail);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        public Dictionary<long, Dictionary<long, DisclosureInfo731.ObjectRepairServiceDetail[]>> GetRepairDetailsDict(
            IQueryable<long> diroIdQuery)
        {
            var result = Container.ResolveDomain<WorkRepairDetail>().GetAll()
                .Where(y => diroIdQuery.Contains(y.BaseService.DisclosureInfoRealityObj.Id))
                .Where(x => x.WorkPpr.GroupWorkPpr != null)
                .Select(x => new
                {
                    RealityObjectId = x.BaseService.DisclosureInfoRealityObj.RealityObject.Id,
                    BaseServiceId = x.BaseService.Id,
                    x.WorkPpr.Name,
                    UnitMeasure = x.WorkPpr.Measure.Name,
                    GroupWorkPprId = x.WorkPpr.GroupWorkPpr.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.BaseServiceId)
                .ToDictionary(
                    z => z.Key,
                    z => z.GroupBy(y => y.GroupWorkPprId)
                          .ToDictionary(
                              y => y.Key,
                              y => y.Select(x => new DisclosureInfo731.ObjectRepairServiceDetail
                              {
                                  Name = x.Name,
                                  UnitMeasure = x.UnitMeasure
                              })
                              .ToArray()));

            return result;
        }
    }
}
