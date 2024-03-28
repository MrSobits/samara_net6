namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class PlanReductionExpenseInterceptor : EmptyDomainInterceptor<PlanReductionExpense>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<PlanReductionExpense> service, PlanReductionExpense entity)
        {
            var planReductionExpenseWorksService = this.Container.Resolve<IDomainService<PlanReductionExpenseWorks>>();
            var planReductionExpenseWorksIds = planReductionExpenseWorksService.GetAll().Where(x => x.PlanReductionExpense.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in planReductionExpenseWorksIds)
            {
                planReductionExpenseWorksService.Delete(id);
            }

            return Success();
        }
    }
}
