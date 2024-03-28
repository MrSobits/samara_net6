namespace Bars.GkhDi.Regions.Tatarstan.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Regions.Tatarstan.Entities;
    using Bars.GkhDi.Report.Fucking731;

    using Castle.Windsor;

    public class WorkRepairDetailTatService : IWorkRepairDetailService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddWorks(BaseParams baseParams)
        {
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                var service = Container.Resolve<IDomainService<WorkRepairDetailTat>>();
                var workPprService = Container.Resolve<IDomainService<WorkPpr>>();

                try
                {
                    var baseServiceId = baseParams.Params.GetAs<long>("baseServiceId");

                    // получаем у уже существующие работы, чтобы не добавлять их повторно
                    var exsistingWorkRepairDetail = service.GetAll()
                        .Where(x => x.BaseService.Id == baseServiceId)
                        .Select(x => x.WorkPpr.Id)
                        .ToList();

                    var objectIds =
                        baseParams.Params.GetAs<string>("objectIds")
                            .Split(',')
                            .Select(x => x.ToLong())
                            .Except(exsistingWorkRepairDetail)
                            .ToArray();

                    var worksPpr =
                        workPprService.GetAll()
                            .Where(x => objectIds.Contains(x.Id))
                            .ToArray();

                    foreach (var workPpr in worksPpr)
                    {
                        var newWorkRepairDetail = new WorkRepairDetailTat
                        {
                            WorkPpr = workPpr,
                            BaseService = new BaseService {Id = baseServiceId},
                            UnitMeasure = workPpr.Measure
                        };

                        service.Save(newWorkRepairDetail);
                    }

                    tr.Commit();
                }
                catch (ValidationException exc)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, exc.Message);
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(service);
                    Container.Release(workPprService);
                }
            }

            return new BaseDataResult();
        }

        public Dictionary<long, Dictionary<long, DisclosureInfo731.ObjectRepairServiceDetail[]>> GetRepairDetailsDict(IQueryable<long> diroIdQuery)
        {
            var result = Container.ResolveDomain<WorkRepairDetailTat>().GetAll()
                .Where(y => diroIdQuery.Contains(y.BaseService.DisclosureInfoRealityObj.Id))
                .Where(x => x.WorkPpr.GroupWorkPpr != null)
                .Select(x => new
                {
                    RealityObjectId = x.BaseService.DisclosureInfoRealityObj.RealityObject.Id,
                    BaseServiceId = x.BaseService.Id,
                    x.WorkPpr.Name,
                    UnitMeasure = x.WorkPpr.Measure.Name,
                    GroupWorkPprId = x.WorkPpr.GroupWorkPpr.Id,
                    x.FactVolume,
                    x.PlannedVolume
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
                                  UnitMeasure = x.UnitMeasure,
                                  FactVolume = x.FactVolume ?? 0,
                                  Volume = x.PlannedVolume ?? 0
                              })
                              .ToArray()));

            return result;
        }
    }
}