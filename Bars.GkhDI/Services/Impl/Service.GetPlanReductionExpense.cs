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
        public GetPlanReductionExpenseResponse GetPlanReductionExpense(string houseId, string periodId)
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
                    return new GetPlanReductionExpenseResponse { Result = Result.DataNotFound };
                }

                var planReductionExpenseIds =
                    Container.Resolve<IDomainService<PlanReductionExpense>>().GetAll()
                        .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                        .Select(x => x.Id)
                        .ToList();

                var arrangments = 
                    Container.Resolve<IDomainService<PlanReductionExpenseWorks>>().GetAll()
                        .Where(x => planReductionExpenseIds.Contains(x.PlanReductionExpense.Id))
                        .Select(x => new Arrangement
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Period = x.DateComplete.HasValue ? x.DateComplete.Value.ToShortDateString() : null,
                            PlannedReductionExpense = x.PlannedReductionExpense.HasValue ? x.PlannedReductionExpense.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                            FactedReductionExpense = x.FactedReductionExpense.HasValue ? x.FactedReductionExpense.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                            RejectReason = x.ReasonRejection
                        })
                        .ToArray();

                return new GetPlanReductionExpenseResponse { PlanReductionExpenses = arrangments, Result = Result.NoErrors };
            }

            return new GetPlanReductionExpenseResponse { Result = Result.DataNotFound };
        }
    }
}
