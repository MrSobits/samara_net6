namespace Bars.GkhDi.Regions.Tatarstan.DomainService
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Regions.Tatarstan.Entities;
    using Bars.GkhDi.Regions.Tatarstan.Entities.Dict;
    using System.Linq;
    using Castle.Windsor;

    public class PlanReduceMeasureNameService : IPlanReduceMeasureNameService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PlanReductionExpenseWorks> PlanReductionExpenseWorksDomain { get; set; }

        public IDataResult AddPlanReduceMeasureName(BaseParams baseParams)
        {
            try
            {
                var planReductionExpenseId = baseParams.Params["planReductionExpenseId"].ToLong();
                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',').Select(x => x.Trim().ToLong()).ToList();

                var service = this.Container.Resolve<IDomainService<PlanReduceMeasureName>>();

                 //получаем у контроллера услуги что бы не добавлять их повторно
                var exsistingPlanReductionExpense = service.GetAll()
                    .Where(x => x.PlanReductionExpenseWorks.PlanReductionExpense.Id == planReductionExpenseId)
                    .Select(x => x.MeasuresReduceCosts.Id)
                    .ToList();

                foreach (var id in objectIds)
                {
                    if (exsistingPlanReductionExpense.Contains(id))
                    {
                        continue;
                    }

                    var newPlanReductionExpenseWork = new PlanReductionExpenseWorks
                    {
                        PlanReductionExpense = new PlanReductionExpense { Id = planReductionExpenseId }
                    };

                    PlanReductionExpenseWorksDomain.Save(newPlanReductionExpenseWork);

                    var newPlanReductionExpenseMeasure = new PlanReduceMeasureName
                    {
                        MeasuresReduceCosts = new MeasuresReduceCosts { Id = id },
                        PlanReductionExpenseWorks = newPlanReductionExpenseWork
                    };

                    service.Save(newPlanReductionExpenseMeasure);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}
