namespace Bars.GkhDi.Services.Impl
{
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetPlanWorkServiceRepairResponse GetPlanWorkServiceRepair(string houseId, string periodId)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }

            var idHouse = houseId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idHouse != 0 && idPeriod != 0)
            {
                var diRealObj = Container.Resolve<IDomainService<DisclosureInfoRealityObj>>()
                    .GetAll()
                    .FirstOrDefault(x => x.RealityObject.Id == idHouse && x.PeriodDi.Id == idPeriod);

                if (diRealObj == null)
                {
                    return new GetPlanWorkServiceRepairResponse { Result = Result.DataNotFound };
                }

                var planWorkServiceRepairIds = Container.Resolve<IDomainService<PlanWorkServiceRepair>>()
                             .GetAll()
                             .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                             .Select(x => x.Id)
                             .ToList();

                var repairPlans = Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>()
                             .GetAll()
                             .Where(x => planWorkServiceRepairIds.Contains(x.PlanWorkServiceRepair.Id))
                             .Select(x => new RepairPlan
                                     {
                                         Id = x.Id,
                                         Name = x.WorkRepairList.GroupWorkPpr.Name,
                                         Period = x.PeriodicityTemplateService.Name,
                                         Cost = x.Cost.HasValue ? x.Cost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                         FactCost = x.FactCost.HasValue ? x.FactCost.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                         ReasonReject = x.ReasonRejection,
                                         DateStart = x.DateStart.HasValue ? x.DateStart.Value.ToShortDateString() : null,
                                         DateEnd = x.DateEnd.HasValue ? x.DateEnd.Value.ToShortDateString() : null
                                     })
                             .ToArray();

                return new GetPlanWorkServiceRepairResponse { RepairPlans = repairPlans, Result = Result.NoErrors };
            }

            return new GetPlanWorkServiceRepairResponse { Result = Result.DataNotFound };
        }
    }
}
