namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class PlanRedExpWorksViewModel : BaseViewModel<PlanReductionExpenseWorks>
    {
        public override IDataResult List(IDomainService<PlanReductionExpenseWorks> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var planReductionExpenseId = baseParams.Params.GetAs<long>("planReductionExpenseId");

            var data = domainService
                .GetAll()
                .Where(x => x.PlanReductionExpense.Id == planReductionExpenseId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DateComplete,
                    x.PlannedReductionExpense,
                    x.FactedReductionExpense,
                    x.ReasonRejection 
                }).Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}