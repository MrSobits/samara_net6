namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.Domain;

    using Entities;

    public class PerformedWorkActRecordViewModel : BaseViewModel<PerformedWorkActRecord>
    {
        public override IDataResult List(IDomainService<PerformedWorkActRecord> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var performedWorkActId = baseParams.Params.GetAs("performedWorkActId", loadParams.Filter.GetAsId("performedWorkActId"));

            //если true, то показывать только те записи, которые по всем полям соответствуют записям в смете по этому виду работы
            var showEstimateMatch = baseParams.Params.GetAs("showEstimMatch", loadParams.Filter.GetAs("showEstimMatch", false));

            if (showEstimateMatch && performedWorkActId > 0)
            {
                var result = new List<PerformedWorkActRecord>();

                var work = Container.Resolve<IDomainService<PerformedWorkAct>>().Load(performedWorkActId).TypeWorkCr;

                var estimates = new List<Estimate>();

                if (work != null)
                {
                    estimates = Container.Resolve<IDomainService<Estimate>>()
                                 .GetAll()
                                 .Where(x => x.EstimateCalculation.TypeWorkCr.Id == work.Id)
                                 .Filter(loadParams, Container)
                                 .ToList();
                }

                var acts = domainService.GetAll().Where(x => x.PerformedWorkAct.Id == performedWorkActId)
                    .Filter(loadParams, Container)
                    .Order(loadParams)
                    .ToList();

                foreach (var act in acts)
                {
                    var estim1 = estimates
                        .FirstOrDefault(estim => 
                            estim.Name == act.Name
                            && estim.Number == act.Number
                            && estim.Reason == act.Reason
                            && estim.MechanicSalary == act.MechanicSalary
                            && estim.BaseSalary == act.BaseSalary
                            && estim.MechanicWork == act.MechanicWork
                            && estim.BaseWork == act.BaseWork
                            && estim.TotalCount == act.TotalCount
                            && estim.TotalCost == act.TotalCost
                            && estim.OnUnitCount == act.OnUnitCount
                            && estim.OnUnitCost == act.OnUnitCost
                            && estim.MaterialCost == act.MaterialCost
                            && estim.MachineOperatingCost == act.MachineOperatingCost
                            && estim.UnitMeasure == act.UnitMeasure);

                    if (estim1 != null)
                    {
                         result.Add(act);
                    }
                }

                return new ListDataResult(result.AsQueryable().Paging(loadParams).ToList(), result.Count);
            }

            var data = domainService.GetAll()
                .Where(x => x.PerformedWorkAct.Id == performedWorkActId)
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    x.Reason,
                    x.Name,
                    x.UnitMeasure,
                    x.MechanicSalary,
                    x.BaseSalary,
                    x.MechanicWork,
                    x.BaseWork,
                    x.TotalCount,
                    x.TotalCost,
                    x.OnUnitCount,
                    x.OnUnitCost,
                    x.MaterialCost,
                    x.MachineOperatingCost
                })
               .Filter(loadParams, this.Container);

            var totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}