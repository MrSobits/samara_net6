namespace Bars.GkhDi.Regions.Tatarstan.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Regions.Tatarstan.Entities;

    public class PlanRedExpWorksViewModel : GkhDi.DomainService.PlanRedExpWorksViewModel
    {
        public IDomainService<PlanReduceMeasureName> PlanReduceMeasureNameDomain { get; set; }

        public override IDataResult List(IDomainService<PlanReductionExpenseWorks> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var planReductionExpenseId = baseParams.Params.GetAs<long>("planReductionExpenseId");

            var planReductionExpenseWorksQuery = domainService
                .GetAll()
                .Where(x => x.PlanReductionExpense.Id == planReductionExpenseId);

            var measureNames = PlanReduceMeasureNameDomain.GetAll()
                .Where(x => planReductionExpenseWorksQuery.Select(y => y.Id).Contains(x.PlanReductionExpenseWorks.Id))
                .Select(x => new { x.PlanReductionExpenseWorks.Id, x.MeasuresReduceCosts.MeasureName })
                .ToDictionary(x => x.Id, x => x.MeasureName);

            var data = planReductionExpenseWorksQuery
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DateComplete,
                    x.PlannedReductionExpense,
                    x.FactedReductionExpense,
                    x.ReasonRejection 
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    Name = measureNames.ContainsKey(x.Id) ? measureNames[x.Id] : x.Name,
                    x.DateComplete,
                    x.PlannedReductionExpense,
                    x.FactedReductionExpense,
                    x.ReasonRejection 
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);
            
            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}